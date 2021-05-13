using jay.school.bussiness.Repository;
using jay.school.contracts.Contracts;
using jay.school.contracts.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jay.school.bussiness.Bussiness
{
    public class TeacherBussiness : ITeachersService
    {
        private readonly IMDBContext _teacherMDBContext;
        private readonly IMongoCollection<Teacher> _teacher;
        public TeacherBussiness(IMDBContext context)
        {
            _teacherMDBContext = context;
            _teacher = _teacherMDBContext.GetCollection<Teacher>(typeof(Teacher).Name);
        }
        public async Task<CustomResponse<string>> AddTeachers(CustomRequest<List<Teacher>> customRequest)
        {
            await _teacher.InsertManyAsync(customRequest.Data);

            return new CustomResponse<string>(1, "Inserted Successfully", null);
           
        }

        public async Task<CustomResponse<Teacher>> GetTeacher(int id)
        {
            Teacher teacher = await _teacher.FindAsync(e => e.TeacherId == id).Result.FirstOrDefaultAsync();

            return new CustomResponse<Teacher>(1, teacher, null);
        }

        public Task<CustomResponse<Teacher>> GetTeacherBySection(string section)
        {
            throw new NotImplementedException();
        }

        public Task<CustomResponse<List<Teacher>>> GetTeacherBySections(string section)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomResponse<List<Teacher>>> GetTeachers()
        {
          List<Teacher> teachers =   await _teacher.FindAsync(e => true).Result.ToListAsync();

            return new CustomResponse<List<Teacher>>(1, teachers, null);
        }
    }
}
