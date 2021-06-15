using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.contracts.Entities
{
    public  class FileDoc
    {
        public string  Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string From { get; set; }
        public string FilePath { get; set; }
        [JsonIgnore]
        public IFormFile File { get; set; }
    }
}
