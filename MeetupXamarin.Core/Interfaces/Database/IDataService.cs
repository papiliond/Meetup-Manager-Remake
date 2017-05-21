using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetupXamarin.Core.Models;
using MeetupXamarin.Core.Models.Database;

namespace MeetupXamarin.Core.Interfaces.Database
{
    public interface IDataService
    {
        Task CheckInMember(EventRSVP rsvp);
        Task CheckOutMember(string eventId, string userId);
        Task<bool> IsCheckedIn(string eventId, string userId, string eventName, string groupId, string groupName, long eventDate);

        Task AddNewMember(NewMember member);
        Task<IEnumerable<NewMember>> GetNewMembers(string eventId);
        Task RemoveNewMember(int id);
        Task<IEnumerable<NewMember>> GetNewMembersForGroup(string groupId);
        Task<IEnumerable<EventRSVP>> GetRSVPsForGroup(string groupId);
    }
}
