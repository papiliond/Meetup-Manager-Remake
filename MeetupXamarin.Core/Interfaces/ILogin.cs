using System;
using System.Collections.Generic;

namespace MeetupXamarin.Core.Interfaces
{
    public interface ILogin
    {
        void LoginAsync(Action<bool, Dictionary<string, string>> loginCallBack);
    }
}
