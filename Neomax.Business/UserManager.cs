////-----------------------------------------------------------------------
//// <copyright file="UserManager.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Data.Util;
    using Neomax.Data.DataAccess;
    using Neomax.Data.Repository;
    using Neomax.Model.Dto;
    using Neomax.Model.Util;
    using System.Text.RegularExpressions;
    using Neomax.Model.Exception;
    using System.IO;
    using Neomax.Business.Util;
    using AutoMapper;

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

            /*
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
            */

            return true;
        }

        public static string ReleaseEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new BusinessException("Email não informado");

            UserRepository userRepository = new UserRepository();

            if (userRepository.EmailExist(email))
                throw new BusinessException("Email já cadastrado");

            UserAvailableDao userAvailableDao = new UserAvailableDao()
            {
                Email = email,
                Token = Encrypt(email)
            };

            userRepository.ReleaseEmail(userAvailableDao);

            try
            {
                MailManager.SendCreationUserEmail(userAvailableDao.Email, userAvailableDao.Token);
            }
            catch (Exception e)
            {
                userRepository.RollbackTransaction();
                return "Não foi possível enviar o e-mail";
            }

            return "Email liberado com sucesso!";
        }

        /// <summary>
        /// Checks if username and password are valid for login
        /// </summary>
        /// <param name="credentials">User's credentials (username and password)</param>
        /// <returns>UserSession object</returns>
        public static UserSessionDto Login(CredentialsDto credentials)
        {
            FileManager fileManager = new FileManager();
            UserRepository userRepository = new UserRepository();
            string encryptedPassword = Encrypt(credentials.Password);

            credentials.Username = credentials.Username.Replace(".", "").Replace("-", "").Replace("/", "");

            if (credentials == null)
            {
                throw new BusinessException("Sem dados de login");
            }
            else if (string.IsNullOrWhiteSpace(credentials.Username))
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
                Id = userDao.Id.Value,
                Username = userDao.Username,
                Name = userDao.Name,
                Nickname = userDao.Nickname,
                AccessToken = userDao.AccessToken,
                IsAdmin = userDao.Client == null
            };

            if (userDao.PhotoDao != null)
            {
                session.Photo = fileManager.CreateBase64WithFile(userDao.PhotoDao);
            }

            return session;
        }

        /// <summary>
        /// Sign-up a new user with password
        /// </summary>
        /// <param name="userSignUpDto">User's data</param>
        /// <returns>Success Message</returns>
        public static string SignUpUser(UserInputDto userSignUpDto)
        {
            UserRepository userRepository = new UserRepository();
            FileRepository fileRepository = new FileRepository();

            //// Validate user's data and whether there is another user with the same parameters         
            ValidateUser(null, userSignUpDto.Username, userSignUpDto.Name, userSignUpDto.Nickname);

            //// Validate password's rules           
            ValidatePassword(userSignUpDto.Password, userSignUpDto.PasswordConfirmation);

            //// Create new user
            UserDao newUser = new UserDao()
            {
                Username = userSignUpDto.Username,
                Password = Encrypt(userSignUpDto.Password),
                Name = userSignUpDto.Name,
                Client = null,
                Email = userSignUpDto.Email,
                Nickname = userSignUpDto.Nickname,
                Active = true
            };

            if (userSignUpDto.Photo != null)
            {
                try
                {
                    var stringToConvert = FileManager.RemovePathString(userSignUpDto.Photo);

                    FileDao fileDao = new FileDao()
                    {
                        Name = "Foto - " + userSignUpDto.Name + "/" + userSignUpDto.Username,
                        CreateDate = DateTime.Now,
                        Content = Convert.FromBase64String(stringToConvert),
                        MimeType = userSignUpDto.Photo.MimeType
                    };

                    fileRepository.CreateOrUpdate(fileDao);
                    newUser.PhotoDao = fileDao;
                }
                catch (Exception e)
                {
                    throw new BusinessException("Erro ao inserir foto do brasão");
                }
            }

            // save the user
            userRepository.Create(newUser);

            //// Send welcome e-mail 
            try
            {
                MailManager.SendWelcomeEmail(newUser.Email, newUser.Nickname);

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
            //MailManager.SendPasswordCreationEmail(userDao.Username, userDao.Nickname, idUser, changePasswordToken);
        }

        /// <summary>
        /// Checks if the link is valid for password change (the user exists and the token is not expired)
        /// </summary>
        /// <param name="changeInfo">Info for password change (password is left blank)</param>
        /// <returns>User object</returns>
        public static UserDto ValidatePasswordLink(ChangePasswordDto changeInfo)
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
            UserDto userParcialData = new UserDto()
            {
                Id = userDao.Id,
                Username = userDao.Username,
                Name = userDao.Name,
                Nickname = userDao.Nickname,
            };

            return userParcialData;
        }


        /// <summary>
        /// Checks if the link is valid for password change (the user exists and the token is not expired)
        /// </summary>
        /// <param name="changeInfo">Info for password change (password is left blank)</param>
        /// <returns>User object</returns>
        public static UserDto ValidateNewUserLink(string token)
        {
            UserRepository userRepository = new UserRepository();
            PasswordDefinitionRepository passwordDefinitionRespository = new PasswordDefinitionRepository();

            UserAvailableDao userAvailableDao = userRepository.GetUserAvailable(token);

            if (userAvailableDao == null)
                return null;

            if (userRepository.EmailExist(userAvailableDao.Email))
                throw new BusinessException("Usuário já adicionado com esse email");

            // return success and user's data to be displayed (not completed DTO)
            UserDto userParcialData = new UserDto()
            {
                Email = userAvailableDao.Email
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
            ValidatePassword(changeInfo.NewPassword, changeInfo.NewPassword);

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

            //// validate password's rules           
            ValidatePassword(changeInfo.NewPassword, changeInfo.NewPassword);

            //// changes user's password
            UserDao userDao = userRepository.GetById(loggedUser.Id.Value);

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
            FileManager fileManager = new FileManager();
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

            ClientRepository clientRepository = new ClientRepository();

            UserSessionDto session = new UserSessionDto()
            {
                Id = userDao.Id.Value,
                Username = userDao.Username,
                Name = userDao.Name,
                Nickname = userDao.Nickname,
                AccessToken = userDao.AccessToken,
                Email = userDao.Email,
                IsAdmin = userDao.Client == null,
                Client = Mapper.Map<ClientDto>(clientRepository.GetByIdUser(userDao.Id.Value)),
                IdClient = clientRepository.GetByIdUser(userDao.Id.Value).Id
            };

            //if (session.Client == null)
            //{
            //    //var clientDao = clientRepository.GetByIdUser(session.Id);

            //    session.Client = new ClientDto()
            //    {
            //        Id = userDao.Client.Id
            //    };

            //    session.IdClient = userDao.Client.Id;
            //}

            if (userDao.PhotoDao != null)
            {
                session.Photo = fileManager.CreateBase64WithFile(userDao.PhotoDao);
            }

            return session;
        }

        /// <summary>
        /// Searches users refined by filter and pagination (according to logged user's permission)
        /// </summary>
        /// <param name="filter">Filter parameters to refine the search</param>
        /// <returns> PaginationResponse with List of Users </returns>
        public static PaginationResponseDto<UserDto> Search(UserFilterDto filter)
        {
            UserRepository userRepository = new UserRepository();

            //// sets the quantity of "results per page" from webconfig (didn't come on the filter)
            filter.ResultsPerPage = ApplicationConfiguration.ResultsPerPage;

            //// gets the users based on filter (including pagination)
            var paginationResponseFromRepository = userRepository.GetByFilter(filter);

            //// returns the search using the PaginationResponse model
            PaginationResponseDto<UserDto> paginationResponseDto = new PaginationResponseDto<UserDto>()
            {
                TotalResults = paginationResponseFromRepository.TotalResults,
                ResultsPerPage = filter.ResultsPerPage,
                CurrentPage = filter.PageNumber,
                Response = Mapper.Map<List<UserDto>>(paginationResponseFromRepository.Response)
            };

            return paginationResponseDto;
        }

        /// <summary>
        /// Gets an user by its identifier
        /// </summary>
        /// <param name="loggedUser">Logged User</param>
        /// <param name="id">User identifier</param>
        /// <returns>User object</returns>
        public static UserDto GetUserById(UserDao loggedUser, int id)
        {
            FileManager fileManager = new FileManager();
            UserRepository userRepository = new UserRepository();
            ClientRepository clientRepository = new ClientRepository();

            UserDao userDao = userRepository.GetById(id);

            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("O usuário não existe");
            }

            //// converts from DAO to DTO
            UserDto userDto = new UserDto()
            {
                Id = userDao.Id,
                Username = userDao.Username,
                Name = userDao.Name,
                Nickname = userDao.Nickname,
                Email = userDao.Email
            };

            if (userDao.Client != null)
            {
                userDto.ClientDto = new ClientDto()
                {
                    Id = userDao.Client.Id,
                    AnnualBilling = userDao.Client.AnnualBilling,
                    CNPJPayingSource = !string.IsNullOrWhiteSpace(userDao.Client.CNPJPayingSource) ? userDao.Client.CNPJPayingSource : "Não Informado",
                    Gender = userDao.Client.Gender,
                    NatureBackground = userDao.Client.NatureBackground,
                    TypeNoteEmited = userDao.Client.TypeNoteEmited,
                    User = null,
                    ListBanks = Mapper.Map<List<BankDto>>(userDao.Client.ListBanks),
                    ListContactDay = Mapper.Map<List<ContactDayDto>>(clientRepository.GetContactDayByIdClient(userDao.Client.Id.Value)),
                    ListContactTime = Mapper.Map<List<ContactTimeDto>>(clientRepository.GetContactTimeByIdClient(userDao.Client.Id.Value)),
                    ListTelephones = Mapper.Map<List<TelephoneDto>>(userDao.Client.ListTelephones),
                    AnnualBillingName = userDao.Client.AnnualBilling.HasValue ? Domain.TextValueFrom(userDao.Client.AnnualBilling) : "Não Informado",
                    GenderName = userDao.Client.Gender.HasValue ? Domain.TextValueFrom(userDao.Client.Gender) : "Não Informado",
                    NatureBackgroundName = userDao.Client.NatureBackground.HasValue ? Domain.TextValueFrom(userDao.Client.NatureBackground) : "Não Informado",
                    TypeNoteEmitedName = userDao.Client.TypeNoteEmited.HasValue ? Domain.TextValueFrom(userDao.Client.TypeNoteEmited) : "Não Informado",
                    ListDocumentsBase64 = new List<HttpFileBase64Dto>()
                };

                if (userDao.Client.ListDocuments != null)
                {
                    foreach (var doc in userDao.Client.ListDocuments)
                    {
                        var docInBase64 = fileManager.CreateBase64WithFile(doc);

                        userDto.ClientDto.ListDocumentsBase64.Add(docInBase64);
                    }
                }
            }

            if (userDao.PhotoDao != null)
            {
                userDto.Photo = fileManager.CreateBase64WithFile(userDao.PhotoDao);
            }

            return userDto;
        }

        /// <summary>
        /// Saves an user (create or update)
        /// </summary>
        /// <param name="userDto">User data</param>
        /// <returns>Success Message</returns>
        public static string SaveUser(UserInputDto userDto)
        {
            UserRepository userRepository = new UserRepository();
            FileRepository fileRepository = new FileRepository();

            // validate user's data and whether there is another user with the same parameters            
            ValidateUser(userDto.Id, userDto.Username, userDto.Name, userDto.Nickname);

            UserDao userDao = new UserDao();

            if (userDao == null || !userDao.Active)
            {
                throw new BusinessException("O usuário não existe");
            }

            // update data (including profile and CPF)
            userDao.Username = userDto.Username;
            userDao.Name = userDto.Name;
            userDao.Nickname = userDto.Nickname;
            userDao.Email = userDto.Email;

            UserDao newUser = new UserDao()
            {
                Username = userDto.Username,
                Name = userDto.Name,
                Nickname = userDto.Nickname,
                Email = userDto.Username,
                Active = true
            };

            // generate a temporary password (only to not leave the field empty at database)
            string temporaryPassword = string.Format(@"{0}", Guid.NewGuid()).Substring(0, 8);
            newUser.Password = Encrypt(temporaryPassword);

            // create/update/delete photo
            if (userDto.Photo != null)
            {
                // the DAO has value, then update files and database reference
                try
                {
                    var stringToConvert = FileManager.RemovePathString(userDto.Photo);

                    FileDao fileDao = new FileDao()
                    {
                        Name = "Foto - " + userDto.Name + "/" + userDto.Username,
                        CreateDate = DateTime.Now,
                        Content = Convert.FromBase64String(stringToConvert),
                        MimeType = userDto.Photo.MimeType
                    };

                    fileRepository.CreateOrUpdate(fileDao);
                    userDao.PhotoDao = fileDao;
                }
                catch (Exception e)
                {
                    throw new BusinessException("Erro ao inserir foto do brasão");
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

            return "Usuário Criado com Sucesso";
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
        public static void UpdateUser(UserInputDto userDto)
        {
            UserRepository userRepository = new UserRepository();
            FileRepository fileRepository = new FileRepository();

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
            userDao.Email = userDto.Email;

            // create/update/delete photo
            if (userDto.Photo != null)
            {
                // the DAO has value, then update files and database reference
                try
                {
                    var stringToConvert = FileManager.RemovePathString(userDto.Photo);

                    FileDao fileDao = new FileDao()
                    {
                        Name = "Foto - " + userDto.Name + "/" + userDto.Username,
                        CreateDate = DateTime.Now,
                        Content = Convert.FromBase64String(stringToConvert),
                        MimeType = userDto.Photo.MimeType
                    };

                    fileRepository.CreateOrUpdate(fileDao);
                    userDao.PhotoDao = fileDao;
                }
                catch (Exception e)
                {
                    throw new BusinessException("Erro ao inserir foto do usuário");
                }
            }

            userRepository.CreateOrUpdate(userDao);
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
        private static void ValidateUser(int? id, string username, string name, string nickname)
        {
            UserRepository userRepository = new UserRepository();

            // check whether the required data if fullfilled
            if (string.IsNullOrEmpty(username))
            {
                throw new BusinessException("O campo CPF é obrigatório");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new BusinessException("O campo Nome é obrigatório");
            }

            // check if the username is being used
            if (userRepository.CheckUsername(id, username))
            {
                throw new BusinessException("Já existe usuário cadastrado com esse E-mail");
            }

            //if(userRepository.ValidateToken())
        }

        /// <summary>
        /// Check if password is valid according to password's rules
        /// </summary>
        /// <param name="password">User's password</param>
        private static void ValidatePassword(string password, string passwordConfirmation)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new BusinessException("O campo Senha é obrigatório");
            }

            // check password length
            if (password.Length < 5)
            {
                throw new BusinessException("A senha deve ter no mínimo 5 dígitos");
            }

            if (!password.Equals(passwordConfirmation))
            {
                throw new BusinessException("Senha e confirmação devem ser iguais");
            }
        }

        #endregion
    }
}
