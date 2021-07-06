using jay.school.contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace jay.school.contracts.Contracts
{
    public interface IAnnouncementService
    {
        Task<CustomResponse<Announcement>> AddAnnouncement(Announcement announcement);
        Task<CustomResponse<List<Announcement>>> GetAnnouncement(string from, string std, string sec);

    }
}
