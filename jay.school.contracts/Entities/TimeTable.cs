using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace jay.school.contracts.Entities
{
    public class TimeTable
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Subject { get; set; }
        public string SubjectCode { get; set; }
        public string Std { get; set; }
        public string Section { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime EndTime { get; set; }
        public String Week {get; set;}
        public string Duration { get; set; }
        public string UploadedById { get; set; }

    }

}