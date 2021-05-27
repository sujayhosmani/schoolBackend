using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace jay.school.contracts.Entities
{
    public class SubjectsModel 
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }   
        public int SubjectCode { get; set; }
        public string Subject { get; set; }

    }

}