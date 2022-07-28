using Eventsapp.Repositories;
using EventsApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace eventsapp.WebAPI.Controllers
{
    [RoutePrefix("api/Upload")]
    public class UploadController : ApiController
    {
        private readonly IUserRepository userRepository;
        public UploadController(IUserRepository _userRepository)
        {
            this.userRepository = _userRepository;
        }

        [HttpGet]
        [Route("ZipFiles")]
        public async Task<IHttpActionResult> ZipFiles()
        {
            string folderPath = System.Configuration.ConfigurationManager.AppSettings["WebAPIFolder"];
            HttpResponseMessage response = Request.CreateResponse();

            try
            {
                var qrImages = (await this.userRepository.QueryAsync(x => (x.RegistrationTypeId == (int?)RegistrationTypeEnum.Official ||
                                x.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete ||
                                x.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest) &&
                                !string.IsNullOrEmpty(x.QrImage))).Select(y => y.QrImage).ToArray();
                string sourcePath = @folderPath + "/Uploads/Attachment/";
                string targetPath = @folderPath + "/Uploads/Attachment/QRImages";
                string zipPath = @folderPath + "/Uploads/Attachment/QRImages.zip";
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                if (File.Exists(zipPath))
                {
                    File.Delete(@zipPath);
                }

                var virtualfilepath = "~//Uploads/Attachment/QRImages.zip";
                var absulotePath = VirtualPathUtility.ToAbsolute(virtualfilepath);
                foreach (var fileName in qrImages)
                {
                    var FilePath = folderPath + "/Uploads/Attachment/" + fileName;

                    // Use Path class to manipulate file and directory paths.
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    string destFile = System.IO.Path.Combine(targetPath, fileName);

                    // To copy a folder's contents to a new location:
                    // Create a new target folder, if necessary.
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.IO.Directory.CreateDirectory(targetPath);
                    }

                    // To copy a file to another location and 
                    // overwrite the destination file if it already exists.
                    System.IO.File.Copy(sourceFile, destFile, true);
                }
                ZipFile.CreateFromDirectory(targetPath, zipPath);
                System.IO.DirectoryInfo di = new DirectoryInfo(targetPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                return Ok(new { fileName = "QRImages.zip", httpPath = Request.RequestUri.GetLeftPart(UriPartial.Authority) + absulotePath });
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        [Route("ZipFilesWithNames")]
        public async Task<IHttpActionResult> ZipFilesWithNames()
        {
            string folderPath = System.Configuration.ConfigurationManager.AppSettings["WebAPIFolder"];
            HttpResponseMessage response = Request.CreateResponse();

            try
            {
                var qrImagesUsers = (await this.userRepository.QueryAsync(x => (x.RegistrationTypeId == (int?)RegistrationTypeEnum.Official ||
                                x.RegistrationTypeId == (int?)RegistrationTypeEnum.Athlete ||
                                x.RegistrationTypeId == (int?)RegistrationTypeEnum.Guest) &&
                                x.IsActive == true &&
                                !string.IsNullOrEmpty(x.QrImage))).ToList();
                string sourcePath = @folderPath + "/Uploads/Attachment/";
                string targetPath = @folderPath + "/Uploads/Attachment/QRImagesUsers";
                string zipPath = @folderPath + "/Uploads/Attachment/QRImagesUsers.zip";
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                if (File.Exists(zipPath))
                {
                    File.Delete(@zipPath);
                }

                var virtualfilepath = "~//Uploads/Attachment/QRImagesUsers.zip";
                var absulotePath = VirtualPathUtility.ToAbsolute(virtualfilepath);
                foreach (var fileNameUser in qrImagesUsers)
                {
                    var FilePath = folderPath + "/Uploads/Attachment/" + fileNameUser.QrImage;

                    // Use Path class to manipulate file and directory paths.
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileNameUser.QrImage);
                    string destFile = System.IO.Path.Combine(targetPath, fileNameUser.FirstName + "_" + fileNameUser.LastName + "_" + fileNameUser.QrImage);

                    // To copy a folder's contents to a new location:
                    // Create a new target folder, if necessary.
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.IO.Directory.CreateDirectory(targetPath);
                    }

                    // To copy a file to another location and 
                    // overwrite the destination file if it already exists.
                    if(File.Exists(sourceFile))
                    {
                        System.IO.File.Copy(sourceFile, destFile, true);
                    }
                }
                ZipFile.CreateFromDirectory(targetPath, zipPath);
                System.IO.DirectoryInfo di = new DirectoryInfo(targetPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                return Ok(new { fileName = "QRImagesUsers.zip", httpPath = Request.RequestUri.GetLeftPart(UriPartial.Authority) + absulotePath });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //TODO: Allow write [authorize] after integrating the work
        [HttpPost]
        [Route("UploadFile")]
        public async Task<IHttpActionResult> UploadFile(string uploadFile)
        {
            var vfileName = Guid.NewGuid().ToString();
            var virtualfilepath = "~/Uploads/" + uploadFile + "/" + vfileName;
            var absulotePath = VirtualPathUtility.ToAbsolute(virtualfilepath);
            var physicalfilepath = HttpContext.Current.Server.MapPath(virtualfilepath);

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                string filename = string.Empty;
                await Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider()).ContinueWith((task) =>
                {
                    MultipartMemoryStreamProvider provider = task.Result;

                    foreach (HttpContent content in provider.Contents)
                    {
                        Stream stream = content.ReadAsStreamAsync().Result;

                        filename = content.Headers.ContentDisposition.FileName.Trim('\"');
                        physicalfilepath = physicalfilepath + filename;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            //ms.ToArray();
                            File.WriteAllBytes(physicalfilepath, ms.ToArray());
                        }
                    }
                });
                return Ok(new { fileName = vfileName + filename, httpPath = Request.RequestUri.GetLeftPart(UriPartial.Authority) + absulotePath + filename });
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("DeleteFile")]
        public IHttpActionResult DeleteFile(string fileTodelete)
        {
            //string folderPath = @"C:\Users\inlogicdev1\Source\Workspaces\New folder\ESMAEServices\eServices\eServices.Web.API\" + folderName + "\ + fileTodelete;// configurationManager.GetConfiguration("UploadsPath");

            try
            {
                var virtualfilepath = "~/Uploads/Attachment/" + fileTodelete;
                var absulotePath = VirtualPathUtility.ToAbsolute(virtualfilepath);
                var physicalfilepath = HttpContext.Current.Server.MapPath(virtualfilepath);
                System.IO.File.Delete(physicalfilepath);
                return Ok("File deleted");
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }
    }
}
