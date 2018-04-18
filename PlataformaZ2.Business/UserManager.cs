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

        #region Baisc operations (user creation, login and password operations)

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
        public static OperationResult Login(CredentialsDto credentials)
        {
            UserRepository userRepository = new UserRepository();
            string encryptedPassword = Encrypt(credentials.Password);

            if (credentials == null)
            {
                return new OperationResult(false, "Sem dados de login");
            }
            else if(string.IsNullOrWhiteSpace(credentials.Username))
            {
                return new OperationResult(false, "Sem dados de usuário");
            }
            else if (string.IsNullOrWhiteSpace(credentials.Password))
            {
                return new OperationResult(false, "Sem dados de senha");
            }

            var userDao = userRepository.MatchCredentials(credentials.Username, encryptedPassword);

            if (userDao == null)
            {
                return new OperationResult(false, "Usuário ou senha inválidos");
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

            return new OperationResult(true, string.Empty, session);
        }

        /// <summary>
        /// Sign-up a new user with password
        /// </summary>
        /// <param name="userDto">User's data</param>
        /// <returns>Operation result</returns>
        public static OperationResult SignUpUser(UserSignUpDto userDto)
        {
            UserRepository userRepository = new UserRepository();
            FileRepository fileRepository = new FileRepository();

            //// Validate user's data and whether there is another user with the same parameters         
            OperationResult operation = ValidateUser(null, userDto.Username, userDto.Name, userDto.Nickname, userDto.Cpf);

            if (!operation.Success)
            {
                // return the same operation result of validation
                return operation;
            }

            //// Create new user
            UserDao newUser = new UserDao()
            {
                Username = userDto.Username,
                Password = Encrypt(userDto.Password),
                Name = userDto.Name,
                Nickname = userDto.Nickname,
                Cpf = userDto.Cpf,                
                Active = true
            };            

            if (userDto.Photo != null)
            {
                var result = FileManager.SaveBase64Image(userDto.Photo);

                if (result.Success)
                {
                    var uniqueName = result.Data.ToString();

                    FileDao fileDao = new FileDao()
                    {
                        Name = newUser.Nickname + Path.GetExtension(uniqueName),
                        RealName = uniqueName
                    };

                    fileRepository.Create(fileDao);
                    newUser.Photo = fileRepository.GetById(fileDao.Id.Value);
                }
                else
                {
                    // cancel operation and return the result
                    userRepository.RollbackTransaction();
                    return result;
                }
            }

            // set the profile as "basic user"
            newUser.Profile = new ProfileRepository().GetById(1);

            // save the user
            userRepository.Create(newUser);

            //// Send welcome e-mail 
            operation = MailManager.SendWelcomeEmail(newUser.Username, newUser.Nickname);

            if (operation.Success)
            {
                return new OperationResult(true);
            }
            else
            {
                return new OperationResult(true, "Usuário cadastrado com sucesso, mas não foi possível enviar o e-mail");
            }            
        }

        /// <summary>
        /// Generates token and sends an e-mail to redefine the password
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>Operation result</returns>
        public static OperationResult ForgotPassword(string email)
        {
            UserRepository userRepository = new UserRepository();
            PasswordDefinitionRepository passwordDefinitionRepository = new PasswordDefinitionRepository();

            ////Check user
            var userDao = userRepository.GetByUsername(email);

            if (userDao == null)
            {
                return new OperationResult(false, "Não existe usuário cadastrado com esse e-mail");                
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
            var operation = MailManager.SendForgotPasswordEmail(userDao.Username, userDao.Nickname, userDao.Id.Value, changePasswordToken);

            if (operation.Success)
            {
                int middleIndex = userDao.Username.IndexOf("@");
                string maskedEmail = userDao.Username.Substring(0, 3) + "****" + userDao.Username.Substring(middleIndex, 4) + "***";

                return new OperationResult(true, "E-mail enviado para " + maskedEmail);
            }
            else
            {
                return new OperationResult(false, "Não foi possível enviar o e-mail");
            }
        }

        /// <summary>
        /// Generates token and sends an e-mail of first password creation
        /// </summary>
        /// <param name="idUser">User identifier</param>
        /// <returns>Operation result</returns>
        public static OperationResult PasswordFirstCreation(int idUser)
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
            var operation = MailManager.SendPasswordCreationEmail(userDao.Username, userDao.Nickname, idUser, changePasswordToken);
            if (operation.Success)
            {
                return new OperationResult(true);
            }
            else
            {
                return new OperationResult(false, "Não foi possível enviar o e-mail");
            }
        }

        /// <summary>
        /// Checks if the link is valid for password change (the user exists and the token is not expired)
        /// </summary>
        /// <param name="changeInfo">Info for password change (password is left blank)</param>
        /// <returns>Operation result</returns>
        public static OperationResult ValidatePasswordLink(ChangePasswordDto changeInfo)
        {
            UserRepository userRepository = new UserRepository();
            PasswordDefinitionRepository passwordDefinitionRespository = new PasswordDefinitionRepository();

            UserDao userDao = userRepository.GetById(changeInfo.IdUser);

            if (userDao == null || !userDao.Active)
            {
                return new OperationResult(false, "O usuário não existe");
            }

            ////Check if the token is valid for password change            
            PasswordDefinitionDao passwordDefinitionDao = passwordDefinitionRespository.SearchForValidToken(userDao.Id.Value);

            if (passwordDefinitionDao == null || passwordDefinitionDao.Token != changeInfo.ChangePasswordToken)
            {
                return new OperationResult(false, "Link expirado para troca de senha. Solicite novamente.");                
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

            return new OperationResult(true, string.Empty, userParcialData);
        }

        /// <summary>
        /// Defines/Redefines the password for a non-logged user (using password's token)
        /// </summary>
        /// <param name="changeInfo">Info for password change</param>
        /// <returns>Operation result</returns>
        public static OperationResult ChangePasswordUsingToken(ChangePasswordDto changeInfo)
        {
            UserRepository userRepository = new UserRepository();
            PasswordDefinitionRepository passwordDefinitionRespository = new PasswordDefinitionRepository();

            if (changeInfo == null)
            {
                return new OperationResult(false, "Sem dados de senha");
            }

            //// validate link
            OperationResult linkOperation = ValidatePasswordLink(changeInfo);

            if (!linkOperation.Success)
            {
                ////Return the same operation result of validation 
                return linkOperation;
            }

            //// validate password's rules           
            OperationResult passwordOperation = ValidatePassword(changeInfo.NewPassword);

            if (!passwordOperation.Success)
            {
                // return the same operation result of validation                
                return passwordOperation;
            }

            ////The link and password are valid: Encrypt the password, update the user and expire the token           
            var UserDao = userRepository.GetById(changeInfo.IdUser);
            UserDao.Password = Encrypt(changeInfo.NewPassword);
            userRepository.Update(UserDao);

            var passwordDefinitionDao = passwordDefinitionRespository.SearchForValidToken(UserDao.Id.Value);
            passwordDefinitionDao.ExpirationDate = DateTime.Now;
            passwordDefinitionRespository.Update(passwordDefinitionDao);

            return new OperationResult(true);
        }

        #endregion

        /// <summary>
        /// Redefines the password of a logged user (does not use password's token)
        /// </summary>
        /// <param name="loggedUser">Logged User</param>
        /// <param name="changeInfo">Info for password change (token is left blank)</param>
        /// <returns>Operation result</returns>
        public static OperationResult ChangePasswordForLoggedUser(UserDao loggedUser, ChangePasswordDto changeInfo)
        {
            UserRepository userRepository = new UserRepository();

            if (changeInfo == null)
            {
                return new OperationResult(false, "Sem dados de senha");
            }

            //// checks logged user permission
            if (loggedUser.Id != changeInfo.IdUser)
            {
                log.Warn("Usuário Id=" + loggedUser.Id.ToString() + " tentou entrar como " + changeInfo.IdUser.ToString());
                throw new PermissionException();
            }

            //// validate password's rules           
            OperationResult operation = ValidatePassword(changeInfo.NewPassword);

            if (!operation.Success)
            {
                // return the same operation result of validation                
                return operation;
            }

            //// changes user's password
            UserDao userDao = userRepository.GetById(changeInfo.IdUser);

            if (userDao == null || !userDao.Active)
            {
                return new OperationResult(false, "O usuário não existe");                
            }
            
            userDao.Password = Encrypt(changeInfo.NewPassword);  // Encrypt the password
            userRepository.Update(userDao);
            return new OperationResult(true);
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
                return null;
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
                return null;
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
        /// <returns>Operation result</returns>
        public static OperationResult SaveUser(UserFullDto userDto)
        {
            UserRepository userRepository = new UserRepository();
            FileRepository fileRepository = new FileRepository();

            // validate user's data and whether there is another user with the same parameters            
            OperationResult operation = ValidateUser(userDto.Id, userDto.Username, userDto.Name, userDto.Nickname, userDto.Cpf);

            if (!operation.Success)
            {
                // return the same operation result of validation                
                return operation;
            }

            //// Checks if it is "new user creation" or "user update" operation
            if (userDto.Id.HasValue)
            {
                //// "user update" operation

                UserDao userDao = userRepository.GetById(userDto.Id.Value);

                if (userDao == null || !userDao.Active)
                {
                    return new OperationResult(false, "O usuário não existe");
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

                        FileManager.DeleteFile(userDao.Photo.RealName);
                        var result = FileManager.SaveBase64Image(userDto.Photo);

                        if (result.Success)
                        {
                            var uniqueName = result.Data.ToString();

                            userDao.Photo.Name = userDao.Nickname + Path.GetExtension(uniqueName);
                            userDao.Photo.RealName = uniqueName;
                        }
                        else
                        {
                            // cancel operation and return the result
                            userRepository.RollbackTransaction();
                            return result;
                        }
                    }
                    else
                    {
                        // the DAO doesn't have value, then create the file and link it to the database
                        var result = FileManager.SaveBase64Image(userDto.Photo);

                        if (result.Success)
                        {
                            var uniqueName = result.Data.ToString();

                            FileDao fileDao = new FileDao()
                            {
                                Name = userDao.Nickname + Path.GetExtension(uniqueName),
                                RealName = uniqueName
                            };

                            fileRepository.Create(fileDao);
                            userDao.Photo = fileRepository.GetById(fileDao.Id.Value);
                        }
                        else
                        {
                            // cancel operation and return the result
                            userRepository.RollbackTransaction();
                            return result;
                        }
                    }
                }
                else
                {
                    // the the DTO doesn't came with value (so, delete from server)

                    if (userDao.Photo != null)
                    {
                        // the DAO has value, then delete file and database reference

                        FileManager.DeleteFile(userDao.Photo.RealName);

                        fileRepository.Delete(userDao.Photo);
                        userDao.Photo = null;
                    }
                }

                // save the user
                userRepository.Update(userDao);

                return new OperationResult(true);
            }
            else
            {
                //// "new user creation" operation

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
                    var result = FileManager.SaveBase64Image(userDto.Photo);

                    if (result.Success)
                    {
                        var uniqueName = result.Data.ToString();

                        FileDao fileDao = new FileDao()
                        {
                            Name = newUser.Nickname + Path.GetExtension(uniqueName),
                            RealName = uniqueName
                        };

                        fileRepository.Create(fileDao);
                        newUser.Photo = fileRepository.GetById(fileDao.Id.Value);
                    }
                    else
                    {
                        // cancel operation and return the result
                        userRepository.RollbackTransaction();
                        return result;
                    }
                }
                
                // save the user
                userRepository.Create(newUser);

                // Send "welcome" e-mail (wihtout checking result operation) 
                MailManager.SendWelcomeEmail(newUser.Username, newUser.Nickname);

                // Send "First Password Creation" e-mail
                operation = PasswordFirstCreation(newUser.Id.Value);

                if (operation.Success)
                {
                    return new OperationResult(true);
                }
                else
                {
                    return new OperationResult(true, "Usuário cadastrado com sucesso, mas não foi possível enviar o e-mail de criação de senha");
                }
            }
        }

        /// <summary>
        /// Deletes an user (set as inactive)
        /// </summary>
        /// <param name="idUser">User identifier</param>
        /// <returns>Operation result</returns>
        public static OperationResult DeleteUser(int idUser)
        {
            UserRepository userRepository = new UserRepository();

            UserDao userDao = userRepository.GetById(idUser);
            
            if (userDao == null)
            {
                return new OperationResult(false, "O usuário não existe");
            }

            ////set user as inactive (do not delete from database)
            userDao.Active = false;

            ////save the user with the inactive status
            userRepository.Update(userDao);

            return new OperationResult(true);
        }

        /// <summary>
        /// Updates user's data (done by the user)
        /// </summary>
        /// <param name="loggedUser">Logged User</param>
        /// <param name="userDto">User data</param>
        /// <returns>Operation result</returns>
        public static OperationResult UpdateUserByUser(UserDao loggedUser, UserFullDto userDto)
        {
            UserRepository userRepository = new UserRepository();
            FileRepository fileRepository = new FileRepository();

            if (userDto == null)
            {
                return new OperationResult(false, "Sem dados do usuário");
            }
            else if (!userDto.Id.HasValue)
            {
                return new OperationResult(false, "Nenhum usuário selecionado");
            }

            //// checks logged user permission
            if (loggedUser.Id != userDto.Id)
            {
                log.Warn("Usuário Id=" + loggedUser.Id.ToString() + " tentou atualizar dados do " + userDto.Id.ToString());
                throw new PermissionException();
            }

            // validate user's data and whether there is another user with the same parameters            
            OperationResult operation = ValidateUser(userDto.Id, userDto.Username, userDto.Name, userDto.Nickname, userDto.Cpf);

            if (!operation.Success)
            {
                // return the same operation result of validation                
                return operation;
            }

            //// gets user's data
            UserDao userDao = userRepository.GetById(userDto.Id.Value);

            if (userDao == null || !userDao.Active)
            {
                return new OperationResult(false, "O usuário não existe");
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

                    FileManager.DeleteFile(userDao.Photo.RealName);
                    var result = FileManager.SaveBase64Image(userDto.Photo);

                    if (result.Success)
                    {
                        var uniqueName = result.Data.ToString();

                        userDao.Photo.Name = userDao.Nickname + Path.GetExtension(uniqueName);
                        userDao.Photo.RealName = uniqueName;                        
                    }
                    else
                    {
                        // cancel operation and return the result
                        userRepository.RollbackTransaction();
                        return result;
                    }
                }
                else
                {
                    // the DAO doesn't have value, then create the file and link it to the database
                    var result = FileManager.SaveBase64Image(userDto.Photo);

                    if (result.Success)
                    {
                        var uniqueName = result.Data.ToString();

                        FileDao fileDao = new FileDao()
                        {
                            Name = userDao.Nickname + Path.GetExtension(uniqueName),
                            RealName = uniqueName
                        };

                        fileRepository.Create(fileDao);
                        userDao.Photo = fileRepository.GetById(fileDao.Id.Value);
                    }
                    else
                    {
                        // cancel operation and return the result
                        userRepository.RollbackTransaction();
                        return result;
                    }                    
                }
            }
            else
            {
                // the the DTO doesn't came with value (so, delete from server)

                if (userDao.Photo != null)
                {
                    // the DAO has value, then delete file and database reference

                    FileManager.DeleteFile(userDao.Photo.RealName);

                    fileRepository.Delete(userDao.Photo);
                    userDao.Photo = null;
                }
            }

            // save the user
            userRepository.Update(userDao);

            return new OperationResult(true);
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
        /// <returns>Operation result</returns>
        private static OperationResult ValidateUser(int? id, string username, string name, string nickname, string cpf)
        {
            UserRepository userRepository = new UserRepository();

            // check whether the required data if fullfilled
            if (string.IsNullOrEmpty(username))
            {
                return new OperationResult(false, "O campo E-mail é obrigatório");
            }
            if (string.IsNullOrEmpty(name))
            {
                return new OperationResult(false, "O campo Nome é obrigatório");
            }
            if (string.IsNullOrEmpty(nickname))
            {
                return new OperationResult(false, "O campo 'Como você quer ser chamado' é obrigatório");
            }
            if (string.IsNullOrEmpty(cpf))
            {
                return new OperationResult(false, "O campo CPF é obrigatório");
            }

            // check e-mail format            
            if (!PropertyValidator.ValidateEmail(username))
            {
                return new OperationResult(false, "Formato inválido de E-mail");
            }

            // check CPF format            
            if (!PropertyValidator.ValidateCpf(cpf))
            {
                return new OperationResult(false, "CPF inválido");
            }

            // check if the username is being used
            if (userRepository.CheckUsername(id, username))
            {
                return new OperationResult(false, "Já existe usuário cadastrado com esse E-mail");
            }
            
            //everything is OK
            return new OperationResult(true);
        }

        /// <summary>
        /// Check if password is valid according to password's rules
        /// </summary>
        /// <param name="password">User's password</param>
        /// <returns>Operation result</returns>
        private static OperationResult ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return new OperationResult(false, "O campo Senha é obrigatório");
            }

            // check password length
            if (password.Length < 4)
            {
                return new OperationResult(false, "A senha deve ter no mínimo 4 dígitos");
            }
                     
            // check password digits
            if (!PropertyValidator.ValidateAtLeastLetterNumberText(password))
            {
                return new OperationResult(false, "A senha deve conter pelo menos uma letra e um número");
            }

            //everything is OK
            return new OperationResult(true);
        }

        #endregion
    }
}
