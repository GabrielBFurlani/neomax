////-----------------------------------------------------------------------
//// <copyright file="ClientManager.cs" company="ZetaCorp">
////  (R) Registrado 2020 Zetacorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Util;
    using Neomax.Data.DataAccess;
    using Neomax.Data.Repository;
    using Neomax.Model.Dto;
    using Neomax.Model.Util;
    using Neomax.Model.Exception;
    using System.IO;
    using Neomax.Business.Util;
    using DocumentFormat.OpenXml.Bibliography;
    using AutoMapper;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Manages business rules related to profile
    /// </summary>
    public class ClientManager
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ClientDto CreateOrUpdate(int? id, ClientInputDto clientInputDto)
        {
            ClientRepository clientRepository = new ClientRepository();

            FileRepository fileRepository = new FileRepository();

            UserRepository userRepository = new UserRepository();

            FileManager fileManager = new FileManager();

            if (clientInputDto == null)
            {
                throw new BusinessException("Cliente não informado");
            }

            if (string.IsNullOrWhiteSpace(clientInputDto.Name))
            {
                throw new BusinessException("O nome do cliente deve ser informado");
            }

            if (string.IsNullOrWhiteSpace(clientInputDto.Username))
            {
                throw new BusinessException("O CPF/CNPJ deve ser informado");
            }

            if (string.IsNullOrWhiteSpace(clientInputDto.Password))
            {
                throw new BusinessException("A senha deve ser informada");
            }

            if (string.IsNullOrWhiteSpace(clientInputDto.PasswordConfirmation))
            {
                throw new BusinessException("A confirmação da senha deve ser informada");
            }

            if (!clientInputDto.Password.Equals(clientInputDto.PasswordConfirmation))
            {
                throw new BusinessException("Senhas não combinam!");
            }

            if (userRepository.GetByUsername(clientInputDto.Username) != null)
            {
                throw new BusinessException("CPF/CNPJ já cadastrado");
            }

            UserDao userDao = new UserDao()
            {
                Active = true,
                Email = clientInputDto.Email,
                Name = clientInputDto.Name,
                Nickname = clientInputDto.NickName,
                Password = Encrypt(clientInputDto.Password),
                Username = clientInputDto.Username
            };

            if (clientInputDto.Photo != null)
            {
                try
                {
                    var stringToConvert = FileManager.RemovePathString(clientInputDto.Photo);

                    FileDao fileDao = new FileDao()
                    {
                        Name = "Brasao - " + clientInputDto.Name + "/" + clientInputDto.Username,
                        CreateDate = DateTime.Now,
                        Content = Convert.FromBase64String(stringToConvert),
                        MimeType = clientInputDto.Photo.MimeType
                    };

                    fileRepository.CreateOrUpdate(fileDao);
                    userDao.Photo = fileDao;
                }
                catch (Exception e)
                {
                    throw new BusinessException("Erro ao inserir foto do brasão");
                }
            }

            userRepository.CreateOrUpdate(userDao);

            ClientDao clientDao = new ClientDao()
            {
                AnnualBilling = clientInputDto.AnnualBilling,
                CNPJPayingSource = clientInputDto.CNPJPayingSource,
                Gender = clientInputDto.Gender,
                NatureBackground = clientInputDto.NatureBackground,
                TypeNoteEmited = clientInputDto.TypeNoteEmited,
                ListBanks = new List<BankDao>(),
                ListContactDay = new List<ContactDayDao>(),
                ListDocuments = new List<FileDao>(),
                ListTelephones = new List<TelephoneDao>(),
            };

            if (clientInputDto.Banks?.Count > 0)
            {
                BankDao newBank = null;

                foreach (var bank in clientInputDto.Banks)
                {
                    newBank = new BankDao()
                    {
                        Account = bank.Account,
                        Agency = bank.Agency,
                        Bank = bank.Bank
                    };

                    clientDao.ListBanks.Add(newBank);
                }
            }

            if (clientInputDto.Telephones?.Count > 0)
            {
                TelephoneDao newTelephone = null;

                foreach (var telephone in clientInputDto.Telephones)
                {
                    newTelephone = new TelephoneDao()
                    {
                        Number = int.Parse(telephone.Number),
                        ContactName = telephone.ContactName,
                        TelephoneType = telephone.TelephoneType
                    };

                    clientDao.ListTelephones.Add(newTelephone);
                }
            }

            if (clientInputDto.Documents?.Count > 0)
            {

            }

            clientRepository.CreateOrUpdate(clientDao);

            if (clientInputDto.ContactDays?.Count > 0)
            {
                ContactDayDao newContactDay = null;

                foreach (var contactDay in clientInputDto.ContactDays)
                {
                    newContactDay = new ContactDayDao()
                    {
                        Client = clientDao,
                        ContactDay = contactDay
                    };

                    clientDao.ListContactDay.Add(newContactDay);
                }
            }

            if (clientInputDto.ContactTimes?.Count > 0)
            {
                ContactTimeDao newContactTime = null;

                foreach (var contactTime in clientInputDto.ContactTimes)
                {
                    newContactTime = new ContactTimeDao()
                    {
                        Client = clientDao,
                        ContactTime = contactTime
                    };

                    clientDao.ListContactTime.Add(newContactTime);
                }
            }

            clientRepository.CreateOrUpdate(clientDao);

            //var clientDto = Mapper.Map<ClientDto>(clientDao);

            //User.Photo = fileManager.CreateBase64WithFile(userDao.Photo);

            return null;
        }

        //public static UserDto GetByIdUser(int idUser)
        //{
        //    ClientRepository clientRepository = new ClientRepository();

        //    UserRepository userRepository = new UserRepository();

        //    FileManager fileManager = new FileManager();

        //    if (idUser == 0)
        //    {
        //        throw new BusinessException("Identificador do cliente não informado");
        //    }

        //    UserDao user = userRepository.GetById(idUser);

        //    if (user == null)
        //        return null;

        //    var userDto = Mapper.Map<UserDto>(user);

        //    userDto.Photo = fileManager.CreateBase64WithFile(user.Photo);

        //    return userDto;
        //}

        public static PaginationResponseDto<UserDto> Search(ClientFilterDto filter)
        {
            filter.Argument = string.IsNullOrWhiteSpace(filter.Argument) ? "" : filter.Argument.Replace(".", "").Replace("/", "").Replace("-", "");

            ClientRepository clientRepository = new ClientRepository();

            var userDaos = clientRepository.GetByFilter(filter);

            PaginationResponseDto<UserDto> paginationResponseDto = new PaginationResponseDto<UserDto>()
            {
                CurrentPage = filter.PageNumber,
                Response = Mapper.Map<List<UserDto>>(userDaos.Response),
                ResultsPerPage = userDaos.ResultsPerPage,
                TotalResults = userDaos.TotalResults
            };

            return paginationResponseDto;
        }

        /// <inheritdoc cref="IuserManager.Disable(int)"/>
        public static void Disable(int codigo)
        {
            if (codigo == 0)
            {
                throw new BusinessException("Identificador do cliente não informado");
            }

            UserRepository userRepository = new UserRepository();

            UserDao user = userRepository.GetById(codigo);

            if (user == null)
            {
                throw new BusinessException("Cliente não encontrado");
            }

            if (!user.Active)
            {
                throw new BusinessException("Cliente já inativo");
            }

            user.Active = false;

            userRepository.CreateOrUpdate(user);
        }

        /// <inheritdoc cref="IuserManager.Reactivate(int)"/>
        public static void Reactivate(int codigo)
        {
            if (codigo == 0)
            {
                throw new BusinessException("Identificador do cliente não informado");
            }

            UserRepository userRepository = new UserRepository();

            UserDao user = userRepository.GetById(codigo);

            if (user == null)
            {
                throw new BusinessException("Cliente não encontrado");
            }

            if (user.Active)
            {
                throw new BusinessException("Cliente já ativo");
            }

            user.Active = true;

            userRepository.CreateOrUpdate(user);
        }


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

    }
}
