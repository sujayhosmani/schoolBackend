using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace jay.school.contracts.Entities
{
    public class Announcement
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string UploadedId { get; set; }
        public string UploadedBy { get; set; }
        public string StartDate { get; set; }
        public bool isForSchool { get; set; }
        public List<String> StdSec { get; set; }
        
    }
}
