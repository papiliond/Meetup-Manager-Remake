using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MeetupXamarin.Core.Interfaces;
using Xamarin.Auth;
using MeetupXamarin.Core.Services;
using MeetupXamarin.Core;
using MeetupXamarin.Navigation;

namespace MeetupXamarin.Android.Services
{
    public class Login : ILogin
    {
        readonly OAuth2Authenticator auth =
            new OAuth2Authenticator(MeetupService.ClientId, MeetupService.ClientSecret, string.Empty, new Uri(MeetupService.AuthorizeUrl), new Uri(MeetupService.RedirectUrl), new Uri(MeetupService.AccessTokenUrl));

        public void LoginAsync(Action<bool, Dictionary<string, string>> loginCallBack)
        {
            var activity = IoC.Resolve<ICurrentActivityHolder>().Current;

            auth.AllowCancel = true;

            auth.Completed += (s, ee) =>
            {
                loginCallBack?.Invoke(ee.IsAuthenticated, ee.Account == null ? null : ee.Account.Properties);
            };

            var intent = (Intent)auth.GetUI(activity);
            activity.StartActivity(intent);
        }
    }
}