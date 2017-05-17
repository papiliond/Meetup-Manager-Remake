using System;
using Android.App;
using MeetupXamarin.Core;
using MeetupXamarin.Core.Interfaces;
using MeetupXamarin.Core.ViewModels;
using MeetupXamarin.Navigation;
using Android.Content;
using Android.OS;
using MeetupXamarin.Core.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MeetupXamarin.Android.Navigation
{
    public class NavigationService : INavigationService, ICurrentActivityHolder
    {
        public Activity Current { get; private set; }

        public void SetCurrent(Activity current)
        {
            Current = current;
        }

        public void Navigate<T>(object[] args) where T : BaseViewModel
        {
            var viewModelType = typeof(T);
            var activity = ViewType.FromViewModel(viewModelType);
            var intent = new Intent(Current, activity);

            var bundle = new Bundle();
            bundle.PutString("vmAssembly", viewModelType.Assembly.FullName);
            bundle.PutString("vmNamespace", viewModelType.FullName);

            if (args != null && args.Length > 0)
            {
                int argNo = 0;
                foreach (var arg in args)
                    bundle.PutString("arg_" + argNo++, JsonConvert.SerializeObject(arg));
            }

            intent.PutExtras(bundle);
            Current.StartActivity(intent);
        }


        public void NavigateBack()
        {
            Current.Finish();
        }



    }
}