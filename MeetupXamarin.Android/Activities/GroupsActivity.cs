using Android.App;
using Android.OS;
using MeetupXamarin.Core.ViewModels;
using MeetupXamarin.Core.Helpers;
using System.Timers;
using Android.Widget;
using MeetupXamarin.Android.Adapters;

namespace MeetupXamarin.Android.Activities
{
    [Activity(Label = "GroupsActivity")]
    public class GroupsActivity : BaseActivity
    {
        GroupsViewModel ViewModel;
        ListView ListView;
        bool managerMode;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Groups);

            ViewModel = (GroupsViewModel)DataContext;
            managerMode = Settings.OrganizerMode;

            ListView = FindViewById<ListView>(Resource.Id.groupsList);
            GroupsAdapter groupsAdapter = new GroupsAdapter(this, ViewModel.Groups);
            ListView.Adapter = groupsAdapter;

            SetUpProgressDialog(this, "Loading Groups...");
            SetActionBar(FindViewById<Toolbar>(Resource.Id.support_toolbar));
            ActionBar.Title = "Groups";

            ViewModel.Groups.CollectionChanged += (s, e) =>
            {
                groupsAdapter.UpdateListView();
            };

            ViewModel.FinishedFirstLoad += (index) =>
            {
                if (ViewModel.Groups.Count == 0)
                    return;

                Timer timer = new Timer();
                timer.Interval = 1000;
                timer.Elapsed += (s, e) => RunOnUiThread(() => ListView.SetSelection(index));
            };

            ListView.ItemClick += (sender, e) =>
            {
                if (ViewModel.Groups[e.Position] != null)
                    ViewModel.GoToGroupCommand.Execute(ViewModel.Groups[e.Position]);
            };
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