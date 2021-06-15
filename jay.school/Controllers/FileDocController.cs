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

        public FileDocController(IFileDocService fileDocService)
        {
            _fileDocService = fileDocService;
        }
        
        
        [Route("ping")]
        [HttpGet]
        public ActionResult checkhealth()
        {

            return Ok("working..." + DateTime.Now.ToString());

        }
        
        [Route("UploadFile")]
        [HttpPost, DisableRequestSizeLimit]
        public ActionResult<CustomResponse<FileDoc>> UploadFile([FromForm] FileDoc fileDoc)
        {

            //fileDoc.File = Request.Form.Files[0];

            FileDoc fileDoc1 = new FileDoc
            {
                File = Request.Form.Files,

                FileName = Request.Form["FileName"],

                FileType = Request.Form["FileType"],

                From = Request.Form["From"]
            };

            //string folderName 

            

            return  _fileDocService.UploadFile(fileDoc);

        }  
    }
}

