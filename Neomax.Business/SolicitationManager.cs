////-----------------------------------------------------------------------
//// <copyright file="SolicitationManager.cs" company="Gabriel Furlani">
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
    using DocumentFormat.OpenXml.Office2010.Excel;

    /// <summary>
    /// Manages business rules related to profile
    /// </summary>
    public class SolicitationManager
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets an solicitation by its identifier
        /// </summary>
        /// <param name="loggedSolicitation">Logged Solicitation</param>
        /// <param name="id">Solicitation identifier</param>
        /// <returns>Solicitation object</returns>
        public static SolicitationDto GetById(int id)
        {
            FileManager fileManager = new FileManager();
            SolicitationRepository solicitationRepository = new SolicitationRepository();

            SolicitationDao solicitationDao = solicitationRepository.GetById(id);

            if (solicitationDao == null)
            {
                throw new BusinessException("Solicitação não encontrada");
            }

            SolicitationDto solicitationDto = Mapper.Map<SolicitationDto>(solicitationDao);

            //add products and files

            return solicitationDto;
        }

        public static string Create(SolicitationInputDto solicitationInputDto)
        {
            SolicitationRepository solicitationRepository = new SolicitationRepository();

            FileRepository fileRepository = new FileRepository();

            UserRepository userRepository = new UserRepository();

            FileManager fileManager = new FileManager();

            ClientRepository clientRepository = new ClientRepository();

            if (solicitationInputDto == null)
            {
                throw new BusinessException("Solicitação não informado");
            }

            ClientDao clientDao = clientRepository.GetById(solicitationInputDto.IdClient.Value);

            if (clientDao == null)
            {
                throw new BusinessException("Cliente não encontrado");
            }

            SolicitationDao solicitationDao = new SolicitationDao()
            {
                Client = clientDao,
                CreationDate = DateTime.Now,
                Status = SolicitationStatus.WaitingApprove,
                Protocol = GenerateProtocolNumber(),
                ProductsList = new List<SolicitationProductDao>()
            };

            solicitationRepository.CreateOrUpdate(solicitationDao);

            if (solicitationInputDto.Products?.Count > 0)
            {
                SolicitationProductDao newProductDao = null;

                foreach (var product in solicitationInputDto.Products)
                {
                    newProductDao = new SolicitationProductDao()
                    {
                        CreationDate = DateTime.Now,
                        ProductName = product.ProductType,
                        Solicitation = solicitationDao,
                        Status = SolicitationStatus.WaitingApprove,
                        Title = product.Title,
                        CNPJPayingSource = product.CNPJPayingSource.Replace(".", "").Replace("/", "").Replace("-", "")
                    };

                    if (product.Files != null)
                    {
                        foreach (var file in product.Files)
                        {
                            //fazer
                        }
                    }

                    solicitationDao.ProductsList.Add(newProductDao);
                }
            }

            solicitationRepository.CreateOrUpdate(solicitationDao);

            return "Solicitação enviada com sucesso!";
        }

        public static string Update(int? id, SolicitationInputDto solicitationInputDto)
        {
            SolicitationRepository solicitationRepository = new SolicitationRepository();

            FileRepository fileRepository = new FileRepository();

            UserRepository userRepository = new UserRepository();

            FileManager fileManager = new FileManager();

            ClientRepository clientRepository = new ClientRepository();

            if (solicitationInputDto == null)
            {
                throw new BusinessException("Solicitação não informado");
            }

            ClientDao clientDao = clientRepository.GetById(solicitationInputDto.IdClient.Value);

            if (clientDao == null)
            {
                throw new BusinessException("Cliente não encontrado");
            }

            SolicitationDao solicitationDao = solicitationRepository.GetById(id.Value);

            if (clientDao.Id.Value != solicitationDao.Client.Id)
            {
                throw new BusinessException("Você não pode alterar solicitações de outras pessoas!");
            }

            if (solicitationInputDto.Products?.Count > 0)
            {
                SolicitationProductDao productDao = solicitationDao.ProductsList.FirstOrDefault();

                foreach (var product in solicitationInputDto.Products)
                {
                    productDao.CNPJPayingSource = product.CNPJPayingSource;
                    productDao.Title = product.Title;

                    foreach (var file in product.Files)
                    {
                        //fazer
                    }
                }

                solicitationRepository.CreateOrUpdate(solicitationDao);
            }

            solicitationRepository.CreateOrUpdate(solicitationDao);

            return "Solicitação atualizada com sucesso!";
        }

        public static void UpdateSingleStatus(int idSolicitationProduct, SolicitationStatus solicitationStatus)
        {
            SolicitationProductRepository solicitationProductRepository = new SolicitationProductRepository();

            var solicitationProduct = solicitationProductRepository.GetById(idSolicitationProduct);

            if (solicitationProduct.Status != SolicitationStatus.RevisionSolicited)
            {
                throw new BusinessException("Solicitação não pode ser alterada!");
            }

            solicitationProduct.Status = solicitationStatus;

            solicitationProductRepository.CreateOrUpdate(solicitationProduct);

            if (solicitationStatus == SolicitationStatus.RevisionSolicited)
            {
                solicitationProduct.Status = solicitationStatus;
            }
            else
            {
                if (solicitationProduct.Solicitation.ProductsList.Count() == solicitationProduct.Solicitation.ProductsList.Where(x => x.Status == solicitationStatus).Count())
                {
                    solicitationProduct.Status = solicitationStatus;
                }
            }
        }

        public static PaginationResponseDto<SolicitationDto> AdminSearch(SolicitationFilterDto filter)
        {
            filter.Argument = string.IsNullOrWhiteSpace(filter.Argument) ? "" : filter.Argument.Replace(".", "").Replace("/", "").Replace("-", "");

            UserRepository userRepository = new UserRepository();

            SolicitationRepository solicitationRepository = new SolicitationRepository();

            var solicitations = solicitationRepository.GetForAdmin(filter);

            foreach (var solicitation in solicitations.Response)
            {
                solicitation.Client.User = userRepository.GetById(solicitation.Client.User.Id.Value);
            }

            PaginationResponseDto<SolicitationDto> paginationResponseDto = new PaginationResponseDto<SolicitationDto>()
            {
                CurrentPage = filter.PageNumber,
                Response = Mapper.Map<List<SolicitationDto>>(solicitations.Response),
                ResultsPerPage = filter.ResultsPerPage,
                TotalResults = solicitations.TotalResults
            };

            return paginationResponseDto;
        }


        public static PaginationResponseDto<SolicitationDto> ClientSearch(SolicitationFilterDto filter)
        {
            filter.Argument = string.IsNullOrWhiteSpace(filter.Argument) ? "" : filter.Argument.Replace(".", "").Replace("/", "").Replace("-", "");

            SolicitationRepository solicitationRepository = new SolicitationRepository();

            var solicitations = solicitationRepository.GetForClient(filter);

            PaginationResponseDto<SolicitationDto> paginationResponseDto = new PaginationResponseDto<SolicitationDto>()
            {
                CurrentPage = filter.PageNumber,
                Response = Mapper.Map<List<SolicitationDto>>(solicitations.Response),
                ResultsPerPage = filter.ResultsPerPage,
                TotalResults = solicitations.TotalResults
            };

            return paginationResponseDto;
        }

        #region private methods

        private static string GenerateProtocolNumber()
        {
            string protocolNumber = "";

            SolicitationRepository solicitationRepository = new SolicitationRepository();

            var lastProtocolNumber = solicitationRepository.GetLastProtocolNumber();

            while (lastProtocolNumber.Length != 6)
            {
                lastProtocolNumber = "0" + lastProtocolNumber;
            }

            protocolNumber = lastProtocolNumber + 1 + "/" + DateTime.Now.Year;

            return protocolNumber;
        }

        #endregion
    }
}