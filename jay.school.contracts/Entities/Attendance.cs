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
        public string OnlineClassId { get; set; }
        public string CurrentDate { get; set; }
        public string StudentName { get; set; }
        public string SubjectId { get; set; }
        public string StudentId { get; set; }
        public string Std { get; set; }
        public string Section { get; set; }
        public string TeacherName { get; set; }
        public string Tid { get; set; }
        public string ActualStartTime { get; set; }
        public string ActualEndTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public string SubjectName{get; set;}

    }

}