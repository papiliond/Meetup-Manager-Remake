using Android.App;
using Android.OS;
using MeetupXamarin.Core.ViewModels;
using MeetupXamarin.Core.Helpers;
using System.Timers;
using Android.Widget;
using MeetupXamarin.Android.Adapters;
using Android.Support.V4.Widget;
using static Android.Support.V4.Widget.SwipeRefreshLayout;
using System;
using MeetupXamarin.Android.Activities.Base;

namespace MeetupXamarin.Android.Activities
{
    [Activity(Label = "GroupsActivity")]
    public class GroupsActivity : BaseRefreshableActivity
    {
        GroupsViewModel ViewModel;
        GroupAdapter GroupAdapter;
        ListView ListView;
        bool managerMode;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Title = "Groups";
            SetActivityContentView(Resource.Layout.Groups, Resource.Id.groups_layout);
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);
            SetUpProgressDialog(this, "Loading Groups...");

            ViewModel = (GroupsViewModel)DataContext;
            managerMode = Settings.OrganizerMode;
            ListView = FindViewById<ListView>(Resource.Id.groupsList);

            GroupAdapter = new GroupAdapter(this, ViewModel.Groups);
            ListView.Adapter = GroupAdapter;

            #region Events

            ViewModel.Groups.CollectionChanged += (s, e) =>
            {
                GroupAdapter.UpdateListView();
            };

            ViewModel.FinishedFirstLoad += (index) =>
            {
                if (ViewModel.Groups.Count == 0)
                    return;

                Timer timer = new Timer()
                {
                    Interval = 1000
                };
                timer.Elapsed += (s, e) => RunOnUiThread(() => ListView.SetSelection(index));
            };

            ListView.ItemClick += (sender, e) =>
            {
                if (ViewModel.Groups[e.Position] != null)
                    ViewModel.GoToGroupCommand.Execute(ViewModel.Groups[e.Position]);
            };

            #endregion
        }
    
        protected override void OnStart()
        {
            base.OnStart();
            if (ViewModel.IsBusy)
                return;

            var forceRefresh = managerMode != Settings.OrganizerMode;
            managerMode = Settings.OrganizerMode;
            if (!forceRefresh && ViewModel.Groups.Count > 0)
                return;

            ViewModel.RefreshCommand.Execute(null);
        }

    }
}