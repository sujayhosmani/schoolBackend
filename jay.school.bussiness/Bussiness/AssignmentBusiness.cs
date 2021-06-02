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
         private readonly IMDBContext _assignmentMDBContext;

        public AssignmentBusiness(IMDBContext assignmentMDBContext)
        {
            _assignmentMDBContext = assignmentMDBContext;

             _assignment = _assignmentMDBContext.GetCollection<Assignment>(typeof(Assignment).Name);

        }

        public async Task<CustomResponse<Assignment>> AddAssignment(Assignment assignment)
        {
            if (assignment.Id == null)
            {

                var todayDate = DateTime.Today.ToString("MM/dd/yyyy");

                assignment.StartDate = todayDate;
                
                assignment.EndDate = DateTime.Today.AddDays(int.Parse(assignment.EndDate ?? "0")).ToString("MM/dd/yyyy");

                await _assignment.InsertOneAsync(assignment); 

                return new CustomResponse<Assignment>(1, assignment, null);

            }
            else
            {
                return new CustomResponse<Assignment>(0, null, "Id Exists");
            }


        }

        public async Task<CustomResponse<List<Assignment>>> GetAssignmentsByTid(string tid){
            try{

                List<Assignment> assignments = await _assignment.FindAsync(e => e.Tid == tid).Result.ToListAsync();

                return new CustomResponse<List<Assignment>>(1, assignments, null);

            }catch(Exception e){

                return new CustomResponse<List<Assignment>>(0, null, e.ToString());

            }
               
        }

        public async Task<CustomResponse<List<Assignment>>> GetAssignmentsByClass(string std, string section){
            try{
                
                List<Assignment> assignments = await _assignment.FindAsync(e => ((e.Std == std) && (e.Section == section))).Result.ToListAsync();

                return new CustomResponse<List<Assignment>>(1, assignments, null);

            }catch(Exception e){

                return new CustomResponse<List<Assignment>>(0, null, e.ToString());

            }
               
        }

    }
}