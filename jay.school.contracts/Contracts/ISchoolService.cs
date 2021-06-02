using jay.school.contracts.Entities;
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
        Task<CustomResponse<string>> AddStudents(List<Student> students);
        
        
    }
}
