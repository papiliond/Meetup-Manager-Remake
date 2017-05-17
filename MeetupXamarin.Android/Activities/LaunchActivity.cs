using Android.App;
using Android.OS;
using MeetupXamarin.Core;
using MeetupXamarin.Android.Helpers;
using MeetupXamarin.Core.Interfaces.Database;
using MeetupXamarin.Core.Interfaces;
using MeetupXamarin.Android.Navigation;
using MeetupXamarin.Core.ViewModels;
using MeetupXamarin.Navigation;
using MeetupXamarin.Core.Services;
using MeetupXamarin.Android.Services;
using MeetupXamarin.Core.Services.Database;
using Android.Support.V7.App;
using System.Threading.Tasks;

namespace MeetupXamarin.Android.Activities
{
    [Activity(Label = "MeetupXamarin", MainLauncher = true, Icon = "@drawable/ic_launcher", Theme = "@style/MyTheme.Splash", NoHistory = true)]
    public class LaunchActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var navigationService = new NavigationService();
            navigationService.SetCurrent(this);
            InitialiteIoC(navigationService);
        }

        private void InitialiteIoC (NavigationService navigationService)
        {
            IoC.Register<IFileHelper>(new FileHelper());
            IoC.Register<IMessageDialog>(new MessageDialog());
            IoC.Register<IMeetupService>(new MeetupService());
            IoC.Register<IDataService>(new DataService());
            IoC.Register<ILogin>(new Login());
            IoC.Register<INavigationService>(navigationService);
            IoC.Register<ICurrentActivityHolder>(navigationService);
            IoC.Register<string>("Device", "Mobile");
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        async void SimulateStartup()
        {
            await Task.Delay(1500);
            IoC.Resolve<INavigationService>().Navigate<LoginViewModel>();
        }

        public override void OnBackPressed() { }
    }
}