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
    public class TeachersController : ControllerBase
    {
        private readonly ITeachersService _teachersService;
        public TeachersController(ITeachersService teachersService)
        {
            _teachersService = teachersService;
        }
        
        
        [Route("teachers")]
        [HttpGet]
        public async Task<ActionResult<CustomResponse<List<Teacher>>>> GetTeachers()
        {
            return await _teachersService.GetTeachers();
        }
        
        [Route("teacher/{id}")]
        [HttpGet]
        public async Task<ActionResult<CustomResponse<Teacher>>> GetTeacher(string id)
        {
            return await _teachersService.GetTeacher(id);
        }

        [Route("teacherByPh/{ph}")]
        [HttpGet]
        public async Task<ActionResult<CustomResponse<Teacher>>> GetTeacherByPh(string ph)
        {
             return await _teachersService.GetTeacherByPh(ph);
        }

        [Route("add")]
        [HttpPost]
        public async Task<ActionResult<CustomResponse<string>>> AddTeachers(CustomRequest<Teacher> request)
        {
            
            return await _teachersService.AddTeachers(request);
        }

        [Route("getClassTeacher")]
        [HttpGet]
        public async Task<ActionResult<CustomResponse<Teacher>>> GetClassTeacher(string std, string section)
        {
            return await _teachersService.GetClassTeacher(std,section);
        }

    }
}
