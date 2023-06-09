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
    public class TimeTableBusiness : ITimeTableService
    {

        private readonly IMongoCollection<TimeTable> _timeTable;
        private readonly IMongoCollection<SubjectsModel> _subject;
        private readonly IMongoCollection<CTSModel> _cts;
        private readonly IMongoCollection<OnlineClass> _onlineClass;
        private readonly IMongoCollection<Attendance> _attendance;
        private readonly IMongoCollection<Teacher> _teacher;
        private readonly IMDBContext _timeTableMDBContext;

        public TimeTableBusiness(IMDBContext timeTableMDNContext)
        {
            _timeTableMDBContext = timeTableMDNContext;

            _timeTable = _timeTableMDBContext.GetCollection<TimeTable>(typeof(TimeTable).Name);

            _subject = _timeTableMDBContext.GetCollection<SubjectsModel>(typeof(SubjectsModel).Name);

            _teacher = _timeTableMDBContext.GetCollection<Teacher>(typeof(Teacher).Name);

            _cts = _timeTableMDBContext.GetCollection<CTSModel>(typeof(CTSModel).Name);

            _onlineClass = _timeTableMDBContext.GetCollection<OnlineClass>(typeof(OnlineClass).Name);

            _attendance = _timeTableMDBContext.GetCollection<Attendance>(typeof(Attendance).Name);

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

        public async Task<CustomResponse<List<CTSModel>>> GetCTSByTid(string tid)
        {
            try
            {
                List<CTSModel> cts = await _cts.FindAsync(e => ((e.TID == tid))).Result.ToListAsync();

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

                    List<CTSModel> tidCts = await _cts.FindAsync(e => ((e.Std == std) && (e.Section == section))).Result.ToListAsync();

                    for (int i = 0; i < timeTables.Count; i++)
                    {
                        for (int w = 0; w < timeTables[i].weekSub.Count; w++)
                        {
                            CTSModel cts = tidCts.Where(e => e.Id == timeTables[i].weekSub[w].CTSId).FirstOrDefault();
                            timeTables[i].weekSub[w].TId = cts?.TID ?? "";
                            timeTables[i].weekSub[w].SubjectId = cts?.SubjectId ?? "";
                        }
                    }
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

                            // weekSubjects = fullTimeTable[i].weekSub.Where(e => (e.Week.ToLower() == today.ToString().ToLower()) && (e.CTSId.ToLower() == cts.Id.ToLower())).Select(
                            //     f => new WeekSubjects
                            //     {
                            //         CTSId = f.CTSId,
                            //         SubjectId = cts.SubjectId,
                            //         TId = cts.TID,
                            //         Week = f.Week
                            //     }
                            //     ).ToList();


                            for (int w = 0; w < fullTimeTable[i].weekSub.Count; w++)
                            {
                                if (fullTimeTable[i].weekSub[w].Week == today.ToString() && fullTimeTable[i].weekSub[w].CTSId == cts.Id)
                                {
                                    fullTimeTable[i].weekSub[w].TId = cts.TID;
                                    fullTimeTable[i].weekSub[w].SubjectId = cts.SubjectId;

                                    var todayDate = DateTime.Today.ToString("dd/MM/yyyy");

                                    var CurrentTime = DateTime.Now;

                                    var StartTime = Convert.ToDateTime(fullTimeTable[i].FromTime, culture);
                                    var EndTime = Convert.ToDateTime(fullTimeTable[i].EndTime, culture);

                                    var uniqId = todayDate + fullTimeTable[i].Std + fullTimeTable[i].Section + fullTimeTable[i].FromTime
                                    + fullTimeTable[i].EndTime + fullTimeTable[i].weekSub[w].CTSId + fullTimeTable[i].weekSub[w].Week;

                                    CustomResponse<OnlineClass> oc = await getOnlineClassByUniqId(uniqId);


                                    if (CurrentTime < StartTime)
                                    {
                                        fullTimeTable[i].weekSub[w].Status = "Waiting";
                                        fullTimeTable[i].weekSub[w].StatusCode = 3;
                                    }
                                    else if (CurrentTime >= StartTime && CurrentTime <= EndTime)
                                    {
                                        if (oc.Status == 1)
                                        {
                                            fullTimeTable[i].weekSub[w].Status = "Resume";
                                            fullTimeTable[i].weekSub[w].StatusCode = 1;
                                            fullTimeTable[i].weekSub[w].OnlineClassId = oc.Data.Id;
                                        }
                                        else
                                        {
                                            fullTimeTable[i].weekSub[w].Status = "Start";
                                            fullTimeTable[i].weekSub[w].StatusCode = 2;
                                        }
                                    }
                                    else
                                    {
                                        if (oc.Status == 1)
                                        {
                                            if (oc.Data.Status == "Started")
                                            {
                                                fullTimeTable[i].weekSub[w].Status = "NA Ended";
                                            }
                                            else
                                            {
                                                fullTimeTable[i].weekSub[w].Status = "Ended";
                                            }

                                            fullTimeTable[i].weekSub[w].StatusCode = 5;
                                            fullTimeTable[i].weekSub[w].OnlineClassId = oc.Data.Id;
                                        }
                                        else
                                        {
                                            fullTimeTable[i].weekSub[w].Status = "Skipped";
                                            fullTimeTable[i].weekSub[w].StatusCode = 4;
                                        }
                                    }

                                    weekSubjects.Add(fullTimeTable[i].weekSub[w]);

                                }
                            }

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

        public async Task<CustomResponse<List<TimeTable>>> GetTodayClassStudent(string from, string std, string section, string StudentId)
        {
            try
            {
                CultureInfo culture = new CultureInfo("en-US");

                var today = DateTime.Today.DayOfWeek;

                List<TimeTable> upcomingTimeTable = new List<TimeTable>();

                List<TimeTable> newTimeTable = new List<TimeTable>();

                List<CTSModel> tidCts = await _cts.FindAsync(e => ((e.Std == std) && (e.Section == section))).Result.ToListAsync();

                List<TimeTable> fullTimeTable = await _timeTable.FindAsync(time => (time.Std == std && time.Section == section)).Result.ToListAsync();

                for (int i = 0; i < fullTimeTable.Count; i++)
                {
                    List<WeekSubjects> weekSubjects = new List<WeekSubjects>();

                    for (int w = 0; w < fullTimeTable[i].weekSub.Count; w++)
                    {
                        if (fullTimeTable[i].weekSub[w].Week == today.ToString())
                        {
                            CTSModel cts = tidCts.Where(e => e.Id == fullTimeTable[i].weekSub[w].CTSId).FirstOrDefault();
                            fullTimeTable[i].weekSub[w].TId = cts?.TID ?? "";
                            fullTimeTable[i].weekSub[w].SubjectId = cts?.SubjectId ?? "";

                            var todayDate = DateTime.Today.ToString("dd/MM/yyyy");

                            var CurrentTime = DateTime.Now;

                            var StartTime = Convert.ToDateTime(fullTimeTable[i].FromTime, culture);
                            var EndTime = Convert.ToDateTime(fullTimeTable[i].EndTime, culture);

                            var uniqId = todayDate + fullTimeTable[i].Std + fullTimeTable[i].Section + fullTimeTable[i].FromTime
                            + fullTimeTable[i].EndTime + fullTimeTable[i].weekSub[w].CTSId + fullTimeTable[i].weekSub[w].Week;

                            CustomResponse<OnlineClass> oc = await getOnlineClassByUniqId(uniqId);
                            Attendance atten = null;
                            if (oc.Status == 1)
                            {
                                CustomResponse<Attendance> at = await getAttendanceByOnlineId(oc.Data.Id, StudentId);
                                if (at.Status == 1)
                                {
                                    atten = at.Data;
                                }
                            }


                            if (CurrentTime < StartTime)
                            {
                                fullTimeTable[i].weekSub[w].Status = "Waiting";
                                fullTimeTable[i].weekSub[w].StatusCode = 3;
                            }
                            else if (CurrentTime >= StartTime && CurrentTime <= EndTime)
                            {
                                if (oc.Status == 1)
                                {
                                    if (atten != null)
                                    {
                                        fullTimeTable[i].weekSub[w].Status = "Resume"; // resume
                                    }
                                    else
                                    {
                                        fullTimeTable[i].weekSub[w].Status = "Join"; // resume
                                    }

                                    fullTimeTable[i].weekSub[w].StatusCode = 1;
                                    fullTimeTable[i].weekSub[w].OnlineClassId = oc.Data.Id;
                                }
                                else
                                {
                                    fullTimeTable[i].weekSub[w].Status = "Not started yet"; // start
                                    fullTimeTable[i].weekSub[w].StatusCode = 2;
                                }
                            }
                            else
                            {
                                if (oc.Status == 1)
                                {
                                    if (oc.Data.Status == "Started")
                                    {

                                        if (atten != null)
                                        {
                                            fullTimeTable[i].weekSub[w].Status = "NA Attended"; // resume
                                        }
                                        else
                                        {
                                            fullTimeTable[i].weekSub[w].Status = "NA Absent"; // resume
                                        }
                                    }
                                    else
                                    {
                                        if (atten != null)
                                        {
                                            fullTimeTable[i].weekSub[w].Status = "Attended"; // resume
                                        }
                                        else
                                        {
                                            fullTimeTable[i].weekSub[w].Status = "Absent"; // resume
                                        }
                                    }

                                    fullTimeTable[i].weekSub[w].StatusCode = 5;
                                    fullTimeTable[i].weekSub[w].OnlineClassId = oc.Data.Id;
                                }
                                else
                                {
                                    fullTimeTable[i].weekSub[w].Status = "Skipped by Teacher"; // skipped
                                    fullTimeTable[i].weekSub[w].StatusCode = 4;
                                }
                            }

                            weekSubjects.Add(fullTimeTable[i].weekSub[w]);

                        }
                    }


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
                return new CustomResponse<List<TimeTable>>(0, null, e.ToString());
            }

        }

        public async Task<CustomResponse<OnlineClass>> getOnlineClassByUniqId(string uniqId)
        {

            try
            {
                var onlineClss = await _onlineClass.FindAsync(e => e.UniqueId == uniqId).Result.FirstAsync();

                if (onlineClss != null)
                {
                    return new CustomResponse<OnlineClass>(1, onlineClss, null);
                }
                else
                {
                    return new CustomResponse<OnlineClass>(0, null, "no data");
                }

            }
            catch (Exception e)
            {
                return new CustomResponse<OnlineClass>(0, null, e.Message);
            }

        }

        public async Task<CustomResponse<Attendance>> getAttendanceByOnlineId(string onlineId, string studentId)
        {

            try
            {
                var attendance = await _attendance.FindAsync(e => e.OnlineClassId == onlineId && e.StudentId == studentId).Result.FirstAsync();

                if (attendance != null)
                {
                    return new CustomResponse<Attendance>(1, attendance, null);
                }
                else
                {
                    return new CustomResponse<Attendance>(0, null, "no data");
                }

            }
            catch (Exception e)
            {
                return new CustomResponse<Attendance>(0, null, e.Message);
            }

        }

        public async Task<CustomResponse<OnlineClass>> AddOnlineClass(OnlineClass onlineClass)
        {
            if (onlineClass.Id == null)
            {

                var todayDate = DateTime.Today.ToString("dd/MM/yyyy");

                var uniqId = todayDate + onlineClass.Std + onlineClass.Section + onlineClass.ActualStartTime
                                        + onlineClass.ActualEndTime + onlineClass.CTSId + onlineClass.Week;


                CultureInfo culture = new CultureInfo("en-US");
                var StartTime = Convert.ToDateTime(onlineClass.ActualStartTime, culture);
                var EndTime = Convert.ToDateTime(onlineClass.ActualEndTime, culture);
                if (DateTime.Now >= StartTime && DateTime.Now <= EndTime)
                {
                    var found = await _onlineClass.FindAsync(e => e.UniqueId == uniqId && e.CurrentDate == todayDate).Result.ToListAsync();
                    if (found.Count > 0)
                    {
                        return new CustomResponse<OnlineClass>(1, found[0], null);
                    }
                    else
                    {
                        onlineClass.UniqueId = uniqId;
                        onlineClass.CurrentDate = todayDate;
                        onlineClass.Status = "Started";
                        onlineClass.StartTime = DateTime.Now.ToString();

                        await _onlineClass.InsertOneAsync(onlineClass);

                        return new CustomResponse<OnlineClass>(1, onlineClass, null);
                    }

                }
                else
                {
                    return new CustomResponse<OnlineClass>(0, null, "Time Exceeded");
                }

            }
            else
            {
                return new CustomResponse<OnlineClass>(0, null, "Id Exists");
            }


        }

        public async Task<CustomResponse<Attendance>> AddAttendance(Attendance attendance)
        {
            if (attendance.Id == null)
            {
                var todayDate = DateTime.Today.ToString("dd/MM/yyyy");
                CultureInfo culture = new CultureInfo("en-US");
                var StartTime = Convert.ToDateTime(attendance.ActualStartTime, culture);
                var EndTime = Convert.ToDateTime(attendance.ActualEndTime, culture);
                if (DateTime.Now >= StartTime && DateTime.Now <= EndTime)
                {
                    var found = await _attendance.FindAsync(e => e.OnlineClassId == attendance.OnlineClassId && e.StudentId == attendance.StudentId).Result.ToListAsync();
                    if (found.Count > 0)
                    {
                        return new CustomResponse<Attendance>(1, found[0], null);
                    }
                    else
                    {
                        attendance.CurrentDate = todayDate;
                        attendance.StartTime = DateTime.Now.ToString();

                        await _attendance.InsertOneAsync(attendance);

                        return new CustomResponse<Attendance>(1, attendance, null);
                    }

                }
                else
                {
                    return new CustomResponse<Attendance>(0, null, "Time Exceeded");
                }

            }
            else
            {
                return new CustomResponse<Attendance>(0, null, "Id Exists");
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


        public async Task<CustomResponse<List<OverAll>>> GetAttendance(string std, string sec, string sid){
            
            List<string> SubjectList = _onlineClass.AsQueryable().Where(s => (s.Std == std) && (s.Section == sec)).Select(e => e.SubjectId).Distinct().ToList();
            
            List<OverAll> overAlls = new List<OverAll>();

            for(int i = 0; i < SubjectList.Count; i++){
                long totalClassTaken = await _onlineClass.CountDocumentsAsync(e => ((e.Std == std) && (e.Section == sec) && (e.SubjectId == SubjectList[i])));
                long totalClassAttended = await _attendance.CountDocumentsAsync(a => ((a.Std == std) && (a.Section == sec) && (a.SubjectId == SubjectList[i]) && (a.StudentId == sid)));
                long totalAbsent = totalClassTaken - totalClassAttended;
                string subjectId = SubjectList[i];
                SubjectsModel subjectsModel = await _subject.FindAsync(s => subjectId == s.Id).Result.FirstAsync();
                string subjectName = "";
                if(subjectsModel != null){
                    subjectName = subjectsModel.Subject;
                }
                OverAll overAll = new OverAll{
                    Absent = totalAbsent,
                    Present = totalClassAttended,
                    Subject = subjectName,
                    SubjectId = subjectId,
                    Total = totalClassTaken
                };
                overAlls.Add(overAll);
            }

            long overAllClassTaken = await _onlineClass.CountDocumentsAsync(e => ((e.Std == std) && (e.Section == sec)));
            long overAllClassAttended = await _attendance.CountDocumentsAsync(a => ((a.Std == std) && (a.Section == sec) && (a.StudentId == sid)));
            long overAllAbsent = overAllClassTaken - overAllClassAttended;
            OverAll overAll2 = new OverAll{
                    Absent = overAllAbsent,
                    Present = overAllClassAttended,
                    Subject = "all",
                    SubjectId = "all",
                    Total = overAllClassTaken
                };
                overAlls.Add(overAll2);
            

            return new CustomResponse<List<OverAll>>(1, overAlls, null);
        }




    }


}