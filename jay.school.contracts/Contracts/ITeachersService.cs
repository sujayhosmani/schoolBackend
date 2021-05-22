using jay.school.contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jay.school.contracts.Contracts
{
    public interface ITeachersService
    {
        Task<CustomResponse<List<Teacher>>> GetTeachers();
        Task<CustomResponse<Teacher>> GetTeacher(string id);
        Task<CustomResponse<List<Teacher>>> GetTeacherBySections(string section);
        Task<CustomResponse<Teacher>> GetTeacherBySection(string section);
        Task<CustomResponse<string>> AddTeachers(CustomRequest<Teacher> customRequest);
    }
}
