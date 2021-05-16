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
                string webRootPath = _hostingEnvironment.WebRootPath;
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
                return new CustomResponse<FileDoc>(0, null, ex.Message);
            }
        }

     
    }
}
