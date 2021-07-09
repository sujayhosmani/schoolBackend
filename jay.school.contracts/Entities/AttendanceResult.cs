using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace jay.school.contracts.Entities
{
    public class AttendanceResult
    {
        public List<OverAll> TotalAvg { get; set; }
    
    }

    public class OverAll
    {
        public string Subject { get; set; }
        public string SubjectId { get; set; }
        public long Total { get; set; }
        public long Present { get; set; }
        public long Absent { get; set; }

    }

}