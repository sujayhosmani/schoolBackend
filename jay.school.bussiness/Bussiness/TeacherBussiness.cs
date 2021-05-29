using jay.school.bussiness.Repository;
using jay.school.contracts.Contracts;
using jay.school.contracts.Entities;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jay.school.bussiness.Bussiness
{
    public class TeacherBussiness : ITeachersService
    {
        private readonly IMDBContext _teacherMDBContext;
        private readonly IMongoCollection<Teacher> _teacher;
        private readonly IMongoCollection<CTSModel> _cts;
        public TeacherBussiness(IMDBContext context)
        {
            _teacherMDBContext = context;
            _teacher = _teacherMDBContext.GetCollection<Teacher>(typeof(Teacher).Name);
            _cts = _teacherMDBContext.GetCollection<CTSModel>(typeof(CTSModel).Name);
        }

        public async Task<CustomResponse<string>> AddTeachers(CustomRequest<Teacher> customRequest)
        {
            await _teacher.InsertOneAsync(customRequest.Data);

            return new CustomResponse<string>(1, "Inserted Successfully", null);

        }

        public async Task<CustomResponse<Teacher>> GetTeacher(string id)
        {
            Teacher teacher = await _teacher.FindAsync(e => e.TeacherId == id).Result.FirstOrDefaultAsync();

            return new CustomResponse<Teacher>(1, teacher, null);
        }

        public async Task<CustomResponse<Teacher>> GetTeacherByPh(string ph)
        {
            Teacher teacher = await _teacher.FindAsync(e => e.TeacherPh == ph).Result.FirstOrDefaultAsync();

            if (teacher != null)
            {
                return new CustomResponse<Teacher>(1, teacher, null);
            }else{
                return new CustomResponse<Teacher>(0, null, "Teacher does not exists");
            }

        }

        public async Task<CustomResponse<Teacher>> GetClassTeacher(string std, string section)
        {
            try
            {
                Teacher teacher = await _teacher.FindAsync(e => ((e.isCTRClass == std) && (e.isCTRSection == section) && (e.isCTR))).Result.FirstOrDefaultAsync();

                if (teacher != null)
                {
                    return new CustomResponse<Teacher>(1, teacher, null);
                }
                else
                {
                    return new CustomResponse<Teacher>(0, null, "not added");
                }

            }
            catch (Exception e)
            {
                return new CustomResponse<Teacher>(0, null, e.Message);
            }

        }

        public async Task<CustomResponse<List<Teacher>>> GetTeachers()
        {
            List<Teacher> teachers = await _teacher.FindAsync(e => true).Result.ToListAsync();

            return new CustomResponse<List<Teacher>>(1, teachers, null);
        }

        public async Task<CustomResponse<List<CTSModel>>> GetCTS(string std, string section)
        {
            try
            {
                List<CTSModel> cts = await _cts.FindAsync(e => ((e.Std == std) && (e.Section == section))).Result.ToListAsync();

                return new CustomResponse<List<CTSModel>>(1, cts, null);

            }
            catch (Exception e)
            {
                return new CustomResponse<List<CTSModel>>(0, null, e.Message);
            }

        }

        public async Task<CustomResponse<string>> AddCTS(CustomRequest<List<CTSModel>> ctsList)
        {



            try
            {
                var updateFields = ctsList.Data.Where(e => e.Id != null).ToList();

                var addFields = ctsList.Data.Where(e => e.Id == null).ToList();

                if (addFields.Count > 0)
                {
                    await _cts.InsertManyAsync(addFields);
                }

                foreach (var list in updateFields)
                {
                    if (list.Id != null)
                    {
                        UpdateDefinition<CTSModel> updateDefinition = Builders<CTSModel>.Update.Set(x => x, list);
                        //    .Set(x => x.Std,list.Std)
                        //    .Set(x => x.SubjectId,list.SubjectId)
                        //    .Set(x => x.TID ,list.TID);
                        await _cts.ReplaceOneAsync(x => x.Id == list.Id, list); // replaces first match
                    }
                }

                return new CustomResponse<string>(1, "Inserted Successfully", null);

            }
            catch (Exception e)
            {
                return new CustomResponse<string>(0, null, e.Message);
            }








        }

    }
}
