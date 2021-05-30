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
        public string Std { get; set; }
        public string Section { get; set; }
        public List<WeekSubjects> weekSub { get; set; }
        public string FromTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public string UploadedById { get; set; }

    }

    public class WeekSubjects{
        public string Week { get; set; }
        public string CTSId { get; set; }
        public string TId { get; set; }
        public string SubjectId { get; set; }
    }

    public class FullTimeTable{
        public List<TimeTable> timeTable { get; set; }
        public List<CTSModel> ctsModel { get; set; }
        public List<SubjectsModel> subjects { get; set; }
        public List<Teacher> teacher { get; set; }
    }

}