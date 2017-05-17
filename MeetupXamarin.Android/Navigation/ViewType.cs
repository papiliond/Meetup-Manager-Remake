using System;
using System.Collections.Generic;
using MeetupXamarin.Core.ViewModels;
using MeetupXamarin.Android.Activities;

namespace MeetupXamarin.Android.Navigation
{
    public static class ViewType
    {
        private static readonly Dictionary<Type, Type> ViewsByViewModels = new Dictionary<Type, Type>
        {
            {typeof(LoginViewModel), typeof(LoginActivity)},
            {typeof(GroupsViewModel), typeof(GroupsActivity)},
            {typeof(EventsViewModel), typeof(EventsActivity)},
            {typeof(EventViewModel), typeof(EventActivity)}
        };

        public static Type FromViewModel(Type viewModelType) => ViewsByViewModels[viewModelType];

    }
}