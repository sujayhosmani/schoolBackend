using jay.school.contracts.Contracts;
using jay.school.contracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jay.school.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeTableController : ControllerBase
    {
        private readonly ITimeTableService _timeTableSevice;

        public TimeTableController(ITimeTableService timeTableService)
        {
            _timeTableSevice = timeTableService;
        }


        [Route("getCTS")]
        [HttpGet]
        public async Task<ActionResult<CustomResponse<List<CTSModel>>>> GetCTS(string std, string section)
        {
            return await _timeTableSevice.GetCTS(std,section);
        }

        [Route("getCTSById/{tid}")]
        [HttpGet]
        public async Task<ActionResult<CustomResponse<List<CTSModel>>>> GetCTS(string tid)
        {
            return await _timeTableSevice.GetCTSByTid(tid);
        }

        [Route("addCTS")]
        [HttpPost]
        public async Task<ActionResult<CustomResponse<string>>> AddCTS(CustomRequest<List<CTSModel>> ctsList)
        {
            return await _timeTableSevice.AddCTS(ctsList);
        }

        [Route("addOnlineClass")]
        [HttpPost]
        public async Task<CustomResponse<OnlineClass>> AddOnlineClass(CustomRequest<OnlineClass> onlineClass){
            
            return await _timeTableSevice.AddOnlineClass(onlineClass.Data);

        }


        [Route("addAttendance")]
        [HttpPost]
        public async Task<CustomResponse<Attendance>> AddAttendance(CustomRequest<Attendance> attendance){
            
            return await _timeTableSevice.AddAttendance(attendance.Data);

        }

        [Route("addTimeTable")]
        [HttpPost]
        public async Task<CustomResponse<string>> AddTimeTables(CustomRequest<List<TimeTable>> timeTable)
        {

            return await _timeTableSevice.AddTimeTables(timeTable.Data);
        }

        [Route("addSubject")]
        [HttpPost]
        public async Task<CustomResponse<string>> AddSubject(CustomRequest<SubjectsModel> subject)
        {
            
            return await _timeTableSevice.AddSubject(subject.Data);
        }

        [Route("getTimeTable")]
        [HttpGet]
        public async Task<CustomResponse<List<TimeTable>>> GetTimeTables(string from, string std, string section)
        {
            return await _timeTableSevice.GetTimeTables(from, std, section);
            
        }

        [Route("getSubjects")]
        [HttpGet]
        public async Task<CustomResponse<List<SubjectsModel>>> GetSubjects()
        {
            return await _timeTableSevice.GetSubjects();
            
        }

        [Route("delSubject")]
        [HttpPost]
        public async Task<CustomResponse<string>> DeleteSubject(CustomRequest<SubjectsModel> subject)
        {
            
            return await _timeTableSevice.DeleteSubject(subject.Data);
        }

        [Route("getFullTimeTable")]
        [HttpGet]
        public async Task<CustomResponse<FullTimeTable>> GetFullTimeTables(bool isTT, bool isCTS, bool isSubject, bool isTeacher, string std, string section)
        {
            return await _timeTableSevice.GetFullTimeTables(isTT, isCTS, isSubject, isTeacher, std, section);
            
        }

        [Route("getTodayClass")]
        [HttpGet]
        public async Task<CustomResponse<FullTimeTable>> GetTodayClass(string from, bool isCTS, bool isSubject, bool isTeacher, string std, string section)
        {
            return await _timeTableSevice.GetTodayClass(from, isCTS, isSubject, isTeacher, std, section);
            
        }

        [Route("getTodayTeacherTimeTable")]
        [HttpGet]
        public async Task<CustomResponse<List<TimeTable>>> GetTodayTeacherTimeTable(string from, string tid){
            
            return await _timeTableSevice.GetTodayTeacherTimeTable(from, tid);
        
        }

        [Route("GetTodayClassStudent")]
        [HttpGet]
        public async Task<CustomResponse<List<TimeTable>>> GetTodayClassStudent(string from, string std, string section, string sid){
            
            return await _timeTableSevice.GetTodayClassStudent(from, std, section, sid);
        }
    


    }

}