using jay.school.bussiness.Repository;
using jay.school.contracts.Contracts;
using jay.school.contracts.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace jay.school.bussiness.Bussiness
{
    public class SchoolBussiness : ISchoolService
    {
        //
        private readonly IMDBContext _schoolMDBContext;
        private readonly IMongoCollection<Student> _student;

        public SchoolBussiness(IMDBContext schoolMDBContext)
        {
            _schoolMDBContext = schoolMDBContext;

            
            _student = _schoolMDBContext.GetCollection<Student>(typeof(Student).Name);
                 

        }
        public async Task<CustomResponse<Student>> AddStudent(Student student)
        {
            await _student.InsertOneAsync(student);

            return new CustomResponse<Student>(1, student, null);

        }
        public async Task<CustomResponse<string>> AddStudents(List<Student> students)
        {
            try
            {
                await _student.InsertManyAsync(students);

                return new CustomResponse<string>(1, "Added " + students.Count.ToString() + " Records", null);
            }
            catch (Exception e)
            {
                return new CustomResponse<string>(0, null, e.Message);
            }

        }

        private CustomResponse<T> CheckForNull<T>(T data)
        {
            
            if (data == null)
            {
                return new CustomResponse<T>(0, data, "No data found");
            }
            else
            {
                return new CustomResponse<T>(1, data, null);
            }
        }

        public async Task<CustomResponse<List<Student>>> GetStudentsByClass(string cls, string sec)
        {
            //TODO: add pagination later
            List<Student> stud = await _student.FindAsync(stu => stu.Class == cls && stu.Section == sec).Result.ToListAsync();

            if(stud.Count > 0)
            {
                return new CustomResponse<List<Student>>(1, stud, null);
            }

            return new CustomResponse<List<Student>>(0, null, "No data found");
        }
        public async Task<CustomResponse<Student>> GetStudentsById(string id)
        {
            try
            {
                Student stud = await _student.FindAsync(stu => stu.StudentId == id || stu.AdmissionNo == id).Result.FirstOrDefaultAsync();

                return CheckForNull<Student>(stud);

            }
            catch (Exception e)
            {
                return new CustomResponse<Student>(0, null, e.Message);
            }

        }

        public async Task<CustomResponse<Student>> GetStudentsByPh(string ph)
        {
            Student stud = await _student.FindAsync(stu => stu.FatherPh == ph || stu.MotherPh == ph).Result.FirstOrDefaultAsync();

            return CheckForNull<Student>(stud);
        }

        public async Task<CustomResponse<List<Student>>> GetAllStudents()
        {
            try
            {
                List<Student> stud = await _student.FindAsync(stu => true).Result.ToListAsync();

                if (stud.Count > 0)
                {
                    return new CustomResponse<List<Student>>(1, stud, null);
                }

                return new CustomResponse<List<Student>>(0, null, "No data found");
            }
            catch (Exception e)
            {
                return new CustomResponse<List<Student>>(0, null, e.Message);
            }
            //TODO: add pagination later

        }
        



    }
}
