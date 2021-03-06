﻿using Android.OS;
using static Android.Support.V4.Widget.SwipeRefreshLayout;
using MeetupXamarin.Core.ViewModels;
using Android.Support.V4.Widget;

namespace MeetupXamarin.Android.Activities.Base
{
    public class BaseRefreshableActivity : BaseActivity, IOnRefreshListener
    {
        SwipeRefreshLayout SwipeRefreshLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ((BaseViewModel)DataContext).FinishedLoad += () => SwipeRefreshLayout.Refreshing = false;
        }

        protected void SetActivityContentView(int layoutResId, int swipeRefreshLayoutResId)
        {
            base.SetActivityContentView(layoutResId);
            SwipeRefreshLayout = FindViewById<SwipeRefreshLayout>(swipeRefreshLayoutResId);
            SwipeRefreshLayout.SetOnRefreshListener(this);
        }

        public void OnRefresh()
        {
            ((BaseViewModel)DataContext).RefreshCommand.Execute(null);
        }

    }
}