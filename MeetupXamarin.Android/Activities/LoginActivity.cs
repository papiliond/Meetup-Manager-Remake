using Android.App;
using Android.OS;
using Android.Widget;
using MeetupXamarin.Core.Services;
using MeetupXamarin.Core.ViewModels;
using Xamarin.Auth;
using System;
using Android.Content;

namespace MeetupXamarin.Android.Activities
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : BaseActivity
    {
        LoginViewModel ViewModel;
        bool LoginInProgress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            ViewModel = (LoginViewModel)DataContext;

            SetActionBar(FindViewById<Toolbar>(Resource.Id.support_toolbar));
            ActionBar.Title = "Meetup Manager";

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