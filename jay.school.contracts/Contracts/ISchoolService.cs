﻿using jay.school.contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jay.school.contracts.Contracts
{
    public interface ISchoolService
    {
        Task<CustomResponse<Student>> AddStudent(Student student);
        Task<CustomResponse<Student>> GetStudentsByPh(string ph);
        Task<CustomResponse<Student>> GetStudentsById(string id);
        Task<CustomResponse<List<Student>>> GetStudentsByClass(string cls, string sec);
        Task<CustomResponse<List<Student>>> GetAllStudents();
        Task<CustomResponse<List<TimeTable>>> GetTimeTables(string from, string std, string section);
        Task<CustomResponse<List<SubjectsModel>>> GetSubjects();
        Task<CustomResponse<string>> AddStudents(List<Student> students);
        Task<CustomResponse<string>> AddTimeTables(List<TimeTable> timeTables);
        Task<CustomResponse<string>> AddSubjects(List<SubjectsModel> subjects);
    }
}
