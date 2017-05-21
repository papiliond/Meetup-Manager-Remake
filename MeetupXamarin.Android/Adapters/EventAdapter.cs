using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using MeetupXamarin.Core.Models;
using System.Collections.ObjectModel;
using MeetupXamarin.Android.Helpers;

namespace MeetupXamarin.Android.Adapters
{
    public class EventAdapter : BaseAdapter<Event>
    {
        protected ObservableCollection<Event> Items { get; set; }
        protected ObservableCollection<Event> FilteredItems { get; set; }
        Activity Activity;

        public EventAdapter (Activity activity, ObservableCollection<Event> items)
        {
            Activity = activity;
            Items = items;
            FilteredItems = items;
        }

        public override Event this[int position]
        {
            get
            {
                return FilteredItems[position];
            }
        }

        public override int Count
        {
            get
            {
                return FilteredItems.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return long.Parse(FilteredItems[position].Id);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? Activity.LayoutInflater.Inflate(Resource.Layout.eventlist_item, parent, false);

            view.FindViewById<TextView>(Resource.Id.eventDateMonthAndDay).Text = FilteredItems[position].MonthDay;
            view.FindViewById<TextView>(Resource.Id.eventDateyear).Text = FilteredItems[position].Year;
            view.FindViewById<TextView>(Resource.Id.eventName).Text = TextFormatter.ShortenText(FilteredItems[position].Name, 35);
            view.FindViewById<TextView>(Resource.Id.eventRSVPs).Text = FilteredItems[position].YesRSVPCount.ToString();

            return view;
        }

        public void UpdateListView()
        {
            NotifyDataSetChanged();
        }
        
        public void Filter (string filterText)
        {
            FilteredItems = new ObservableCollection<Event>(GetFilteredItems(filterText));
            NotifyDataSetChanged();
        }

        private List<Event> GetFilteredItems (string filterText)
        {
            if (string.IsNullOrWhiteSpace(filterText))
            {
                return Items.ToList();
            }

            filterText = filterText.ToLower();

            return Items.Select(i => i)
                .Where(i => i.Name.ToLower().Contains(filterText)).ToList();
        }
    }
}