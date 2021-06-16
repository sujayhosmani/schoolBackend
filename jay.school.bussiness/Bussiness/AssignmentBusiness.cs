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
    public class AssignmentBusiness : IAssignmentService
    {

        private readonly IMongoCollection<Assignment> _assignment;
        private readonly IMongoCollection<SubmitAssignments> _submittedAssignment;
        private readonly IMDBContext _assignmentMDBContext;

        public AssignmentBusiness(IMDBContext assignmentMDBContext)
        {
            _assignmentMDBContext = assignmentMDBContext;

            _assignment = _assignmentMDBContext.GetCollection<Assignment>(typeof(Assignment).Name);

            _submittedAssignment = _assignmentMDBContext.GetCollection<SubmitAssignments>(typeof(SubmitAssignments).Name);

        }

        public async Task<CustomResponse<Assignment>> AddAssignment(Assignment assignment)
        {
            if (assignment.Id == null)
            {

                var todayDate = DateTime.Today.ToString("dd/MM/yyyy");

                assignment.StartDate = todayDate;

                assignment.EndDate = DateTime.Today.AddDays(int.Parse(assignment.EndDate.Trim() ?? "0")).ToString("dd/MM//yyyy");

                await _assignment.InsertOneAsync(assignment);

                return new CustomResponse<Assignment>(1, assignment, null);

            }
            else
            {
                return new CustomResponse<Assignment>(0, null, "Id Exists");
            }


        }

        public async Task<CustomResponse<List<Assignment>>> GetAssignmentsByTid(string tid)
        {
            try
            {

                List<Assignment> assignments = await _assignment.FindAsync(e => e.Tid == tid).Result.ToListAsync();

                return new CustomResponse<List<Assignment>>(1, assignments, null);

            }
            catch (Exception e)
            {

                return new CustomResponse<List<Assignment>>(0, null, e.ToString());

            }

        }

        public async Task<CustomResponse<SubmitAssignments>> GetSubmittedAssignment(string sid, string assigId)
        {
            try
            {

                SubmitAssignments sa = await _submittedAssignment.FindAsync(e => ((e.Sid == sid) && (e.AssignmentId == assigId))).Result.FirstAsync();

                return new CustomResponse<SubmitAssignments>(1, sa, null);

            }
            catch (Exception e)
            {

                return new CustomResponse<SubmitAssignments>(0, null, e.ToString());

            }

        }

        public async Task<CustomResponse<List<Assignment>>> GetAssignmentsByClass(string std, string section, string sid)
        {
            try
            {

                List<Assignment> assignments = await _assignment.FindAsync(e => ((e.Std == std) && (e.Section == section))).Result.ToListAsync();

                foreach (var assig in assignments)
                {

                    List<SubmitAssignments> sa = await _submittedAssignment.FindAsync(e => ((e.Sid == sid) && (e.AssignmentId == assig.Id))).Result.ToListAsync();

                    if (sa != null && sa.Count > 0)
                    {
                        if (sa[0].Remark != null || sa[0].Remark != String.Empty)
                        {
                            assig.Status = sa[0].Status;
                        }
                    }
                    else
                    {

                        assig.Status = "Pending";

                    }
                }

                return new CustomResponse<List<Assignment>>(1, assignments, null);

            }
            catch (Exception e)
            {

                return new CustomResponse<List<Assignment>>(0, null, e.ToString());

            }

        }


        public async Task<CustomResponse<SubmitAssignments>> SubmitAssignment(SubmitAssignments submitAssignment)
        {

            var todayDate = DateTime.Today.ToString("dd/MM/yyyy");

            submitAssignment.SubmittedDate = todayDate;

            if (submitAssignment.Id == null)
            {
                try
                {
                    List<SubmitAssignments> sa = await _submittedAssignment.FindAsync(e => ((e.Sid == submitAssignment.Sid) && (e.AssignmentId == submitAssignment.AssignmentId))).Result.ToListAsync();

                    if (sa.Count > 0)
                    {
                        await _submittedAssignment.ReplaceOneAsync(e => e.Id == sa[0].Id, submitAssignment);

                        return new CustomResponse<SubmitAssignments>(1, submitAssignment, null);
                    }
                    else
                    {

                        await _submittedAssignment.InsertOneAsync(submitAssignment);

                        return new CustomResponse<SubmitAssignments>(1, submitAssignment, null);

                    }
                }
                catch (Exception e)
                {
                    return new CustomResponse<SubmitAssignments>(0, null, e.ToString());
                }


            }
            else
            {
                    await _submittedAssignment.ReplaceOneAsync(e => ((e.Sid == submitAssignment.Sid) && (e.AssignmentId == submitAssignment.AssignmentId)), submitAssignment);

                    return new CustomResponse<SubmitAssignments>(1, submitAssignment, null);
            }


        }
    }
}