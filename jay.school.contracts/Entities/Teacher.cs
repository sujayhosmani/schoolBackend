using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.contracts.Entities
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public List<string> Sections { get; set; }
        public List<string> Subjects { get; set; }
        public string PhoneNum { get; set; }
        public string Level { get; set; }
    }
}
