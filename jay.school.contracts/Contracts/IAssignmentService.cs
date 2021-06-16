using jay.school.contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jay.school.contracts.Contracts
{
    public interface IAssignmentService
    {
        Task<CustomResponse<Assignment>> AddAssignment(Assignment assignment);
        Task<CustomResponse<SubmitAssignments>> SubmitAssignment(SubmitAssignments submitAssignment);
        Task<CustomResponse<List<Assignment>>> GetAssignmentsByTid(string tid);
        Task<CustomResponse<SubmitAssignments>> GetSubmittedAssignment(string sid, string assigId);
        Task<CustomResponse<List<Assignment>>> GetAssignmentsByClass(string std, string section, string sid);

    }
}
