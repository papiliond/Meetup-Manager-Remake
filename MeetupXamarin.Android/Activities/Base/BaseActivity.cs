using System;
using Android.Content;
using Android.OS;
using MeetupXamarin.Core;
using MeetupXamarin.Navigation;
using Android.App;
using Android.Views;
using MeetupXamarin.Core.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;

namespace MeetupXamarin.Android.Activities
{
    public abstract class BaseActivity : AppCompatActivity
    {
        protected object DataContext { get; set; }
        protected ProgressDialog ProgressDialog { get; set; }
        protected Toolbar Toolbar { get; set; }
        protected bool UsesToolbar { get; set; } = true;

        public BaseActivity()
        {
            IoC.Resolve<ICurrentActivityHolder>().SetCurrent(this);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitializeViewModel();
            
            ((BaseViewModel)DataContext).ExecutedRefreshCommand += OnFirstRefreshExecuted;
            ((BaseViewModel)DataContext).FinishedFirstLoad += (index) => OnFirstLoadFinished();
        }

        protected virtual void InitializeViewModel ()
        {
            var vmAssembly = Intent.GetStringExtra("vmAssembly");
            var vmNamespace = Intent.GetStringExtra("vmNamespace");

            DataContext = Activator.CreateInstance(vmAssembly, vmNamespace).Unwrap();
        }

        protected void SetUpProgressDialog(Activity activity, string message)
        {
            ProgressDialog = new ProgressDialog(activity);
            ProgressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            ProgressDialog.Indeterminate = true;
            ProgressDialog.SetCancelable(false);
            ProgressDialog.SetMessage(message);
        }

        protected virtual void SetActivityContentView(int layoutResId)
        {
            View view = LayoutInflater.Inflate(layoutResId, null);
            SetUpToolbar(view);
            base.SetContentView(view);
        }

        protected void SetUpToolbar(View view)
        {
            Toolbar = view.FindViewById<Toolbar>(Resource.Id.support_toolbar);
            if (Toolbar != null)
            {
                if (UsesToolbar)
                {
                    SetSupportActionBar(Toolbar);
                    SupportActionBar.Title = Title ?? null;
                }
                else
                    Toolbar.Visibility = ViewStates.Gone;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == global::Android.Resource.Id.Home)
            {
                Finish();
            }

            return base.OnOptionsItemSelected(item);
        }

        public void OnFirstRefreshExecuted ()
        {
            if (ProgressDialog != null)
                ProgressDialog.Show();
            ((BaseViewModel)DataContext).ExecutedRefreshCommand -= OnFirstRefreshExecuted;

        }

        public void OnFirstLoadFinished ()
        {
            if (ProgressDialog != null)
                ProgressDialog.Hide();
        }

    }
}