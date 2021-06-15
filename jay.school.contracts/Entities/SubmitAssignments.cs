using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
        public List<string> FileUrls { get; set; }
        public string StudentName { get; set; }
        public string Sid { get; set; }
        public string Std { get; set; }
        public string Section { get; set; }
        public string StuImg { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string MarksObtained { get; set; }
        public string TotalMarks { get; set; }
        
    }
}
