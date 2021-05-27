using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace jay.school.contracts.Entities
{
    public class Attendance
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } // room id
        public string TimeTableId { get; set; }
        public string OnlineClassId { get; set; }
        public DateTime CurrentDate { get; set; }
        public bool StudentId { get; set; }
        public string StudentName { get; set; }
        public string AdmissionNo { get; set; }
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