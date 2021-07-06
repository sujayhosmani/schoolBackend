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
    public class AnnouncementBusiness : IAnnouncementService
    {

        private readonly IMongoCollection<Announcement> _announcement;
        private readonly IMDBContext _announcementMDBContext;

        public AnnouncementBusiness(IMDBContext announcementMDBContext)
        {
            _announcementMDBContext = announcementMDBContext;

            _announcement = _announcementMDBContext.GetCollection<Announcement>(typeof(Announcement).Name);

        }

        public async Task<CustomResponse<Announcement>> AddAnnouncement(Announcement announcement)
        {
            if (announcement.Id == null)
            {

                var todayDate = DateTime.Today.ToString("dd/MM/yyyy");

                announcement.StartDate = todayDate;

                await _announcement.InsertOneAsync(announcement);

                return new CustomResponse<Announcement>(1, announcement, null);

            }
            else
            {
                return new CustomResponse<Announcement>(0, null, "Id Exists");
            }


        }

        public async Task<CustomResponse<List<Announcement>>> GetAnnouncement(string from, string std, string sec)
        {
            try
            {

                if (from == "student")
                {
                    List<Announcement> announcements = await _announcement.FindAsync(e => ((e.StdSec.Contains(std + sec)) || (e.isForSchool == true))).Result.ToListAsync();

                    return new CustomResponse<List<Announcement>>(1, announcements, null);
                }
                else
                {
                    List<Announcement> announcements = await _announcement.FindAsync(e => e.isForSchool == true).Result.ToListAsync();

                    return new CustomResponse<List<Announcement>>(1, announcements, null);
                }

            }
            catch (Exception e)
            {

                return new CustomResponse<List<Announcement>>(0, null, e.ToString());

            }

        }

    }
}