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
    }
}