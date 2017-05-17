using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetupXamarin.Core.Services.Responses;
using MeetupXamarin.Core.Models;
using MeetupXamarin.Core.Services.Request;

namespace MeetupXamarin.Core.Interfaces
{
    public interface IMeetupService
    {
        Task<EventsRootObject> GetEvents(string groupId, int skip);
        Task<RSVPsRootObject> GetRSVPs(string eventId, int skip);
        Task<GroupsRootObject> GetGroups(string memberId, int skip);
        Task<RequestTokenObject> GetToken(string code);
        Task<bool> RenewAccessToken();

        Task<LoggedInUser> GetCurrentMember();
    }
}
