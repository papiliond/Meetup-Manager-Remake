using System;
using Android.App;

namespace MeetupXamarin.Navigation
{
    public interface ICurrentActivityHolder
    {
        Activity Current { get; }
        void SetCurrent(Activity current);
    }
}
