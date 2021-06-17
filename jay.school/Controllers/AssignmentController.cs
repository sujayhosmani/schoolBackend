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
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [Route("addAssignment")]
        [HttpPost]
        public async Task<CustomResponse<Assignment>> AddAssignment(CustomRequest<Assignment> assignment){

            return await _assignmentService.AddAssignment(assignment.Data);
        }

        [Route("submitAssignment")]
        [HttpPost]
        public async Task<CustomResponse<SubmitAssignments>> SubmitAssignment(CustomRequest<SubmitAssignments> submitAssignment){

            return await _assignmentService.SubmitAssignment(submitAssignment.Data);
        }
        
        [Route("getAssignmentsByTid/{tid}")]
        [HttpGet]
        public async Task<CustomResponse<List<Assignment>>> GetAssignmentsByTid(string tid){

            return await _assignmentService.GetAssignmentsByTid(tid);
        }

        [Route("getSubmittedAssignment")]
        [HttpGet]
        public async Task<CustomResponse<SubmitAssignments>> GetSubmittedAssignment(string sid, string assigId){
           
            return await _assignmentService.GetSubmittedAssignment(sid,assigId);

        }

        [Route("getAllSubmittedAssignment")]
        [HttpGet]
        public async Task<CustomResponse<List<SubmitAssignments>>> GetAllSubmittedAssignment(string assigId){
           
            return await _assignmentService.GetSubmittedAssignment(assigId);

        }

        [Route("getAssignmentsByClass")]
        [HttpGet]
        public async Task<CustomResponse<List<Assignment>>> GetAssignmentsByClass(string std, string section, string sid){

            return await _assignmentService.GetAssignmentsByClass(std, section, sid);
        }


    }

}