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
        private readonly IMongoCollection<TimeTable> _timeTable;
        private readonly IMongoCollection<SubjectsModel> _subject;
        public SchoolBussiness(IMDBContext schoolMDBContext)
        {
            _schoolMDBContext = schoolMDBContext;
            _student = _schoolMDBContext.GetCollection<Student>(typeof(Student).Name);
            _timeTable = _schoolMDBContext.GetCollection<TimeTable>(typeof(TimeTable).Name);
            _subject = _schoolMDBContext.GetCollection<SubjectsModel>(typeof(SubjectsModel).Name);

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
        public async Task<CustomResponse<string>> AddTimeTables(List<TimeTable> timeTables)
        {
            try
            {
                await _timeTable.InsertManyAsync(timeTables);

                return new CustomResponse<string>(1, "Added " + timeTables.ToString() + " Records", null);
            }
            catch (Exception e)
            {
                return new CustomResponse<string>(0, null, e.Message);
            }

        }
        public async Task<CustomResponse<string>> AddSubjects(List<SubjectsModel> subjects)
        {
            try
            {
                await _subject.InsertManyAsync(subjects);

                return new CustomResponse<string>(1, "Added " + subjects.ToString() + " Records", null);
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
            try
            {
                Student stud = await _student.FindAsync(stu => stu.StudentId == id || stu.AdmissionNo == id).Result.FirstAsync();

                return new CustomResponse<Student>(0, stud, null);
            }
            catch (Exception e)
            {
                return new CustomResponse<Student>(0, null, e.Message);
            }

        }
        public async Task<CustomResponse<Student>> GetStudentsByPh(string ph)
        {
            Student stud = await _student.FindAsync(stu => stu.FatherPh == ph || stu.MotherPh == ph).Result.FirstAsync();

            return new CustomResponse<Student>(0, stud, null);
        }
        public async Task<CustomResponse<List<Student>>> GetAllStudents()
        {
            try
            {
                List<Student> stud = await _student.FindAsync(stu => true).Result.ToListAsync();

                return new CustomResponse<List<Student>>(0, stud, null);
            }
            catch (Exception e)
            {
                return new CustomResponse<List<Student>>(0, null, e.Message);
            }
            //TODO: add pagination later

        }
        public async Task<CustomResponse<List<TimeTable>>> GetTimeTables(string from, string std, string section)
        {
            try
            {
                List<TimeTable> timeTables;
                if(from.Equals("all")){
                   timeTables = await _timeTable.FindAsync(time => true).Result.ToListAsync();
                }else{
                    timeTables = await _timeTable.FindAsync(time => (time.Std == std && time.Section == section)).Result.ToListAsync();
                }

                return new CustomResponse<List<TimeTable>>(0, timeTables, null);
            }
            catch (Exception e)
            {
                return new CustomResponse<List<TimeTable>>(0, null, e.Message);
            }

        }
        public async Task<CustomResponse<List<SubjectsModel>>> GetSubjects()
        {
            try
            {
                List<SubjectsModel> subjects = await _subject.FindAsync(stu => true).Result.ToListAsync();

                return new CustomResponse<List<SubjectsModel>>(0, subjects, null);
            }
            catch (Exception e)
            {
                return new CustomResponse<List<SubjectsModel>>(0, null, e.Message);
            }

        }
    }
}
