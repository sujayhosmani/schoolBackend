using jay.school.bussiness.Repository;
using jay.school.contracts.Contracts;
using jay.school.contracts.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
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
        private readonly IMDBContext _teacherMDBContext;
        private readonly IMongoCollection<Student> _student;
        private readonly IMongoCollection<TimeTable> _timeTable;
        private readonly IMongoCollection<SubjectsModel> _subject;
        private readonly IMongoCollection<Teacher> _teacher;
        private readonly IMongoCollection<CTSModel> _cts;
        public SchoolBussiness(IMDBContext schoolMDBContext)
        {
            _schoolMDBContext = schoolMDBContext;
            _teacherMDBContext = schoolMDBContext;
            _student = _schoolMDBContext.GetCollection<Student>(typeof(Student).Name);
            _timeTable = _schoolMDBContext.GetCollection<TimeTable>(typeof(TimeTable).Name);
            _subject = _schoolMDBContext.GetCollection<SubjectsModel>(typeof(SubjectsModel).Name);
            _teacher = _teacherMDBContext.GetCollection<Teacher>(typeof(Teacher).Name);
            _cts = _teacherMDBContext.GetCollection<CTSModel>(typeof(CTSModel).Name);

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
                var updateFields = timeTables.Where(e => e.Id != null).ToList();

                var addFields = timeTables.Where(e => e.Id == null).ToList();

                if (addFields.Count > 0)
                {
                    await _timeTable.InsertManyAsync(addFields);
                }

                foreach (var list in updateFields)
                {
                    if (list.Id != null)
                    {
                        UpdateDefinition<TimeTable> updateDefinition = Builders<TimeTable>.Update.Set(x => x, list);
                        //    .Set(x => x.Std,list.Std)
                        //    .Set(x => x.SubjectId,list.SubjectId)
                        //    .Set(x => x.TID ,list.TID);
                        await _timeTable.ReplaceOneAsync(x => x.Id == list.Id, list); // replaces first match
                    }
                }

                return new CustomResponse<string>(1, "Inserted Successfully", null);

            }
            catch (Exception e)
            {
                return new CustomResponse<string>(0, null, e.Message);
            }

        }
        public async Task<CustomResponse<string>> AddSubject(SubjectsModel subject)
        {
            try
            {
                SubjectsModel found = null;
                try
                {
                    found = await _subject.FindAsync(sub => ((subject.Subject == sub.Subject) || (subject.SubjectCode == sub.SubjectCode))).Result.FirstAsync();

                    return new CustomResponse<string>(0, null, found.Subject + " already exists");

                }
                catch (Exception ex)
                {
                    await _subject.InsertOneAsync(subject);

                    return new CustomResponse<string>(1, "Added " + subject.Subject + " to Records", null);
                }

            }
            catch (Exception e)
            {
                return new CustomResponse<string>(0, null, e.Message);
            }

        }

        public async Task<CustomResponse<string>> DeleteSubject(SubjectsModel subject)
        {
            try
            {
                var found = await _subject.DeleteOneAsync(sub => ((subject.Id == sub.Id)));


                return new CustomResponse<string>(1, subject.Subject + " deleted..", null);

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

            return new CustomResponse<List<Student>>(1, stud, null);
        }
        public async Task<CustomResponse<Student>> GetStudentsById(string id)
        {
            try
            {
                Student stud = await _student.FindAsync(stu => stu.StudentId == id || stu.AdmissionNo == id).Result.FirstAsync();

                return new CustomResponse<Student>(1, stud, null);
            }
            catch (Exception e)
            {
                return new CustomResponse<Student>(0, null, e.Message);
            }

        }
        public async Task<CustomResponse<Student>> GetStudentsByPh(string ph)
        {
            Student stud = await _student.FindAsync(stu => stu.FatherPh == ph || stu.MotherPh == ph).Result.FirstAsync();

            return new CustomResponse<Student>(1, stud, null);
        }
        public async Task<CustomResponse<List<Student>>> GetAllStudents()
        {
            try
            {
                List<Student> stud = await _student.FindAsync(stu => true).Result.ToListAsync();

                return new CustomResponse<List<Student>>(1, stud, null);
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
                if (from.Equals("all"))
                {
                    timeTables = await _timeTable.FindAsync(time => true).Result.ToListAsync();
                }
                else
                {
                    timeTables = await _timeTable.FindAsync(time => (time.Std == std && time.Section == section)).Result.ToListAsync();
                }

                return new CustomResponse<List<TimeTable>>(1, timeTables, null);
            }
            catch (Exception e)
            {
                return new CustomResponse<List<TimeTable>>(0, null, e.Message);
            }

        }

        public async Task<CustomResponse<FullTimeTable>> GetFullTimeTables(bool isTT, bool isCTS, bool isSubject, bool isTeacher, string std, string section)
        {
            try
            {
                List<CTSModel> ctsModel = new List<CTSModel>();
                List<SubjectsModel> subjects = new List<SubjectsModel>();
                List<TimeTable> timeTables = new List<TimeTable>();
                List<Teacher> teachers = new List<Teacher>();
                if (isTT)
                {
                    try
                    {
                        timeTables = await _timeTable.FindAsync(time => (time.Std == std && time.Section == section)).Result.ToListAsync();

                    }
                    catch (Exception e)
                    {

                    }

                }

                if (isCTS)
                {
                    try
                    {
                        ctsModel = await _cts.FindAsync(e => ((e.Std == std) && (e.Section == section))).Result.ToListAsync();
                    }
                    catch (Exception e)
                    {

                    }
                }


                if (isSubject)
                {
                    try
                    {
                        subjects = await _subject.FindAsync(stu => true).Result.ToListAsync();
                    }
                    catch (Exception e)
                    {

                    }
                }

                if (isTeacher)
                {
                    try
                    {

                        teachers = await _teacher.FindAsync(tea => true).Result.ToListAsync();

                    }
                    catch (Exception e)
                    {

                    }
                }

                FullTimeTable tab = new FullTimeTable
                {
                    ctsModel = ctsModel,
                    subjects = subjects,
                    timeTable = timeTables,
                    teacher = teachers
                };

                return new CustomResponse<FullTimeTable>(1, tab, null);
            }
            catch (Exception e)
            {
                return new CustomResponse<FullTimeTable>(0, null, e.ToString());
            }

        }
        public async Task<CustomResponse<List<SubjectsModel>>> GetSubjects()
        {
            try
            {
                List<SubjectsModel> subjects = await _subject.FindAsync(stu => true).Result.ToListAsync();

                return new CustomResponse<List<SubjectsModel>>(1, subjects, null);
            }
            catch (Exception e)
            {
                return new CustomResponse<List<SubjectsModel>>(0, null, e.Message);
            }

        }

        public async Task<CustomResponse<FullTimeTable>> GetTodayClass(string from, bool isCTS, bool isSubject, bool isTeacher, string std, string section)
        {
            try
            {
                CultureInfo culture = new CultureInfo("en-US");

                var today = DateTime.Today.DayOfWeek;

                List<TimeTable> upcomingTimeTable = new List<TimeTable>();

                List<TimeTable> newTimeTable = new List<TimeTable>();

                List<TimeTable> fullTimeTable = await _timeTable.FindAsync(time => (time.Std == std && time.Section == section)).Result.ToListAsync();

                for (int i = 0; i < fullTimeTable.Count; i++)
                {
                    List<WeekSubjects> weekSubjects = new List<WeekSubjects>();

                    weekSubjects = fullTimeTable[i].weekSub.Where(e => (e.Week.ToLower() == today.ToString().ToLower())).ToList();

                    if (weekSubjects.Count > 0)
                    {
                        fullTimeTable[i].weekSub.Clear();

                        fullTimeTable[i].weekSub = weekSubjects;

                        newTimeTable.Add(fullTimeTable[i]);

                        if (from == "up")
                        {
                            DateTime tempDate = Convert.ToDateTime(fullTimeTable[i].EndTime, culture);

                            if (tempDate >= DateTime.Now)
                            {
                                upcomingTimeTable.Add(fullTimeTable[i]);
                            }
                        }
                    }



                }

                FullTimeTable full = new FullTimeTable();

                CustomResponse<FullTimeTable> res = await GetFullTimeTables(false, isCTS, isSubject, isTeacher, std, section);

                full = res.Data;

                if (from == "up")
                {
                    full.timeTable = upcomingTimeTable;
                }
                else
                {
                    full.timeTable = newTimeTable;
                }

                return new CustomResponse<FullTimeTable>(1, full, null);

            }
            catch (Exception e)
            {
                return new CustomResponse<FullTimeTable>(0, null, e.Message);
            }

        }


        public async Task<CustomResponse<List<TimeTable>>> GetTodayTeacherTimeTable(string from, string tid)
        {
            try
            {
                CultureInfo culture = new CultureInfo("en-US");

                var today = DateTime.Today.DayOfWeek;

                List<TimeTable> upcomingTimeTable = new List<TimeTable>();

                List<TimeTable> newTimeTable = new List<TimeTable>();

                List<CTSModel> tidCTS = await _cts.FindAsync(e => e.TID == tid).Result.ToListAsync();

                List<TimeTable> fullTimeTable = await _timeTable.FindAsync(e => true).Result.ToListAsync();

                foreach (var cts in tidCTS)
                {

                    for (int i = 0; i < fullTimeTable.Count; i++)
                    {

                        if (fullTimeTable[i].Std == cts.Std && fullTimeTable[i].Section == cts.Section)
                        {

                            List<WeekSubjects> weekSubjects = new List<WeekSubjects>();

                            weekSubjects = fullTimeTable[i].weekSub.Where(e => (e.Week.ToLower() == today.ToString().ToLower()) && (e.CTSId.ToLower() == cts.Id.ToLower())).Select(f => f).ToList();

                            if (weekSubjects.Count > 0)
                            {

                                fullTimeTable[i].weekSub.Clear();

                                fullTimeTable[i].weekSub = weekSubjects;

                                newTimeTable.Add(fullTimeTable[i]);

                                if (from == "up")
                                {
                                    DateTime tempDate = Convert.ToDateTime(fullTimeTable[i].EndTime, culture);

                                    if (tempDate >= DateTime.Now)
                                    {
                                        upcomingTimeTable.Add(fullTimeTable[i]);
                                    }
                                }
                            }

                        }
                    }
                }


                if (from == "up")
                {
                    return new CustomResponse<List<TimeTable>>(1, upcomingTimeTable, null);
                }
                else
                {
                    return new CustomResponse<List<TimeTable>>(1, newTimeTable, null);
                }



            }
            catch (Exception e)
            {
                return new CustomResponse<List<TimeTable>>(0, null, e.Message);
            }

        }



    }
}
