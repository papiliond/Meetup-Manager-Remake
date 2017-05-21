using MeetupXamarin.Core.Commanding;
using MeetupXamarin.Core.Models;
using MeetupXamarin.Core.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetupXamarin.Core.ViewModels
{
    public class MemberViewModel : BaseViewModel
    {
        public static string DefaultIcon = @"http://refractored.com/default.png";

        public Member Member { get; set; }

        private bool checkedIn;
        public bool CheckedIn
        {
            get { return checkedIn; }
            set
            {
                SetProperty(ref checkedIn, value);
            }
        }

        private string guests;
        public string Guests
        {
            get { return guests; }
            set
            {
                SetProperty(ref guests, value);
            }
        }

        public bool CanDelete
        {
            get { return NewUserId != 0; }
        }

        public int NewUserId { get; set; }

        public bool HasGuests { get { return !string.IsNullOrWhiteSpace(guests); } }

        public MemberPhoto Photo { get; set; }
        public string ThumbLink
        {
            get
            {
                if (Photo == null)
                    return DefaultIcon;

                return Photo.ThumbLink;
            }
        }

        readonly string eventId, eventName, groupId, groupName;
        readonly long eventDate;

        public string Name { get { return Member.Name; } }

        public MemberViewModel(Member member, MemberPhoto photo, string eventId, string eventName, string groupId, string groupName, long eventDate, int guests = 0)
        {
            this.guests = string.Empty;

            if (guests == 1)
                this.guests = "1 guest";
            else if (guests > 1)
                this.guests = guests + "guests";

            Member = member;
            this.eventId = eventId;
            this.eventName = eventName;
            this.groupId = groupId;
            this.groupName = groupName;
            this.eventDate = eventDate;
            Photo = photo ?? new MemberPhoto
            {
                HighResLink = DefaultIcon,
                PhotoId = 0,
                ThumbLink = DefaultIcon,
                PhotoLink = DefaultIcon
            };

            if (string.IsNullOrWhiteSpace(Photo.HighResLink))
                Photo.HighResLink = DefaultIcon;


            if (string.IsNullOrWhiteSpace(Photo.ThumbLink))
                Photo.ThumbLink = DefaultIcon;

            if (string.IsNullOrWhiteSpace(Photo.PhotoLink))
                Photo.PhotoLink = DefaultIcon;
        }

        Command checkInCommand;

        public ICommand CheckInCommand
        {
            get { return checkInCommand ?? (checkInCommand = new Command(async () => await ExecuteCheckInCommand())); }
        }

        async Task ExecuteCheckInCommand()
        {

            await dataService.CheckInMember(new EventRSVP(eventId, Member.MemberId.ToString(), eventName, groupId, groupName, eventDate));

            CheckedIn = true;

        }

    }
}
