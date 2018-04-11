////-----------------------------------------------------------------------
//// <copyright file="FileManager.cs" company="ZetaCorp">
////  (R) Registrado 2018 ZetaCorp.
////  Desenvolvido por ZETACORP.
//// </copyright>
////-----------------------------------------------------------------------
namespace PlataformaZ2.Business
{
    using System;
    using System.IO;
    using DocumentFormat.OpenXml.Packaging;
    using PlataformaZ2.Data.Util;
    using PlataformaZ2.Model.Util;
    using Model.Exception;
    using System.Linq;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Data.DataAccess;
    using Model.Dto;
    using System.Collections.Generic;
    using System.Web;
    using System.Collections;
    using System.IO.Compression;

    /// <summary>
    /// Manager for files persisted by the application
    /// </summary>
    public class FileManager
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Basic file operations

        /// <summary>
        /// Convert a HttpFileCollection to a list of HttpFileDto (standard DTO, used on all other methods)
        /// <param name="fileCollection">File Collection</param>
        /// </summary>
        /// <returns>List of HttFileDto</returns>
        public static List<HttpFileDto> ConvertFromHttpFileCollection(HttpFileCollection fileCollection)
        {            
            if(fileCollection != null && fileCollection.Count > 0)
            {
                //// Convert the HttpFileCollection to HttpPostedFile (to became enumerable)
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
                        Content = fileMemoryStream.ToArray(),   // convert ot byte array
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
        /// <returns>Operation result</returns>
        public static OperationResult SaveHttpFile(HttpFileDto httpFile)
        {
            try
            {
                ////Generates unique file name with extension (the same extension of the file's suggested name)
                string uniqueFileName = string.Format(@"{0}", Guid.NewGuid()) + Path.GetExtension(httpFile.FileName);

                ////Saves the Http File
                string filePath = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.FileDirectoryPath) + "\\" + uniqueFileName;
                File.WriteAllBytes(filePath, httpFile.Content);
                
                return new OperationResult(true, string.Empty, uniqueFileName);
            }
            catch (Exception)
            {
                return new OperationResult(false, "Não foi possível salvar o arquivo");
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
                throw new BusinessException("Não foi possível recuperar o arquivo");
            }
        }
        
        /// <summary>
        /// Deletes a file by name
        /// </summary>
        /// <param name="fileRealName">File's real name (at file system)</param>
        /// <returns>Operation result</returns>
        public static OperationResult DeleteFile(string fileRealName)
        {
            try
            {
                ////Gets the complete file path
                string filePath = System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.FileDirectoryPath) + "\\" + fileRealName;

                ////Deletes the file
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                    return new OperationResult(true);
                }
                else
                {
                    return new OperationResult(false, "Não foi possível excluir o arquivo");
                }
            }
            catch (Exception)
            {
                return new OperationResult(false, "Não foi possível excluir o arquivo");
            }
        }

        #endregion

        #region Image (Base64) file operations

        /// <summary>
        /// Save a base64 image as a file
        /// </summary>
        /// <param name="base64Image">Base64 Image string</param>
        /// <returns>Operation result with file's real name (name at file system)</returns>
        public static OperationResult SaveBase64Image(ImageBase64Dto imageBase64Dto)
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

                return new OperationResult(true, string.Empty, uniqueFileName);
            }
            catch (Exception)
            {
                return new OperationResult(false, "Não foi possível salvar a imagem");
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