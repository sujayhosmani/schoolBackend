using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.contracts.Entities
{
    public class Student
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Setion { get; set; }
        public string ClassNsection { get => ClassNsection; set { ClassNsection = Class + Setion; } }
        public string AdmissionNo { get; set; }
        public string RollNo { get; set; }
        public string Email { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherOccupation { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherPh { get; set; }
        public string FatherPh { get; set; }
        public string PendingFee { get; set; }
        public string TotalPaidFee { get; set; }

    }
}
