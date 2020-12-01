////-----------------------------------------------------------------------
//// <copyright file="ClientManager.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
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
    using DocumentFormat.OpenXml.Office2010.PowerPoint;

    /// <summary>
    /// Manages business rules related to profile
    /// </summary>
    public class ClientManager
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string CreateOrUpdate(int? id, ClientInputDto clientInputDto)
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

            if (!IsCnpj(clientInputDto.Username) && !IsCpf(clientInputDto.Username))
            {
                throw new BusinessException("CPF/CNPJ inválido");
            }

            clientInputDto.Username = clientInputDto.Username.Replace(".", "").Replace("-", "").Replace("/", "");

            /*    if (!IsCnpj(clientInputDto.CnpjPaying))
                {
                    throw new BusinessException("CNPJ da fonte pagadora inválido");
                }*/

            //clientInputDto.CnpjPaying = clientInputDto.CnpjPaying.Replace(".", "").Replace("-", "").Replace("/", "");

            if (!id.HasValue && string.IsNullOrWhiteSpace(clientInputDto.Password))
            {
                throw new BusinessException("A senha deve ser informada");
            }

            if (!id.HasValue && string.IsNullOrWhiteSpace(clientInputDto.PasswordConfirmation))
            {
                throw new BusinessException("A confirmação da senha deve ser informada");
            }

            if (!id.HasValue && !clientInputDto.Password.Equals(clientInputDto.PasswordConfirmation))
            {
                throw new BusinessException("Senhas não combinam!");
            }

            UserDao userDao;

            if (!id.HasValue)
                userDao = userRepository.GetByUsername(clientInputDto.Username);
            else
                userDao = userRepository.GetById(id.Value);

            if (userDao == null && id.HasValue)
            {
                throw new BusinessException("Usuário não encontrado");
            }

            /*if (userDao != null && id != userDao.Id)
            {
                throw new BusinessException("CPF/CNPJ já cadastrado");
            }*/

            if (userDao != null)
            {
                userDao.Active = true;
                userDao.Email = clientInputDto.Email;
                userDao.Name = clientInputDto.Name;
                userDao.Nickname = clientInputDto.NickName;
                //userDao.Username = clientInputDto.Username
            }
            else
            {
                userDao = new UserDao()
                {
                    Active = true,
                    Email = clientInputDto.Email,
                    Name = clientInputDto.Name,
                    Nickname = clientInputDto.NickName,
                    Password = Encrypt(clientInputDto.Password),
                    Username = clientInputDto.Username
                };
            }


            //if (clientInputDto.Photo != null)
            //{
            //    try
            //    {
            //        var stringToConvert = FileManager.RemovePathString(clientInputDto.Photo);

            //        FileDao fileDao = new FileDao()
            //        {
            //            Name = "Brasao - " + clientInputDto.Name + "/" + clientInputDto.Username,
            //            CreateDate = DateTime.Now,
            //            Content = Convert.FromBase64String(stringToConvert),
            //            MimeType = clientInputDto.Photo.MimeType
            //        };

            //        fileRepository.CreateOrUpdate(fileDao);
            //        userDao.Photo = fileDao;
            //    }
            //    catch (Exception e)
            //    {
            //        throw new BusinessException("Erro ao inserir foto do brasão");
            //    }
            //}

            userRepository.CreateOrUpdate(userDao);

            ClientDao clientDao = id.HasValue ? userDao.Client : new ClientDao();

            if (!id.HasValue)
            {
                clientDao = new ClientDao()
                {
                    User = userDao,
                    AnnualBilling = clientInputDto.AnnualBilling,
                    CNPJPayingSource = clientInputDto.CnpjPaying,
                    Gender = clientInputDto.Gender,
                    NatureBackground = clientInputDto.CompanyNatureType,
                    TypeNoteEmited = clientInputDto.TypeNoteEmited,
                    ListBanks = new List<BankDao>(),
                    ListContactDay = new List<ContactDayDao>(),
                    ListContactTime = new List<ContactTimeDao>(),
                    ListDocuments = new List<FileDao>(),
                    ListTelephones = new List<TelephoneDao>(),
                };
            }
            else
            {
                clientDao.User = userDao;
                clientDao.AnnualBilling = clientInputDto.AnnualBilling;
                clientDao.CNPJPayingSource = clientInputDto.CnpjPaying;
                clientDao.Gender = clientInputDto.Gender;
                clientDao.NatureBackground = clientInputDto.CompanyNatureType;
                clientDao.TypeNoteEmited = clientInputDto.TypeNoteEmited;
                clientDao.ListBanks = new List<BankDao>();
                clientDao.ListContactDay = new List<ContactDayDao>();
                clientDao.ListContactTime = new List<ContactTimeDao>();
                clientDao.ListDocuments = new List<FileDao>();
                clientDao.ListTelephones = new List<TelephoneDao>();
            }

            clientDao.ListBanks.Clear();

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

            clientDao.ListTelephones.Clear();

            if (clientInputDto.Telephones?.Count > 0)
            {
                TelephoneDao newTelephone = null;

                foreach (var telephone in clientInputDto.Telephones)
                {
                    newTelephone = new TelephoneDao()
                    {
                        Number = telephone.Number,
                        ContactName = telephone.ContactName,
                        TelephoneType = telephone.TelephoneType
                    };

                    clientDao.ListTelephones.Add(newTelephone);
                }
            }

            clientDao.ListDocuments.Clear();

            if (clientInputDto.Documents?.Count > 0)
            {
                foreach (var photo in clientInputDto.Documents)
                {
                    try
                    {
                        var stringToConvert = FileManager.RemovePathString(photo);

                        FileDao fileDao = new FileDao()
                        {
                            Name = photo.FileName,
                            CreateDate = DateTime.Now,
                            Content = Convert.FromBase64String(stringToConvert),
                            MimeType = photo.MimeType,
                        };

                        fileRepository.CreateOrUpdate(fileDao);

                        clientDao.ListDocuments.Add(fileDao);

                    }
                    catch (Exception e)
                    {
                        throw new BusinessException("Erro ao inserir foto do brasão");
                    }
                }
            }

            clientRepository.CreateOrUpdate(clientDao);

            clientDao.ListContactDay.Clear();

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

                    clientRepository.CreateContactHour(newContactDay);
                }
            }

            clientDao.ListContactTime.Clear();

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

                    clientRepository.CreateContactTime(newContactTime);
                }
            }

            clientRepository.CreateOrUpdate(clientDao);

            userDao.Client = clientDao;
            userRepository.CreateOrUpdate(userDao);

            return "Seu Cadastro foi Finalizado com Sucesso";
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

        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

    }
}