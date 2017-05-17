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
    }
}
