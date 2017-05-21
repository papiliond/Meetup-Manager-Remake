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
            Title = "Event";

            SetActivityContentView(Resource.Layout.Event, Resource.Id.event_layout);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SetUpProgressDialog(this, "Loading Event...");

            ViewModel = (EventViewModel)DataContext;
            ListView = FindViewById<ListView>(Resource.Id.eventList);
            SearchView = FindViewById<SearchView>(Resource.Id.event_searchview);
            MemberAdapter = new MemberAdapter(this, ViewModel.Members);
            ListView.Adapter = MemberAdapter;

            #region Events

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

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}