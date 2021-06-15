using jay.school.contracts.Contracts;
using jay.school.contracts.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace jay.school.bussiness.Bussiness
{
    public class FileDocBussiness : IFileDocService
    {
        private IHostingEnvironment _hostingEnvironment;
        public FileDocBussiness(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public CustomResponse<FileDoc> UploadFile(FileDoc fileDoc1)
        {
            string webRootPath2 = _hostingEnvironment.ContentRootPath;
            try
            {
                // var file = fileDoc1.File[0];

                string folderName = "";

                switch (fileDoc1.From)
                {
                    case "student_profile":

                        folderName = "Res/Student/Profile";

                        break;

                    case "assignment":

                        folderName = "Res/Assignments/" + fileDoc1.UploadingDate + "/" + fileDoc1.ClassSection + "/" + fileDoc1.Subject + "/" + fileDoc1.StudentName + "_" + fileDoc1.Sid;

                        break;
                }
                string webRootPath = "/var/www/data/";
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                List<AssignmentFiles> afiles = new List<AssignmentFiles>();
                if (fileDoc1.File.Count > 0)
                {
                    afiles.Clear();
                    foreach (var filez in fileDoc1.File)
                    {
                        string fileName = "";
                        if (filez.Length > 0)
                        {
                            fileName = ContentDispositionHeaderValue.Parse(filez.ContentDisposition).FileName.Trim('"');
                            string fullPath = Path.Combine(newPath, fileName);
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                filez.CopyTo(stream);
                            }
                            AssignmentFiles f = new AssignmentFiles
                            {
                                ImgUrl = fullPath,
                                Key = afiles.Count + 1,
                                Type = fileDoc1.FileType,
                                UploadedDate = fileDoc1.UploadingDate

                            };
                            afiles.Add(f);
                        }
                    }
                    fileDoc1.FilePath = afiles;
                }
                return new CustomResponse<FileDoc>(1, fileDoc1, null);
            }
            catch (System.Exception ex)
            {
                return new CustomResponse<FileDoc>(0, null, ex.Message + webRootPath2);
            }
        }


    }
}

