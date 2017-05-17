using System;
using MeetupXamarin.Core.Helpers;
using Android.OS;
using MeetupXamarin.Core.Models;
using MeetupXamarin.Core.ViewModels;
using Newtonsoft.Json;
using Android.App;
using System.Timers;
using Android.Widget;
using MeetupXamarin.Android.Helpers;
using MeetupXamarin.Android.Adapters;
using Support = Android.Support;

namespace MeetupXamarin.Android.Activities
{
    [Activity(Label = "EventsActivity")]
    public class EventsActivity : BaseActivity
    {
        private EventsViewModel ViewModel;
        ListView ListView;
        bool allEvents;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Events);
            allEvents = Settings.ShowAllEvents;

            ViewModel = (EventsViewModel)DataContext;

            ListView = FindViewById<ListView>(Resource.Id.eventsList);
            EventsAdapter eventsAdapter = new EventsAdapter(this, ViewModel.Events);
            ListView.Adapter = eventsAdapter;

            SetUpProgressDialog(this, "Loading Events...");
            SetActionBar(FindViewById<Toolbar>(Resource.Id.support_toolbar));

            ActionBar.Title = TextFormatter.ShortenText(ViewModel.GroupName, 15);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            ViewModel.Events.CollectionChanged += (s, e) =>
            {
                eventsAdapter.UpdateListView();
            };

            ViewModel.FinishedFirstLoad += (index) =>
            {
                if (ViewModel.Events.Count == 0)
                    return;

                Timer timer = new Timer();
                timer.Interval = 1000;
                timer.Elapsed += (s, e) => RunOnUiThread(() => ListView.SetSelection(index));
            };

            ListView.ItemClick += (sender, e) =>
            {
                if (ViewModel.Events[e.Position] != null)
                    ViewModel.GoToEventCommand.Execute(ViewModel.Events[e.Position]);
            };

        }

        protected override void InitializeViewModel()
        {
            var vmAssembly = Intent.GetStringExtra("vmAssembly");
            var vmNamespace = Intent.GetStringExtra("vmNamespace");

            var group = JsonConvert.DeserializeObject<Group>(Intent.GetStringExtra("arg_0"));

            DataContext = Activator.CreateInstance(assemblyName: vmAssembly, typeName: vmNamespace, args: new object[] { group },
                ignoreCase: false, bindingAttr: 0, binder: null, culture: null, activationAttributes: null).Unwrap();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (ViewModel.IsBusy)
                return;

            var forceRefresh = allEvents != Settings.ShowAllEvents;
            allEvents = Settings.ShowAllEvents;
            if (ViewModel.Events.Count > 0 && !forceRefresh)
                return;

            ViewModel.RefreshCommand.Execute(null);
        }

    }
}