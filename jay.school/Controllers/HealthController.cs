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
    public class HealthController : ControllerBase
    {
        [Route("ping")]
        [HttpGet]
        public ActionResult GetStudentsByPh(string ph)
        {

            return Ok("working..." + DateTime.Now.ToString());

        }

    }

}