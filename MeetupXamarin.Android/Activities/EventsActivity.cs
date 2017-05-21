using System;
using MeetupXamarin.Core.Helpers;
using Android.OS;
using MeetupXamarin.Core.Models;
using MeetupXamarin.Core.ViewModels;
using Newtonsoft.Json;
using Android.App;
using System.Timers;
using Android.Widget;
using MeetupXamarin.Android.Adapters;
using SearchView = Android.Support.V7.Widget.SearchView;
using MeetupXamarin.Android.Activities.Base;
using static Android.Widget.AdapterView;

namespace MeetupXamarin.Android.Activities
{
    [Activity(Label = "EventsActivity")]
    public class EventsActivity : BaseRefreshableActivity
    {
        EventsViewModel ViewModel;
        EventAdapter EventAdapter;
        ListView ListView;
        SearchView SearchView;
        bool allEvents;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = ((EventsViewModel)DataContext).GroupName;

            SetActivityContentView(Resource.Layout.Events, Resource.Id.events_layout);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SetUpProgressDialog(this, "Loading Events...");

            allEvents = Settings.ShowAllEvents;
            ViewModel = (EventsViewModel)DataContext;
            ListView = FindViewById<ListView>(Resource.Id.eventsList);
            SearchView = FindViewById<SearchView>(Resource.Id.events_searchview);
            EventAdapter = new EventAdapter(this, ViewModel.Events);
            ListView.Adapter = EventAdapter;

            #region Events

            ListView.ItemClick += GoToEvent;

            SearchView.QueryTextChange += (s, e) => EventAdapter.Filter(e.NewText);
            SearchView.QueryTextSubmit += (s, e) => EventAdapter.Filter(e.Query);

            ViewModel.Events.CollectionChanged += (s, e) =>
            {
                EventAdapter.UpdateListView();
            };

            ViewModel.FinishedFirstLoad += (index) =>
            {
                if (ViewModel.Events.Count == 0)
                    return;

                Timer timer = new Timer();
                timer.Interval = 1000;
                timer.Elapsed += (s, e) => RunOnUiThread(() => ListView.SetSelection(index));
            };

            #endregion
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

        protected void GoToEvent(object sender, ItemClickEventArgs e)
        {
            ViewModel.GoToEventCommand.Execute(ViewModel.Events[e.Position]);
        }

    }
}