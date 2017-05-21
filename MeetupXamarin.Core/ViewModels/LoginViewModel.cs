using MeetupXamarin.Core.Commanding;
using MeetupXamarin.Core.Helpers;
using MeetupXamarin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetupXamarin.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        Command refreshLoginCommand;

        public ICommand RefreshLoginCommand
        {
            get { return refreshLoginCommand ?? (refreshLoginCommand = new Command(ExecuteRefreshLoginCommand)); }
        }

        private void ExecuteRefreshLoginCommand()
        {
            if (IsBusy)
                return;

            if (DateTime.UtcNow.Ticks < Settings.KeyValidUntil)
            {
                RenewAccessToken();
            }
            else if (!string.IsNullOrWhiteSpace(Settings.AccessToken) &&
                !string.IsNullOrWhiteSpace(Settings.RefreshToken))
            {
                navigationService.Navigate<GroupsViewModel>();
            }
        }

        private async void RenewAccessToken()
        {
            IsBusy = true;
            bool success = await meetupService.RenewAccessToken();
            IsBusy = false;

            if (success)
            {
                navigationService.Navigate<GroupsViewModel>();
            } else
            {
                messageDialog.SendToast("Please login again to re-validate credentials.");
            }
        }

        Command loginCommand;

        public ICommand LoginCommand
        {
            get { return loginCommand ?? (loginCommand = new Command(ExecuteLoginCommand)); }
        }

        private void ExecuteLoginCommand()
        {
            if (IoC.Resolve<string>("Device") == "Mobile")
            {
                //Navigate to Meetup Login Page (Auth)
            }
            else
            {
                var login = IoC.Resolve<ILogin>();
                login.LoginAsync(async (success, properties) =>
                await FinishLogin(success, properties));
            }
        }

        public async Task FinishLogin(bool success, Dictionary<string, string> properties)
        {
            if (success)
            {
                Settings.AccessToken = properties["access_token"];
                Settings.RefreshToken = properties["refresh_token"];

                long time;
                long.TryParse(properties["expires_in"], out time);
                var nextTime = DateTime.UtcNow.AddSeconds(time).Ticks;
                Settings.KeyValidUntil = nextTime;

                IsBusy = true;
                try
                {
                    var user = await meetupService.GetCurrentMember();
                    Settings.UserId = user.Id.ToString();
                    Settings.UserName = user.Name ?? string.Empty;
                }
                catch (Exception e)
                {
                    if (Settings.Insights)
                        Xamarin.Insights.Report(e);
                }

                IsBusy = false;
                messageDialog.SendToast("Welcome to my app, " + Settings.UserName);
                navigationService.Navigate<GroupsViewModel>();

            }
            else
            {
                //TODO: Begin Invoke On Main Thread
                messageDialog.SendMessage("Please try again using your meetup credentials.", "Login Failure");
                IsBusy = false;
               
            }
        }
    }
}
