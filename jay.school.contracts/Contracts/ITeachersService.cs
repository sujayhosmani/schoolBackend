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
        Task<CustomResponse<Teacher>> GetClassTeacher(string std, string section);
        Task<CustomResponse<List<CTSModel>>> GetCTS(string std, string section);
        Task<CustomResponse<string>> AddTeachers(CustomRequest<Teacher> customRequest);
        Task<CustomResponse<string>> AddCTS(CustomRequest<List<CTSModel>> ctsList);
    }
}
