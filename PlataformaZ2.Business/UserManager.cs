////-----------------------------------------------------------------------
//// <copyright file="UserManager.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Data.Util;
    using PlataformaZ2.Data.DataAccess;
    using PlataformaZ2.Data.Repository;
    using PlataformaZ2.Model.Dto;
    using PlataformaZ2.Model.Util;
    using System.Text.RegularExpressions;
    using PlataformaZ2.Model.Exception;
    using System.IO;
    using PlataformaZ2.Business.Util;

    /// <summary>
    /// Manages business rules related to users
    /// </summary>
    public static class UserManager
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Basic Operations (user creation, login and password operations)

        /// <summary>
        /// Authenticate user by AccessToken (verify token and permissions)
        /// </summary>
        /// <param name="accessToken">User's accessToken</param>
        /// <param name="neededPermissions">User's accessToken</param>
        /// <returns>Authentication result</returns>
        public static bool AuthenticateByAcessToken(string accessToken, Permissions[] neededPermissions)
        {
            UserRepository userRepository = new UserRepository();
            UserDao userDao = userRepository.GetByAcessToken(accessToken);

            // Check whether the user exists and has a valid token (within an amount of hours from token's creation date)
            if (userDao == null || DateTime.Now > userDao.AccessTokenCreationDate.Value.AddHours(ApplicationConfiguration.AccessTokenExpirationPeriod))
            {
                return false;
            }

            // Check permissions by user's profile
            foreach (var neededPermission in neededPermissions)
            {
                // Verify whether the user has this needed permission
                PermissionDao permissionDao = new PermissionRepository().GetById(Convert.ToInt32(neededPermission));

                // the user must have all needed permissions
                if (!userDao.Profile.Permissions.Contains(permissionDao))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if username and password are valid for login
        /// </summary>
        /// <param name="credentials">User's credentials (username and password)</param>
        /// <returns>UserSession object</returns>
        public static UserSessionDto Login(CredentialsDto credentials)
        {
            UserRepository userRepository = new UserRepository();
            string encryptedPassword = Encrypt(credentials.Password);

            if (credentials == null)
            {
                throw new BusinessException("Sem dados de login");
            }
            else if(string.IsNullOrWhiteSpace(credentials.Username))
            {
                throw new BusinessException("Sem dados de usuário");
            }
            else if (string.IsNullOrWhiteSpace(credentials.Password))
            {
                throw new BusinessException("Sem dados de senha");
            }

            var userDao = userRepository.MatchCredentials(credentials.Username, encryptedPassword);

            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("Usuário ou senha inválidos");
            }

            ////Generate AccessToken (if user does not have one or if it is expired)
            if (userDao.AccessToken == null
                || (userDao.AccessTokenCreationDate.HasValue
                    && DateTime.Now >= userDao.AccessTokenCreationDate.Value.AddHours(ApplicationConfiguration.AccessTokenExpirationPeriod))
               )
            {
                // generate new "access token" for the user                
                Guid guid = Guid.NewGuid();
                string accessToken = Convert.ToBase64String(guid.ToByteArray());
                accessToken = accessToken.Replace("=", "1");
                accessToken = accessToken.Replace("+", "2");
                accessToken = accessToken.Replace("/", "0");

                userDao.AccessToken = accessToken;
                userDao.AccessTokenCreationDate = DateTime.Now;

                //update user's data with new token
                userRepository.Update(userDao);
            }

            ////Return user's session
            UserSessionDto session = new UserSessionDto()
            {
                IdUser = userDao.Id.Value,
                Username = userDao.Username,
                Name = userDao.Name,
                Nickname = userDao.Nickname,
                IdProfile = userDao.Profile.Id.Value,
                AccessToken = userDao.AccessToken
            };

            if (userDao.Photo != null)
            {
                session.Photo = FileManager.ReadAsBase64Image(userDao.Photo.RealName);
            }            

            return session;
        }

        /// <summary>
        /// Sign-up a new user with password
        /// </summary>
        /// <param name="userSignUpDto">User's data</param>
        /// <returns>Success Message</returns>
        public static string SignUpUser(UserSignUpDto userSignUpDto)
        {
            UserRepository userRepository = new UserRepository();
            FileRepository fileRepository = new FileRepository();

            //// Validate user's data and whether there is another user with the same parameters         
            ValidateUser(null, userSignUpDto.Username, userSignUpDto.Name, userSignUpDto.Nickname, userSignUpDto.Cpf);
            
            //// Validate password's rules           
            ValidatePassword(userSignUpDto.Password);
            
            //// Create new user
            UserDao newUser = new UserDao()
            {
                Username = userSignUpDto.Username,
                Password = Encrypt(userSignUpDto.Password),
                Name = userSignUpDto.Name,
                Nickname = userSignUpDto.Nickname,
                Cpf = userSignUpDto.Cpf,                
                Active = true
            };            

            if (userSignUpDto.Photo != null)
            {
                try
                {
                    var uniqueName = FileManager.SaveBase64Image(userSignUpDto.Photo);

                    FileDao fileDao = new FileDao()
                    {
                        Name = newUser.Nickname + Path.GetExtension(uniqueName),
                        RealName = uniqueName
                    };

                    fileRepository.Create(fileDao);
                    newUser.Photo = fileRepository.GetById(fileDao.Id.Value);
                }
                catch (Exception)
                {
                    // cancel operation
                    userRepository.RollbackTransaction();
                    throw new BusinessException("Não foi possível salvar a imagem do usuário");
                }
            }

            // set the profile as "basic user"
            newUser.Profile = new ProfileRepository().GetById(1);

            // save the user
            userRepository.Create(newUser);

            //// Send welcome e-mail 
            try
            {
                MailManager.SendWelcomeEmail(newUser.Username, newUser.Nickname);

                return "Usuário cadastrado com sucesso";
            }
            catch (Exception)
            {
                return "Usuário cadastrado com sucesso, mas não foi possível enviar o e-mail de boas-vindas";
            }
        }

        /// <summary>
        /// Generates token and sends an e-mail to redefine the password
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>Success Message</returns>
        public static string ForgotPassword(string email)
        {
            UserRepository userRepository = new UserRepository();
            PasswordDefinitionRepository passwordDefinitionRepository = new PasswordDefinitionRepository();

            ////Check user
            var userDao = userRepository.GetByUsername(email);

            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("Não existe usuário cadastrado com esse e-mail");
            }

            ////Check if exists a valid token and expires it                        
            PasswordDefinitionDao old = passwordDefinitionRepository.SearchForValidToken(userDao.Id.Value);

            if (old != null)
            {
                old.ExpirationDate = DateTime.Now;
                passwordDefinitionRepository.Update(old);
            }

            ////Generate "change password" token
            Guid guid = Guid.NewGuid();
            string changePasswordToken = Convert.ToBase64String(guid.ToByteArray());
            changePasswordToken = changePasswordToken.Replace("=", "1");
            changePasswordToken = changePasswordToken.Replace("+", "2");
            changePasswordToken = changePasswordToken.Replace("/", "0");            

            ////Register the new token
            PasswordDefinitionDao passwordDefinitionDao = new PasswordDefinitionDao()
            {
                Token = changePasswordToken,
                User = userDao,
                CreationDate = DateTime.Now                
            };

            passwordDefinitionDao.ExpirationDate = passwordDefinitionDao.CreationDate.AddDays(3);

            passwordDefinitionRepository.Create(passwordDefinitionDao);

            ////Send e-mail
            MailManager.SendForgotPasswordEmail(userDao.Username, userDao.Nickname, userDao.Id.Value, changePasswordToken);

            int middleIndex = userDao.Username.IndexOf("@");
            string maskedEmail = userDao.Username.Substring(0, 3) + "****" + userDao.Username.Substring(middleIndex, 4) + "***";

            return string.Format("E-mail enviado para {0}", maskedEmail);
        }

        /// <summary>
        /// Generates token and sends an e-mail of first password creation
        /// </summary>
        /// <param name="idUser">User identifier</param>
        public static void PasswordFirstCreation(int idUser)
        {
            UserRepository userRepository = new UserRepository();
            PasswordDefinitionRepository passwordDefinitionRepository = new PasswordDefinitionRepository();

            var userDao = userRepository.GetById(idUser);

            ////Check if exists a valid token and expires it (should not exist, as it is first password creation)                       
            PasswordDefinitionDao old = passwordDefinitionRepository.SearchForValidToken(userDao.Id.Value);

            if (old != null)
            {
                old.ExpirationDate = DateTime.Now;
                passwordDefinitionRepository.Update(old);
            }

            ////Generate "change password" token
            Guid guid = Guid.NewGuid();
            string changePasswordToken = Convert.ToBase64String(guid.ToByteArray());
            changePasswordToken = changePasswordToken.Replace("=", "1");
            changePasswordToken = changePasswordToken.Replace("+", "2");
            changePasswordToken = changePasswordToken.Replace("/", "0");

            ////Register the new token
            PasswordDefinitionDao passwordDefinitionDao = new PasswordDefinitionDao()
            {
                Token = changePasswordToken,
                User = userDao,
                CreationDate = DateTime.Now
            };

            passwordDefinitionDao.ExpirationDate = passwordDefinitionDao.CreationDate.AddDays(3);

            passwordDefinitionRepository.Create(passwordDefinitionDao);

            ////Send e-mail
            MailManager.SendPasswordCreationEmail(userDao.Username, userDao.Nickname, idUser, changePasswordToken);
        }

        /// <summary>
        /// Checks if the link is valid for password change (the user exists and the token is not expired)
        /// </summary>
        /// <param name="changeInfo">Info for password change (password is left blank)</param>
        /// <returns>User object</returns>
        public static UserFullDto ValidatePasswordLink(ChangePasswordDto changeInfo)
        {
            UserRepository userRepository = new UserRepository();
            PasswordDefinitionRepository passwordDefinitionRespository = new PasswordDefinitionRepository();

            UserDao userDao = userRepository.GetById(changeInfo.IdUser);

            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("O usuário não existe");
            }

            ////Check if the token is valid for password change            
            PasswordDefinitionDao passwordDefinitionDao = passwordDefinitionRespository.SearchForValidToken(userDao.Id.Value);

            if (passwordDefinitionDao == null || passwordDefinitionDao.Token != changeInfo.ChangePasswordToken)
            {
                throw new BusinessException("Link expirado para troca de senha. Solicite novamente.");                
            }

            ////The link is valid (the user exists and the token is not expired)

            // return success and user's data to be displayed (not completed DTO)
            UserFullDto userParcialData = new UserFullDto()
            {
                Id = userDao.Id,
                Username = userDao.Username,
                Name = userDao.Name,
                Nickname = userDao.Nickname,
                IdProfile = userDao.Profile.Id.Value,
                ProfileName = userDao.Profile.Name
            };

            return userParcialData;
        }

        /// <summary>
        /// Defines/Redefines the password for a non-logged user (using password's token)
        /// </summary>
        /// <param name="changeInfo">Info for password change</param>
        public static void ChangePasswordUsingToken(ChangePasswordDto changeInfo)
        {
            UserRepository userRepository = new UserRepository();
            PasswordDefinitionRepository passwordDefinitionRespository = new PasswordDefinitionRepository();

            if (changeInfo == null)
            {
                throw new BusinessException("Sem dados de senha");
            }

            //// validate link
            ValidatePasswordLink(changeInfo);

            //// validate password's rules           
            ValidatePassword(changeInfo.NewPassword);
            
            ////The link and password are valid: Encrypt the password, update the user and expire the token           
            var UserDao = userRepository.GetById(changeInfo.IdUser);
            UserDao.Password = Encrypt(changeInfo.NewPassword);
            userRepository.Update(UserDao);

            var passwordDefinitionDao = passwordDefinitionRespository.SearchForValidToken(UserDao.Id.Value);
            passwordDefinitionDao.ExpirationDate = DateTime.Now;
            passwordDefinitionRespository.Update(passwordDefinitionDao);
        }

        #endregion

        /// <summary>
        /// Redefines the password of a logged user (does not use password's token)
        /// </summary>
        /// <param name="loggedUser">Logged User</param>
        /// <param name="changeInfo">Info for password change (token is left blank)</param>
        public static void ChangePasswordForLoggedUser(UserDao loggedUser, ChangePasswordDto changeInfo)
        {
            UserRepository userRepository = new UserRepository();

            if (changeInfo == null)
            {
                throw new BusinessException("Sem dados de senha");
            }

            //// checks logged user permission
            if (loggedUser.Id != changeInfo.IdUser)
            {
                log.Warn("Usuário Id=" + loggedUser.Id.ToString() + " tentou entrar como " + changeInfo.IdUser.ToString());
                throw new PermissionException();
            }

            //// validate password's rules           
            ValidatePassword(changeInfo.NewPassword);
            
            //// changes user's password
            UserDao userDao = userRepository.GetById(changeInfo.IdUser);

            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("O usuário não existe");                
            }
            
            userDao.Password = Encrypt(changeInfo.NewPassword);  // Encrypt the password
            userRepository.Update(userDao);
        }

        /// <summary>
        /// Get an user session by username
        /// </summary>
        /// <param name="loggedUser">Logged User</param>
        /// <param name="username">User's username </param>
        /// <returns>UserSession object</returns>
        public static UserSessionDto GetUserSessionByUsername(UserDao loggedUser, string username)
        {
            UserRepository userRepository = new UserRepository();

            //// checks logged user permission
            if (!loggedUser.Username.Equals(username))
            {
                log.Warn("Usuário Id=" + loggedUser.Id.ToString() + " tentou entrar como " + username);
                throw new PermissionException();
            }

            //// gets user's session
            UserDao userDao = userRepository.GetByUsername(username);

            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("O usuário não existe");
            }

            UserSessionDto session = new UserSessionDto()
            {
                IdUser = userDao.Id.Value,
                Username = userDao.Username,
                Name = userDao.Name,
                Nickname = userDao.Nickname,
                IdProfile = userDao.Profile.Id.Value,
                AccessToken = userDao.AccessToken
            };

            if (userDao.Photo != null)
            {
                session.Photo = FileManager.ReadAsBase64Image(userDao.Photo.RealName);
            }

            return session;
        }

        /// <summary>
        /// Searches users refined by filter and pagination (according to logged user's permission)
        /// </summary>
        /// <param name="filter">Filter parameters to refine the search</param>
        /// <returns> PaginationResponse with List of Users </returns>
        public static PaginationResponseDto<UserListItemDto> Search(UserFilterDto filter)
        {
            UserRepository userRepository = new UserRepository();
            List<UserListItemDto> userDtoList = new List<UserListItemDto>();

            //// sets the quantity of "results per page" from webconfig (didn't come on the filter)
            filter.ResultsPerPage = ApplicationConfiguration.ResultsPerPage;
                        
            //// gets the users based on filter (including pagination)
            var paginationResponseFromRepository = userRepository.GetByFilter(filter);
            
            //// converts the list from DAO to DTO
            foreach (var userDao in paginationResponseFromRepository.Response)
            {
                UserListItemDto userDto = new UserListItemDto()
                {
                    Id = userDao.Id,
                    Name = userDao.Name,
                    ProfileName = userDao.Profile.Name
                };

                userDtoList.Add(userDto);
            }

            //// returns the search using the PaginationResponse model
            PaginationResponseDto<UserListItemDto> paginationResponseDto = new PaginationResponseDto<UserListItemDto>()
            {
                TotalResults = paginationResponseFromRepository.TotalResults,
                ResultsPerPage = filter.ResultsPerPage,
                CurrentPage = filter.PageNumber,
                Response = userDtoList
            };
           
            return paginationResponseDto;
        }

        /// <summary>
        /// Gets an user by its identifier
        /// </summary>
        /// <param name="loggedUser">Logged User</param>
        /// <param name="id">User identifier</param>
        /// <returns>User object</returns>
        public static UserFullDto GetUserById(UserDao loggedUser, int id)
        {
            UserRepository userRepository = new UserRepository();

            UserDao userDao = userRepository.GetById(id);
           
            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("O usuário não existe");
            }

            //// checks logged user permission
            if (!loggedUser.Profile.Permissions.Any(x => x.Id == (int)Permissions.ManageAllUsers))
            {
                // needs to be the "user" himself
                if (loggedUser.Id != id)
                {
                    log.Warn("Usuário Id=" + loggedUser.Id.ToString() + " tentou visualizar dados do usuário id=" + id.ToString());
                    throw new PermissionException();
                }
            }            

            //// converts from DAO to DTO
            UserFullDto userDto = new UserFullDto()
            {
                Id = userDao.Id,
                Username = userDao.Username,
                Name = userDao.Name,
                Nickname = userDao.Nickname,
                Cpf = userDao.Cpf,
                FormatedCpf = PropertyFormatter.FormatCpf(userDao.Cpf),
                IdProfile = userDao.Profile.Id.Value,
                ProfileName = userDao.Profile.Name                
            };

            if (userDao.Photo != null)
            {                
                userDto.Photo = FileManager.ReadAsBase64Image(userDao.Photo.RealName);
            }

            return userDto;
        }

        /// <summary>
        /// Saves an user (create or update)
        /// </summary>
        /// <param name="userDto">User data</param>
        /// <returns>Success Message</returns>
        public static string SaveUser(UserFullDto userDto)
        {
            UserRepository userRepository = new UserRepository();
            FileRepository fileRepository = new FileRepository();

            // validate user's data and whether there is another user with the same parameters            
            ValidateUser(userDto.Id, userDto.Username, userDto.Name, userDto.Nickname, userDto.Cpf);
            
            //// Checks if it is creation or update operation
            if (userDto.Id.HasValue)
            {
                //// update operation

                UserDao userDao = userRepository.GetById(userDto.Id.Value);

                if (userDao == null || !userDao.Active)
                {
                    throw new BusinessException("O usuário não existe");
                }

                // update data (including profile and CPF)
                userDao.Username = userDto.Username;
                userDao.Name = userDto.Name;
                userDao.Nickname = userDto.Nickname;
                userDao.Cpf = userDto.Cpf;

                userDao.Profile = new ProfileDao() { Id = userDto.IdProfile };
                
                // create/update/delete photo
                if (userDto.Photo != null)
                {
                    // the the DTO cames with value (so, it must be saved or updated)

                    if (userDao.Photo != null)
                    {
                        // the DAO has value, then update files and database reference
                        try
                        {
                            FileManager.DeleteFile(userDao.Photo.RealName);

                            var uniqueName = FileManager.SaveBase64Image(userDto.Photo);

                            userDao.Photo.Name = userDao.Nickname + Path.GetExtension(uniqueName);
                            userDao.Photo.RealName = uniqueName;
                        }
                        catch (Exception)
                        {
                            // cancel operation
                            userRepository.RollbackTransaction();
                            throw new BusinessException("Não foi possível atualizar a foto do usuário");
                        }
                    }
                    else
                    {
                        // the DAO doesn't have value, then create the file and link it to the database
                        try
                        {
                            var uniqueName = FileManager.SaveBase64Image(userDto.Photo);

                            FileDao fileDao = new FileDao()
                            {
                                Name = userDao.Nickname + Path.GetExtension(uniqueName),
                                RealName = uniqueName
                            };

                            fileRepository.Create(fileDao);
                            userDao.Photo = fileRepository.GetById(fileDao.Id.Value);
                        }
                        catch (Exception)
                        {
                            // cancel operation
                            userRepository.RollbackTransaction();
                            throw new BusinessException("Não foi possível salvar a foto do usuário");
                        }
                    }
                }
                else
                {
                    // the the DTO doesn't came with value (so, delete from server)

                    if (userDao.Photo != null)
                    {
                        // the DAO has value, then delete file and database reference
                        try
                        {
                            FileManager.DeleteFile(userDao.Photo.RealName);

                            fileRepository.Delete(userDao.Photo);
                            userDao.Photo = null;
                        }
                        catch (Exception)
                        {
                            // cancel operation
                            userRepository.RollbackTransaction();
                            throw new BusinessException("Não foi possível excluir a foto do usuário");
                        }
                    }
                }

                // save the user
                userRepository.Update(userDao);

                return "Usuário atualizado com sucesso";
            }
            else
            {
                //// creation operation

                UserDao newUser = new UserDao()
                {
                    Username = userDto.Username,
                    Name = userDto.Name,
                    Nickname = userDto.Nickname,
                    Cpf = userDto.Cpf,
                    Profile = new ProfileDao() { Id = userDto.IdProfile },
                    Active = true
                };

                // generate a temporary password (only to not leave the field empty at database)
                string temporaryPassword = string.Format(@"{0}", Guid.NewGuid()).Substring(0, 8);
                newUser.Password = Encrypt(temporaryPassword);
                
                // save the photo
                if (userDto.Photo != null)
                {
                    try
                    {
                        var uniqueName = FileManager.SaveBase64Image(userDto.Photo);

                        FileDao fileDao = new FileDao()
                        {
                            Name = newUser.Nickname + Path.GetExtension(uniqueName),
                            RealName = uniqueName
                        };

                        fileRepository.Create(fileDao);
                        newUser.Photo = fileRepository.GetById(fileDao.Id.Value);
                    }
                    catch (Exception)
                    {
                        // cancel operation
                        userRepository.RollbackTransaction();
                        throw new BusinessException("Não foi possível salvar a foto do usuário");
                    }
                }
                
                // save the user
                userRepository.Create(newUser);

                // Send "welcome" e-mail
                try
                {
                    MailManager.SendWelcomeEmail(newUser.Username, newUser.Nickname);
                }
                catch (Exception)
                {
                    // allow continue even if welcome e-mail does not work
                }

                // Send "First Password Creation" e-mail
                try
                {
                    PasswordFirstCreation(newUser.Id.Value);

                    return "Usuário cadastrado com sucesso. Um e-mail para criação de senha foi enviado.";
                }
                catch (Exception)
                {
                    return "Usuário cadastrado com sucesso, mas não foi possível enviar o e-mail de criação de senha";
                }
            }
        }

        /// <summary>
        /// Deletes an user (set as inactive)
        /// </summary>
        /// <param name="idUser">User identifier</param>
        public static void DeleteUser(int idUser)
        {
            UserRepository userRepository = new UserRepository();

            UserDao userDao = userRepository.GetById(idUser);
            
            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("O usuário não existe");
            }

            ////set user as inactive (do not delete from database)
            userDao.Active = false;

            ////save the user with the inactive status
            userRepository.Update(userDao);
        }

        /// <summary>
        /// Updates user's data (done by the user)
        /// </summary>
        /// <param name="loggedUser">Logged User</param>
        /// <param name="userDto">User data</param>
        public static void UpdateUserByUser(UserDao loggedUser, UserFullDto userDto)
        {
            UserRepository userRepository = new UserRepository();
            FileRepository fileRepository = new FileRepository();

            if (userDto == null)
            {
                throw new BusinessException("Sem dados do usuário");
            }
            else if (!userDto.Id.HasValue)
            {
                throw new BusinessException("Nenhum usuário selecionado");
            }

            //// checks logged user permission
            if (loggedUser.Id != userDto.Id)
            {
                log.Warn("Usuário Id=" + loggedUser.Id.ToString() + " tentou atualizar dados do " + userDto.Id.ToString());
                throw new PermissionException();
            }

            // validate user's data and whether there is another user with the same parameters            
            ValidateUser(userDto.Id, userDto.Username, userDto.Name, userDto.Nickname, userDto.Cpf);

            //// gets user's data
            UserDao userDao = userRepository.GetById(userDto.Id.Value);

            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("O usuário não existe");
            }

            // update data (except username, profile, CPF)
            userDao.Name = userDto.Name;
            userDao.Nickname = userDto.Nickname;

            // create/update/delete photo
            if (userDto.Photo != null)
            {
                // the the DTO cames with value (so, it must be saved or updated)

                if (userDao.Photo != null)
                {
                    // the DAO has value, then update files and database reference
                    try
                    {
                        FileManager.DeleteFile(userDao.Photo.RealName);

                        var uniqueName = FileManager.SaveBase64Image(userDto.Photo);

                        userDao.Photo.Name = userDao.Nickname + Path.GetExtension(uniqueName);
                        userDao.Photo.RealName = uniqueName;
                    }
                    catch (Exception)
                    {
                        // cancel operation
                        userRepository.RollbackTransaction();
                        throw new BusinessException("Não foi possível atualizar a foto do usuário");
                    }
                }
                else
                {
                    // the DAO doesn't have value, then create the file and link it to the database
                    try
                    {
                        var uniqueName = FileManager.SaveBase64Image(userDto.Photo);

                        FileDao fileDao = new FileDao()
                        {
                            Name = userDao.Nickname + Path.GetExtension(uniqueName),
                            RealName = uniqueName
                        };

                        fileRepository.Create(fileDao);
                        userDao.Photo = fileRepository.GetById(fileDao.Id.Value);
                    }
                    catch (Exception)
                    {
                        // cancel operation
                        userRepository.RollbackTransaction();
                        throw new BusinessException("Não foi possível salvar a foto do usuário");
                    }
                }
            }
            else
            {
                // the the DTO doesn't came with value (so, delete from server)

                if (userDao.Photo != null)
                {
                    // the DAO has value, then delete file and database reference
                    try
                    {
                        FileManager.DeleteFile(userDao.Photo.RealName);

                        fileRepository.Delete(userDao.Photo);
                        userDao.Photo = null;
                    }
                    catch (Exception)
                    {
                        // cancel operation
                        userRepository.RollbackTransaction();
                        throw new BusinessException("Não foi possível excluir a foto do usuário");
                    }
                }
            }

            // save the user
            userRepository.Update(userDao);
        }        
       
        #region Private methods

        /// <summary>
        /// Encrypts an given value
        /// </summary>
        /// <param name="text">Value to encrypt</param>
        /// <returns>Encrypted text</returns>
        private static string Encrypt(string text)
        {
            ASCIIEncoding encoder;
            byte[] combined;
            byte[] result;
            StringBuilder sb;

            try
            {
                using (SHA1 hash = SHA1.Create())
                {
                    encoder = new ASCIIEncoding();
                    combined = encoder.GetBytes(text);
                    result = hash.ComputeHash(combined);
                    sb = new StringBuilder(result.Length * 2);
                }

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Check if there is another user with the same parameters
        /// </summary>
        /// <param name="id">User's identifier</param>
        /// <param name="username">User's username</param>
        /// <param name="name">User's name</param>
        /// <param name="nickname">User's nickname</param>
        /// <param name="cpf">User's CPF</param>
        private static void ValidateUser(int? id, string username, string name, string nickname, string cpf)
        {
            UserRepository userRepository = new UserRepository();

            // check whether the required data if fullfilled
            if (string.IsNullOrEmpty(username))
            {
                throw new BusinessException("O campo E-mail é obrigatório");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new BusinessException("O campo Nome é obrigatório");
            }
            if (string.IsNullOrEmpty(nickname))
            {
                throw new BusinessException("O campo 'Como você quer ser chamado' é obrigatório");
            }
            if (string.IsNullOrEmpty(cpf))
            {
                throw new BusinessException("O campo CPF é obrigatório");
            }

            // check e-mail format            
            if (!PropertyValidator.ValidateEmail(username))
            {
                throw new BusinessException("Formato inválido de E-mail");
            }

            // check CPF format            
            if (!PropertyValidator.ValidateCpf(cpf))
            {
                throw new BusinessException("CPF inválido");
            }

            // check if the username is being used
            if (userRepository.CheckUsername(id, username))
            {
                throw new BusinessException("Já existe usuário cadastrado com esse E-mail");
            }

            // check if the CPF is being used
            if (userRepository.CheckCpf(id, cpf))
            {
                throw new BusinessException("Já existe usuário cadastrado com esse CPF");
            }
        }

        /// <summary>
        /// Check if password is valid according to password's rules
        /// </summary>
        /// <param name="password">User's password</param>
        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new BusinessException("O campo Senha é obrigatório");
            }

            // check password length
            if (password.Length < 4)
            {
                throw new BusinessException("A senha deve ter no mínimo 4 dígitos");
            }
                     
            // check password digits
            if (!PropertyValidator.ValidateAtLeastLetterNumberText(password))
            {
                throw new BusinessException("A senha deve conter pelo menos uma letra e um número");
            }
        }

        #endregion
    }
}
