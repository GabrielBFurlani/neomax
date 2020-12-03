////-----------------------------------------------------------------------
//// <copyright file="FileManager.cs" company="Gabriel Furlani">
////  (R) Registrado 2020 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Business
{
    using System;
    using System.IO;
    using DocumentFormat.OpenXml.Packaging;
    using Neomax.Data.Util;
    using Neomax.Model.Util;
    using System.Linq;
    using DocumentFormat.OpenXml.Spreadsheet;
    using System.Collections.Generic;
    using System.Web;
    using System.Collections;
    using System.IO.Compression;
    using Neomax.Model.Exception;
    using Neomax.Data.Repository;

    /// <summary>
    /// Manager for files persisted by the application
    /// </summary>
    public class FileManager
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Basic file operations

        public void UnlinkDoc(int idUser, int idFile)
        {
            FileRepository fileRepository = new FileRepository();

            var file = fileRepository.GetById(idFile);

            UserRepository userRepository = new UserRepository();

            var user = userRepository.GetById(idUser);

            var fileToUnlink = user.Client.ListDocuments.FirstOrDefault(x => x.Id == idFile);

            if (fileToUnlink == null)
            {
                throw new BusinessException("Não foi possível recuperar o arquivo");
            }

            user.Client.ListDocuments.Remove(fileToUnlink);
        }

        public void UnlinkPhoto(int idUser)
        {
            UserRepository userRepository = new UserRepository();

            var user = userRepository.GetById(idUser);

            user.PhotoDao = null;

            userRepository.CreateOrUpdate(user);
        }

        public HttpFileBase64Dto CreateBase64WithFile(Neomax.Data.DataAccess.FileDao file)
        {
            HttpFileBase64Dto fileBase64Dto = new HttpFileBase64Dto();

            try
            {
                ////Converts the image to base64 string
                fileBase64Dto.ImageBase64 = Convert.ToBase64String(file.Content);

                ////Generates the the MIME Type from file extension 
                fileBase64Dto.MimeType = file.MimeType;

                fileBase64Dto.FileId = file.Id;

                fileBase64Dto.FileName = file.Name;

                return fileBase64Dto;
            }
            catch (Exception)
            {
                throw new BusinessException("Não foi possível recuperar o arquivo");
            }
        }

        /// <inheritdoc cref="IFileManager.GetBase64ByIdFile(int)"/>
        public HttpFileBase64Dto GetBase64ByIdFile(int idFile)
        {
            if (idFile == 0)
            {
                throw new BusinessException("Código do arquivo não informado");
            }

            FileRepository fileRepository = new FileRepository();

            Data.DataAccess.FileDao file = fileRepository.GetById(idFile);

            if (file == null)
            {
                return null;
            }

            return CreateBase64WithFile(file);
        }

        /// <summary>
        /// Takes only the image data and sets aside the type / patch of the image file
        /// <param name="imageBase64Dto">Image File</param>
        /// </summary>
        /// <returns>List of HttFileDto</returns>
        public static string RemovePathString(ImageBase64Dto imageBase64Dto)
        {
            int indexStartData;
            var imgData = "";

            if (imageBase64Dto.MimeType == "image/png")
            {
                indexStartData = imageBase64Dto.ImageData.ToLower().IndexOf("base64,");

                imgData = imageBase64Dto.ImageData.Substring(indexStartData + 7);
            }

            //jpeg or jpg
            indexStartData = imageBase64Dto.ImageData.IndexOf("/9j/");

            if (indexStartData >= 0)
                imgData = imageBase64Dto.ImageData.Substring(indexStartData + 4);

            return imgData;
        }

        /// <summary>
        /// Takes only the image data and sets aside the type / patch of the image file
        /// <param name="imageBase64Dto">Image File</param>
        /// </summary>
        /// <returns>List of HttFileDto</returns>
        public static string RemovePathString(HttpFileBase64Dto imageBase64Dto)
        {
            int indexStartData;
            var imgData = "";

            if (imageBase64Dto.MimeType == "image/png")
            {
                indexStartData = imageBase64Dto.ImageBase64.ToLower().IndexOf("base64,");

                imgData = imageBase64Dto.ImageBase64.Substring(indexStartData + 7);
            }

            if (imageBase64Dto.MimeType == "application/pdf")
            {
                indexStartData = imageBase64Dto.ImageBase64.IndexOf(",");

                if (indexStartData >= 0)
                    imgData = imageBase64Dto.ImageBase64.Substring(indexStartData + 1);
            }

            if (imageBase64Dto.MimeType == "image/jpg" || imageBase64Dto.MimeType == "image/jpeg")
            {
                //jpeg or jpg
                indexStartData = imageBase64Dto.ImageBase64.IndexOf("/9j/");

                if (indexStartData >= 0)
                    imgData = imageBase64Dto.ImageBase64.Substring(indexStartData + 4);
            }

            return imgData;
        }

        /// <summary>
        /// Convert a HttpFileCollection to a list of HttpFileDto (standard DTO, used on all other methods)
        /// <param name="fileCollection">File Collection</param>
        /// </summary>
        /// <returns>List of HttFileDto</returns>
        public static List<HttpFileDto> ConvertFromHttpFileCollection(HttpFileCollection fileCollection)
        {
            if (fileCollection != null && fileCollection.Count > 0)
            {
                //// Convert the HttpFileCollection to a list of HttpPostedFile (to became enumerable)
                List<HttpPostedFile> httpPostedFiles = Enumerable.Range(0, fileCollection.Count).Select(i => fileCollection[i]).ToList();

                //// Convert each HttpPostedFile to HttpFileDto

                // prepare the Http File list
                List<HttpFileDto> httpFiles = new List<HttpFileDto>();

                // convert each file
                foreach (var httpPostedFile in httpPostedFiles)
                {
                    // convert the file's stream to MemoryStream (in order to be able to convert to byte array later)
                    MemoryStream fileMemoryStream = new MemoryStream();
                    httpPostedFile.InputStream.CopyTo(fileMemoryStream);

                    var httpFile = new HttpFileDto()
                    {
                        Content = fileMemoryStream.ToArray(),   // convert to byte array
                        FileName = httpPostedFile.FileName,
                        MimeType = httpPostedFile.ContentType
                    };

                    // add the converted Http File to the file list
                    httpFiles.Add(httpFile);
                }

                // return the Http File list
                return httpFiles;
            }
            else
            {
                // The collection is empty, so do not return any list
                return null;
            }
        }

        /// <summary>
        /// Saves a Http File
        /// </summary>
        /// <param name="httpFile">Http File</param>
        /// <returns>File's real name (name at file system)</returns>
        public static string SaveHttpFile(HttpFileDto httpFile)
        {
            try
            {
                ////Generates unique file name with extension (the same extension of the file's suggested name)
                string uniqueFileName = string.Format(@"{0}", Guid.NewGuid()) + Path.GetExtension(httpFile.FileName);

                ////Saves the Http File
                string filePath = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.FileDirectoryPath) + "\\" + uniqueFileName;
                File.WriteAllBytes(filePath, httpFile.Content);

                return uniqueFileName;
            }
            catch (Exception)
            {
                throw new BusinessException("Não foi possível salvar o arquivo " + httpFile.FileName);
            }
        }

        /// <summary>
        /// Reads a file as Http file
        /// </summary>
        /// <param name="fileName">file's name (for user visualization)</param>
        /// <param name="fileRealName">File's real name (at file system)</param>
        /// <returns>File object</returns>
        public static HttpFileDto ReadAsHttpFile(string fileName, string fileRealName)
        {
            HttpFileDto httpFileDto = new HttpFileDto();

            try
            {
                //// Names the file for user visualization
                httpFileDto.FileName = fileName;

                ////Reads the file content
                string filePath = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.FileDirectoryPath) + "\\" + fileRealName;
                httpFileDto.Content = File.ReadAllBytes(filePath);

                ////Generates the the MIME Type from file extension 
                httpFileDto.MimeType = MimeMapping.GetMimeMapping(fileRealName);

                ////Returns the file DTO
                return httpFileDto;
            }
            catch (Exception)
            {
                throw new BusinessException("Não foi possível recuperar o arquivo " + fileName);
            }
        }

        /// <summary>
        /// Deletes a file by name
        /// </summary>
        /// <param name="fileRealName">File's real name (at file system)</param>
        public static void DeleteFile(string fileRealName)
        {
            try
            {
                ////Gets the complete file path
                string filePath = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.FileDirectoryPath) + "\\" + fileRealName;

                ////Deletes the file
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    throw new BusinessException("O arquivo não existe");
                }
            }
            catch (Exception)
            {
                throw new BusinessException("Não foi possível excluir o arquivo");
            }
        }

        #endregion

        #region Image (Base64) file operations

        /// <summary>
        /// Save a base64 image as a file
        /// </summary>
        /// <param name="base64Image">Base64 Image string</param>
        /// <returns>File's real name (name at file system)</returns>
        public static string SaveBase64Image(ImageBase64Dto imageBase64Dto)
        {
            try
            {
                ////Extracts the file extension from the MIME Type
                int index = imageBase64Dto.MimeType.IndexOf('/');
                string extension = imageBase64Dto.MimeType.Substring(index + 1).ToLower();

                ////Generates unique file name with extension
                string uniqueFileName = string.Format(@"{0}", Guid.NewGuid()) + "." + extension;

                ////Prepares file path and image data
                string filePath = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.FileDirectoryPath) + "\\" + uniqueFileName;
                byte[] imageByteArray = Convert.FromBase64String(imageBase64Dto.ImageData);

                ////Writes to file
                File.WriteAllBytes(filePath, imageByteArray);

                return uniqueFileName;
            }
            catch (Exception)
            {
                throw new BusinessException("Não foi possível salvar a imagem");
            }
        }

        /// <summary>
        /// Reads a file as base64 image
        /// </summary>
        /// <param name="fileRealName">File's real name (at file system)</param>
        /// <returns>Image object</returns>
        public static ImageBase64Dto ReadAsBase64Image(string fileRealName)
        {
            ImageBase64Dto imageBase64Dto = new ImageBase64Dto();

            try
            {
                ////Reads the file
                string filePath = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.FileDirectoryPath) + "\\" + fileRealName;
                byte[] imageByteArray = File.ReadAllBytes(filePath);

                ////Converts the image to base64 string
                imageBase64Dto.ImageData = Convert.ToBase64String(imageByteArray);

                ////Generates the the MIME Type from file extension 
                imageBase64Dto.MimeType = MimeMapping.GetMimeMapping(fileRealName);

                ////Returns the DTO with image data (base64 string) and MIME Type
                return imageBase64Dto;
            }
            catch (Exception)
            {
                throw new BusinessException("Não foi possível recuperar a imagem");
            }
        }

        #endregion

        #region zip operations

        /// <summary>
        /// Reads multiples files as a zip Http file
        /// </summary>
        /// <param name="fileIdentifications">List of file identifications (name and real name)</param>
        /// <returns>Zip File object</returns>
        public static HttpFileDto ReadFilesAsZipHttpFile(IList<FileIdentificationDto> fileIdentifications)
        {
            //// Prepares the zip http file
            HttpFileDto httpFileDto = new HttpFileDto()
            {
                Content = null,
                FileName = "arquivos.zip",
                MimeType = "application/zip"
            };

            try
            {
                //// Creates a memory stream for the zip file 
                using (var zipFileStream = new MemoryStream())
                {
                    //// Creates a zip package for the zip stream
                    using (ZipArchive zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create, true))
                    {
                        //// For each file
                        foreach (var fileIdentification in fileIdentifications)
                        {
                            //// Reads the file content (with real name)
                            string filePath = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.FileDirectoryPath) + "\\" + fileIdentification.RealName;
                            byte[] fileContent = File.ReadAllBytes(filePath);

                            //// Creates an entry at zip archive for the file (with the name for user visualiztion)
                            ZipArchiveEntry fileInZipArchive = zipArchive.CreateEntry(fileIdentification.Name, CompressionLevel.Optimal);

                            using (var entryStream = fileInZipArchive.Open())
                                entryStream.Write(fileContent, 0, fileContent.Count());
                        }
                    }

                    //// Converts the zip stream to byte array and assigns it to the Http File
                    httpFileDto.Content = zipFileStream.ToArray();
                }

                //Returns the Http File
                return httpFileDto;
            }
            catch (Exception)
            {
                throw new BusinessException("Não foi possível gerar o arquivo zip");
            }
        }

        /// <summary>
        /// Gather all files as independent files (extract from zip folders if needed)
        /// The received files can be a mix o indepent files and zip folders.
        /// This method unzip folders and gather all files to a list of independent files.
        /// <param name="receivedFiles">List of received files (independent files and/or zip folders)</param>
        /// </summary>
        /// <returns>List of independent files</returns>
        public static List<HttpFileDto> GatherAllFiles(IList<HttpFileDto> receivedFiles)
        {
            //// Create the independent files list
            List<HttpFileDto> independentFiles = new List<HttpFileDto>();

            //// For each received file (zip folder or file)
            foreach (var httpFile in receivedFiles)
            {
                //// Checks whether is a zip folder or independent file
                if (httpFile.MimeType.Equals("application/zip") || httpFile.MimeType.Equals("application/x-zip-compressed"))
                {
                    //// The file is a zip folder. Unzip and extract the files

                    // create a stream from byte array (for the zip file)
                    Stream zipStream = new MemoryStream(httpFile.Content);

                    // unzip the stream
                    using (ZipArchive zipArchive = new ZipArchive(zipStream))
                    {
                        // for each file inside the zip folder
                        foreach (ZipArchiveEntry entry in zipArchive.Entries)
                        {
                            // convert the file's stream to MemoryStream (in order to be able to convert to byte array later)
                            MemoryStream entryMemoryStream = new MemoryStream();
                            entry.Open().CopyTo(entryMemoryStream);

                            var httpfile = new HttpFileDto()
                            {
                                Content = entryMemoryStream.ToArray(),    //convert to byte array
                                FileName = entry.Name,
                                MimeType = MimeMapping.GetMimeMapping(entry.Name)
                            };

                            // add the extracted file to the independent file list
                            independentFiles.Add(httpfile);
                        }
                    }
                }
                else
                {
                    //// The file already is independent. Just add to the independent file list
                    independentFiles.Add(httpFile);
                }
            }

            return independentFiles;
        }

        #endregion        
    }
}