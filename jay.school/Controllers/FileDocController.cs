using jay.school.contracts.Contracts;
using jay.school.contracts.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace jay.school.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileDocController : ControllerBase
    {
        private readonly IFileDocService _fileDocService;
        private IHostingEnvironment _hostingEnvironment;

        public FileDocController(IFileDocService fileDocService, IHostingEnvironment hostingEnvironment)
        {
            _fileDocService = fileDocService;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("uploadFile")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<CustomResponse<FileDoc>>> UploadFile([FromForm] FileDoc fileDoc)
        {

            //fileDoc.File = Request.Form.Files[0];

            FileDoc fileDoc1 = new FileDoc
            {
                File = Request.Form.Files[0],

                FileName = Request.Form["FileName"],

                FileType = Request.Form["FileType"],

                From = Request.Form["From"]
            };

            //string folderName 

            

            return await _fileDocService.UploadFile(fileDoc);

        }  


        [Route("multipleFiles")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<CustomResponse<MultipleFileDoc>>> multipleFiles(MultipleFileDoc multipleFileDoc)
        {

            FileDoc fileDoc1 = new FileDoc
            {
                File = Request.Form.Files[0],

                FileName = Request.Form["FileName"],

                FileType = Request.Form["FileType"],

                From = Request.Form["From"]
            };

            return await _fileDocService.multipleFiles(multipleFileDoc);

        }  
        
    }
}
