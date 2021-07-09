using jay.school.contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jay.school.contracts.Contracts
{
    public interface ITimeTableService
    {
        Task<CustomResponse<List<CTSModel>>> GetCTS(string std, string section);

        Task<CustomResponse<List<CTSModel>>> GetCTSByTid(string tid);

        Task<CustomResponse<string>> AddCTS(CustomRequest<List<CTSModel>> ctsList);

        Task<CustomResponse<List<TimeTable>>> GetTodayTeacherTimeTable(string from, string tid);
        Task<CustomResponse<List<TimeTable>>> GetTodayClassStudent(string from, string std, string section, string StudentId);
        Task<CustomResponse<List<TimeTable>>> GetTimeTables(string from, string std, string section);
        Task<CustomResponse<FullTimeTable>> GetFullTimeTables(bool isTT, bool isCTS, bool isSubject, bool isTeacher, string std, string section);
        Task<CustomResponse<FullTimeTable>> GetTodayClass(string from, bool isCTS, bool isSubject, bool isTeacher, string std, string section);
        Task<CustomResponse<OnlineClass>> AddOnlineClass(OnlineClass onlineClass);
        Task<CustomResponse<Attendance>> AddAttendance(Attendance attendance);
        Task<CustomResponse<string>> DeleteSubject(SubjectsModel subject);
        Task<CustomResponse<List<SubjectsModel>>> GetSubjects();

        Task<CustomResponse<string>> AddTimeTables(List<TimeTable> timeTables);
        Task<CustomResponse<string>> AddSubject(SubjectsModel subject);
        CustomResponse<List<string>> GetAttendance(string std, string sec, string sid);

    }
}
