﻿using jay.school.contracts.Contracts;
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


    
    }
}
