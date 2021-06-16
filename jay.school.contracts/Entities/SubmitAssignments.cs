using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.contracts.Entities
{
    public class SubmitAssignments
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string AssignmentId { get; set; }
        public string ActualDate { get; set; }
        public string ActualEndDate { get; set; }
        public string SubmittedDate { get; set; }
        public List<AssignmentFiles> FileUrls { get; set; }
        public string StudentName { get; set; }
        public string Sid { get; set; }
        public string Std { get; set; }
        public string Section { get; set; }
        public string StuImg { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string StudentRemark { get; set; }
        public string MarksObtained { get; set; }
        public string TotalMarks { get; set; }

    }

    public class AssignmentFiles
    {
        public string ImgUrl { get; set; }
        public string UploadedDate { get; set; }
        public string Type { get; set; }
        public int Key { get; set; }
        public IFormFile AssigFile { get; set; }
        public bool isUploaded { get; set; }
        public bool isUploading { get; set; }
    }
}
