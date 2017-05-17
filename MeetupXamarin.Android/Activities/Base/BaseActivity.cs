using System;
using Android.Content;
using Android.OS;
using MeetupXamarin.Core;
using MeetupXamarin.Navigation;
using Android.App;
using Android.Views;
using MeetupXamarin.Core.ViewModels;

namespace MeetupXamarin.Android.Activities
{
    public abstract class BaseActivity : Activity
    {
        public object DataContext { get; set; }
        public ProgressDialog ProgressDialog { get; set; }

        public BaseActivity()
        {
            IoC.Resolve<ICurrentActivityHolder>().SetCurrent(this);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitializeViewModel();
            
            ((BaseViewModel)DataContext).ExecutedRefreshCommand += OnRefreshExecuted;
            ((BaseViewModel)DataContext).FinishedFirstLoad += (index) => OnFirstLoadFinished();
        }

        protected virtual void InitializeViewModel ()
        {
            var vmAssembly = Intent.GetStringExtra("vmAssembly");
            var vmNamespace = Intent.GetStringExtra("vmNamespace");

            DataContext = Activator.CreateInstance(vmAssembly, vmNamespace).Unwrap();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        public void OnRefreshExecuted ()
        {
            if (ProgressDialog != null)
                ProgressDialog.Show();
        }

        public void OnFirstLoadFinished ()
        {
            if (ProgressDialog != null)
                ProgressDialog.Hide();
        }

        public void SetUpProgressDialog(Activity activity, string message)
        {
            ProgressDialog = new ProgressDialog(activity);
            ProgressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            ProgressDialog.Indeterminate = true;
            ProgressDialog.SetCancelable(false);
            ProgressDialog.SetMessage(message);
        }

    }
}