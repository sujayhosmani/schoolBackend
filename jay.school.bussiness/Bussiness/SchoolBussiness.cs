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

        public async Task<CustomResponse<List<Student>>> GetStudentsByClass(string cls, string sec)
        {
            //TODO: add pagination later
            List<Student> stud = await _student.FindAsync(stu => stu.Class == cls && stu.Section == sec).Result.ToListAsync();

            return new CustomResponse<List<Student>>(0, stud, null);
        }

        public async Task<CustomResponse<Student>> GetStudentsById(string id)
        {
            try {
                Student stud = await _student.FindAsync(stu => stu.StudentId == id || stu.AdmissionNo == id).Result.FirstAsync();

                return new CustomResponse<Student>(0, stud, null);
            }
            catch(Exception e) {
                return new CustomResponse<Student>(0, null, "modifieddd " + e.Message);
            }
            
        }

        public async Task<CustomResponse<Student>> GetStudentsByPh(string ph)
        {
            Student stud = await _student.FindAsync(stu => stu.FatherPh == ph || stu.MotherPh == ph).Result.FirstAsync();

            return new CustomResponse<Student>(0, stud, null);
        }

        public async Task<CustomResponse<List<Student>>> GetAllStudents()
        {
            //TODO: add pagination later
            List<Student> stud = await _student.FindAsync(stu => true).Result.ToListAsync();

            return new CustomResponse<List<Student>>(0, stud, null);
        }

    }
}
                                                            