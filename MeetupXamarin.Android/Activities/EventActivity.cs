using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MeetupXamarin.Core.ViewModels;
using MeetupXamarin.Android.Activities.Base;
using Newtonsoft.Json;
using MeetupXamarin.Core.Models;
using MeetupXamarin.Android.Adapters;
using SearchView = Android.Support.V7.Widget.SearchView;
using System.Timers;

namespace MeetupXamarin.Android.Activities
{
    [Activity(Label = "EventActivity")]
    public class EventActivity : BaseRefreshableActivity
    {
        EventViewModel ViewModel;
        MemberAdapter MemberAdapter;
        ListView ListView;
        SearchView SearchView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewModel = (EventViewModel)DataContext;
            Title = ViewModel.EventName;

            SetActivityContentView(Resource.Layout.Event, Resource.Id.event_layout);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SetUpProgressDialog(this, "Loading Event...");

            ListView = FindViewById<ListView>(Resource.Id.membersList);
            //ListView.EmptyView = Resource.Layout
            SearchView = FindViewById<SearchView>(Resource.Id.event_searchview);
            MemberAdapter = new MemberAdapter(this, ViewModel.Members);
            ListView.Adapter = MemberAdapter;

            var tapToCheckinText = FindViewById<TextView>(Resource.Id.taptocheckin_text);
            tapToCheckinText.Text = tapToCheckinText.Text + " " + ViewModel.RSVPCount;

            #region Events

            SearchView.QueryTextChange += (s, e) => MemberAdapter.Filter(e.NewText);
            SearchView.QueryTextSubmit += (s, e) => MemberAdapter.Filter(e.Query);

            ViewModel.Members.CollectionChanged += (s, e) => MemberAdapter.UpdateListView();


            ListView.ItemClick += (s, e) =>
            {
                if (ListView.SelectedItem == null)
                    return;

                ViewModel.CheckInCommand.Execute(ViewModel.Members[e.Position]);
            };

            ViewModel.FinishedFirstLoad += (index) =>
            {
                if (ViewModel.Members.Count == 0)
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
            var @event = JsonConvert.DeserializeObject<Event>(Intent.GetStringExtra("arg_0"));
            var groupId = JsonConvert.DeserializeObject<string>(Intent.GetStringExtra("arg_1"));
            var groupName = JsonConvert.DeserializeObject<string>(Intent.GetStringExtra("arg_2"));

            DataContext = Activator.CreateInstance(assemblyName: vmAssembly, typeName: vmNamespace, args: new object[] { @event, groupId, groupName },
                ignoreCase: false, bindingAttr: 0, binder: null, culture: null, activationAttributes: null).Unwrap();
        }

        //TODO
        //public void OnRemove(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    ViewModel.DeleteUserCommand.Execute(mi.CommandParameter);
        //}

        protected override void OnStart()
        {
            base.OnStart();

            if (ViewModel.IsBusy || ViewModel.Members.Count > 0)
                return;

            ViewModel.RefreshCommand.Execute(null);

        }
    }
}