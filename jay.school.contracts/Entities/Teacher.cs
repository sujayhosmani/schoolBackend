using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace jay.school.contracts.Entities
{
    public class Teacher
    {
        // salary details and payment histories and documents/resume about teacher
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string TeacherPh { get; set; }
        public string DateOfJoining { get; set; }
        public string Qualification { get; set; }
        public string Experience { get; set; }
        public bool isCTR { get; set; }
        public string isCTRClass { get; set; }
        public string isCTRSection { get; set; }
        public Address PermanentAddress { get; set; }
        public List<TeacherSubjects> TSubjects {get; set;}
        public string ImageUrl { get; set; }
        public Address CurrentAddress { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }    
        public string Password { get; set; }
        public string LastName { get; set; }    
        
    }

    public class TeacherSubjects
    {
        public string subject {get; set;}
        // public List<TeacherClasses> classes {get; set;}
    }

    public class TeacherClasses
    {
        public string Std {get; set;}
        public string Section {get; set;}
    }
}
