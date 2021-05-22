using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace jay.school.contracts.Entities
{
    public class Student
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Gender { get; set; }
        // public string ClassNsection { get => ClassNsection; set { ClassNsection = Class + Section; } }
        public string AdmissionNo { get; set; }
        public string RollNo { get; set; }
        public string Email { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherOccupation { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherPh { get; set; }
        public string FatherPh { get; set; }
        public string TotalPaidFee { get; set; }
        public string DateOfJoining { get; set; }
        public string DateOfBirth { get; set; }
        public string ImageUrl { get; set; }
        public Address PermanentAddress { get; set; }
        public Address CurrentAddress { get; set; }
        public string AdmissionCopy { get; set; }
         public string Password { get; set; }


    }
}
