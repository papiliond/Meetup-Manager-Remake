using MeetupXamarin.Core.Interfaces.Database;
using MeetupXamarin.Core.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupXamarin.Core.Services.Database
{
    /// <summary>
    /// Is the main data service that can be used in the application for reading/writing to the cameroon database.
    /// </summary>
    public class DataService : IDataService
    {
        private readonly MeetupManagerDatabase database;

        public DataService()
        {
            database = new MeetupManagerDatabase(IoC.Resolve<IFileHelper>().GetLocalFilePath("meetupmanager.db3"));
        }

        #region IDataService implementation

        public async Task CheckInMember(EventRSVP rsvp)
        {
            await Task.Factory.StartNew(() =>
            {
                database.SaveItem<EventRSVP>(rsvp);
            });
        }

        public async Task<bool> IsCheckedIn(string eventId, string userId, string eventName, string groupId, string groupName, long eventDate)
        {
            return await Task.Factory.StartNew<bool>(() =>
            {
                var e = database.GetEventRSVP(eventId, userId);
                if (e != null && string.IsNullOrEmpty(e.GroupId))
                {
                    e.EventName = eventName;
                    e.GroupId = groupId;
                    e.GroupName = groupName;
                    e.EventDate = eventDate;
                    database.SaveItem<EventRSVP>(e);
                }
                return e != null;
            });
        }

        public Task CheckOutMember(string eventId, string userId)
        {
            return Task.Factory.StartNew(() =>
            {
                var item = database.GetEventRSVP(eventId, userId);
                if (item != null)
                    database.DeleteItem<EventRSVP>(item);
            });
        }
        #endregion

        public Task AddNewMember(NewMember member)
        {
            return Task.Factory.StartNew(() =>
            {
                database.SaveItem<NewMember>(member);
            });
        }

        public Task<IEnumerable<NewMember>> GetNewMembers(string eventId)
        {
            return Task.Factory.StartNew(() => database.GetNewMembers(eventId));
        }

        public Task RemoveNewMember(int id)
        {
            return Task.Factory.StartNew(() => database.DeleteItem<NewMember>(id));
        }

        public Task<IEnumerable<NewMember>> GetNewMembersForGroup(string groupId)
        {
            return Task.Factory.StartNew(() => database.GetNewMembersByDate(groupId));
        }

        public Task<IEnumerable<EventRSVP>> GetRSVPsForGroup(string groupId)
        {
            return Task.Factory.StartNew(() => database.GetRSVPsByDate(groupId));
        }
    }
}
