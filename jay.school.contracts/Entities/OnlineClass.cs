using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace jay.school.contracts.Entities
{
    public class OnlineClass
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } // room id
        public string TimeTableId { get; set; }
        public DateTime CurrentDate { get; set; }
        public bool Status { get; set; }
        public string Subject { get; set; }
        public string SubjectCode { get; set; }
        public string Topic { get; set; }
        public string Std { get; set; }
        public string Section { get; set; }
        public string TeacherName { get; set; }
        public string TeacherId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Duration { get; set; }

    }

}