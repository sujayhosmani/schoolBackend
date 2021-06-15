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
        public async Task<CustomResponse<FileDoc>> UploadFile(FileDoc fileDoc1)
        {
            string webRootPath2 = _hostingEnvironment.ContentRootPath;
            try
            {
                var file = fileDoc1.File;

                string folderName = "";

                switch (fileDoc1.From)
                {
                    case "student_profile":

                        folderName = "Res/Student/Profile";

                        break;
                }
                string webRootPath = "/var/www/data/";
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                string fileName = "";
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    fileDoc1.FilePath = fullPath;
                }

                return new CustomResponse<FileDoc>(1, fileDoc1, null);
            }
            catch (System.Exception ex)
            {
                return new CustomResponse<FileDoc>(0, null, ex.Message + webRootPath2);
            }
        }


        public async Task<CustomResponse<MultipleFileDoc>> multipleFiles(MultipleFileDoc multipleFileDoc)
        {
            try
            {

                string folderName = "";

                switch (multipleFileDoc.From)
                {
                    case "assignments":

                        folderName = "Res/Assignments/" + multipleFileDoc.UploadingDate + "/" + multipleFileDoc.ClassSection + "/" + multipleFileDoc.Subject + "/" + multipleFileDoc.StudentName + "_" + multipleFileDoc.Sid;
                        break;
                }
                string webRootPath = "/var/www/data/";
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                // List<AssignmentFiles> afiles = new List<AssignmentFiles>();
                // afiles.Clear();
                multipleFileDoc.FilePath = new List<AssignmentFiles>();
                for (var i = 0; i < multipleFileDoc.Files.Count; i++)
                {
                    if (multipleFileDoc.Files[i].Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(multipleFileDoc.Files[i].ContentDisposition).FileName.Trim('"');
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await multipleFileDoc.Files[i].CopyToAsync(stream);
                        }
                        AssignmentFiles f = new AssignmentFiles
                        {
                            ImgUrl = fullPath,
                            Key = i + 1,
                            Type = multipleFileDoc.FileType,
                            UploadedDate = multipleFileDoc.UploadingDate

                        };
                        // afiles.Add(f);
                        multipleFileDoc.FilePath.Add(f);
                    }
                }
                multipleFileDoc.Files = null;
                return new CustomResponse<MultipleFileDoc>(1, multipleFileDoc, null);
            }
            catch (System.Exception ex)
            {
                return new CustomResponse<MultipleFileDoc>(0, null, ex.ToString());
            }
        }
    }
}
