using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MeetupXamarin.Core.Models;
using System.Collections.ObjectModel;
using MeetupXamarin.Android.Helpers;

namespace MeetupXamarin.Android.Adapters
{
    public class EventsAdapter : BaseAdapter<Event>
    {
        public ObservableCollection<Event> Items { get; set; }
        private Activity activity;

        public EventsAdapter (Activity activity, ObservableCollection<Event> items)
        {
            this.activity = activity;
            Items = items;
        }

        #region BaseAdapter implementation
        public override Event this[int position]
        {
            get
            {
                return Items[position];
            }
        }

        public override int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return long.Parse(Items[position].Id);
        }
        #endregion

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.eventlist_item, parent, false);

            view.FindViewById<TextView>(Resource.Id.eventDateMonthAndDay).Text = Items[position].MonthDay;
            view.FindViewById<TextView>(Resource.Id.eventDateyear).Text = Items[position].Year;
            view.FindViewById<TextView>(Resource.Id.eventName).Text = TextFormatter.ShortenText(Items[position].Name, 35);
            view.FindViewById<TextView>(Resource.Id.eventRSVPs).Text = Items[position].YesRSVPCount.ToString();

            return view;
        }

        public void UpdateListView()
        {
            NotifyDataSetChanged();
        }

    }
}