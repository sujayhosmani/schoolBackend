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
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }


        [Route("getByPh/{ph}")]
        [HttpGet]
        public async Task<CustomResponse<Student>> GetStudentsByPh(string ph)
        {
          
            return await _schoolService.GetStudentsByPh(ph);

        }

        [Route("getById/{id}")]
        [HttpGet]
        public async Task<CustomResponse<Student>> GetStudentsById(string id)
        {
//
            return await _schoolService.GetStudentsById(id);

        }

        [Route("getByClass/{cls}/{sec}")]
        [HttpGet]
        public async Task<CustomResponse<List<Student>>> GetStudentsByClass(string cls, string sec)
        {

            return await _schoolService.GetStudentsByClass(cls, sec);

        }

        [Route("getAllStudents")]
        [HttpGet]
        public async Task<CustomResponse<List<Student>>> GetAllStudents(string cls, string sec)
        {

            return await _schoolService.GetAllStudents();

        }

        [Route("add")]
        [HttpPost]
        public async Task<CustomResponse<Student>> AddStudent(CustomRequest<Student> student)
        {

            return await _schoolService.AddStudent(student.Data);

        }

        [Route("bulkAdd")]
        [HttpPost]
        public async Task<CustomResponse<string>> AddStudents(CustomRequest<List<Student>> students)
        {
            return await _schoolService.AddStudents(students.Data);
        }


        [Route("addTimeTable")]
        [HttpPost]
        public async Task<CustomResponse<string>> AddTimeTables(CustomRequest<List<TimeTable>> timeTable)
        {

            return await _schoolService.AddTimeTables(timeTable.Data);
        }

        [Route("addSubject")]
        [HttpPost]
        public async Task<CustomResponse<string>> AddSubject(CustomRequest<SubjectsModel> subject)
        {
            
            return await _schoolService.AddSubject(subject.Data);
        }

        [Route("getTimeTable")]
        [HttpGet]
        public async Task<CustomResponse<List<TimeTable>>> GetTimeTables(string from, string std, string section)
        {
            return await _schoolService.GetTimeTables(from, std, section);
            
        }

        [Route("getSubjects")]
        [HttpGet]
        public async Task<CustomResponse<List<SubjectsModel>>> GetSubjects()
        {
            return await _schoolService.GetSubjects();
            
        }

        [Route("delSubject")]
        [HttpPost]
        public async Task<CustomResponse<string>> DeleteSubject(CustomRequest<SubjectsModel> subject)
        {
            
            return await _schoolService.DeleteSubject(subject.Data);
        }

        [Route("getFullTimeTable")]
        [HttpGet]
        public async Task<CustomResponse<FullTimeTable>> GetFullTimeTables(bool isTT, bool isCTS, bool isSubject, bool isTeacher, string std, string section)
        {
            return await _schoolService.GetFullTimeTables(isTT, isCTS, isSubject, isTeacher, std, section);
            
        }

        [Route("getTodayClass")]
        [HttpGet]
        public async Task<CustomResponse<FullTimeTable>> GetTodayClass(string from, bool isCTS, bool isSubject, bool isTeacher, string std, string section)
        {
            return await _schoolService.GetTodayClass(from, isCTS, isSubject, isTeacher, std, section);
            
        }

        [Route("getTodayTeacherTimeTable")]
        [HttpGet]
        public async Task<CustomResponse<List<TimeTable>>> GetTodayTeacherTimeTable(string from, string tid){
            
            return await _schoolService.GetTodayTeacherTimeTable(from, tid);
        
        }

    }
}
