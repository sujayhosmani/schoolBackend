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
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [Route("addAnnouncement")]
        [HttpPost]
        public async Task<CustomResponse<Announcement>> AddAssignment(CustomRequest<Announcement> announcement){

            return await _announcementService.AddAnnouncement(announcement.Data);
        }

       
        
        [Route("getAAnnouncement")]
        [HttpGet]
        public async Task<CustomResponse<List<Announcement>>> GetAnnouncement(string from, string std, string sec, string tid){

            return await _announcementService.GetAnnouncement(from, std, sec, tid);
        }


    }

}