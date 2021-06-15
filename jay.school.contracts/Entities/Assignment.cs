using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace jay.school.contracts.Entities
{
    public class Assignment
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Tid { get; set; }
        public string TeacherName { get; set; }
        public string Std { get; set; }
        public string Section { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public string TotalMarks { get; set; }
        
    }
}
