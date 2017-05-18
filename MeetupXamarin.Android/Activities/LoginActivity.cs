using Android.App;
using Android.OS;
using Android.Widget;
using MeetupXamarin.Core.Services;
using MeetupXamarin.Core.ViewModels;
using Xamarin.Auth;
using System;
using Android.Content;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MeetupXamarin.Android.Activities
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : BaseActivity
    {
        LoginViewModel ViewModel;
        bool LoginInProgress = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = "Meetup Manager";
            SetActivityContentView(Resource.Layout.Login);

            ViewModel = (LoginViewModel)DataContext;

            var loginButton = FindViewById<Button>(Resource.Id.loginButton);
            loginButton.Click += (sender, e) => LoginProcess();
        }

        protected override void OnResume()
        {
            base.OnResume();
            ViewModel.RefreshLoginCommand.Execute(null);
        }

        private async void LoginProcess()
        {
            LoginViewModel viewModel = (LoginViewModel)DataContext;

            if (LoginInProgress)
                return;

            LoginInProgress = true;
            try
            {
                var auth = new OAuth2Authenticator(MeetupService.ClientId,
                    MeetupService.ClientSecret,
                    string.Empty,
                    new Uri(MeetupService.AuthorizeUrl),
                    new Uri(MeetupService.RedirectUrl),
                    new Uri(MeetupService.AccessTokenUrl));

                auth.IsLoadableRedirectUri = false;
                auth.AllowCancel = true;
                // If authorization succeeds or is canceled, .Completed will be fired.
                auth.Completed += async (succeed, ee) =>
                {
                    await viewModel.FinishLogin(ee.IsAuthenticated, ee.Account == null ? null : ee.Account.Properties);
                    LoginInProgress = false;
                    Finish();
                };

                auth.Error += (s, ee) =>
                {
                    //Should exit.
                };

                auth.IsLoadableRedirectUri = false;
                StartActivity(auth.GetUI(this) as Intent);

            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                await viewModel.FinishLogin(false, null);
                LoginInProgress = false;
                Finish();
            }
        }


    }
}