using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace jay.school.contracts.Entities
{
    public class CTSModel // class teacher subject model
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } // room id
        public string Section { get; set; }
        // public string SubjectCode { get; set; }
        // public string Subject { get; set; }
        public string Std { get; set; }
        public string TID { get; set; }
        public string SubjectId { get; set; }
        // public string TeacherName { get; set; }
        // public string TeacherId { get; set; }
        // public bool isCTR { get; set; } // class teacher

    }

}