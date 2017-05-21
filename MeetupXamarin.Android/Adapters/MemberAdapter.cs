using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.ObjectModel;
using MeetupXamarin.Core.ViewModels;

namespace MeetupXamarin.Android.Adapters
{
    class MemberAdapter : BaseAdapter<MemberViewModel>
    {
        protected ObservableCollection<MemberViewModel> Items { get; set; }
        protected ObservableCollection<MemberViewModel> FilteredItems { get; set; }
        Activity Activity;

        public MemberAdapter (Activity activity, ObservableCollection<MemberViewModel> items)
        {
            Activity = activity;
            Items = items;
            FilteredItems = items;
        }

        public override MemberViewModel this[int position]
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
            return FilteredItems[position].Member.MemberId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }

        public void UpdateListView()
        {
            NotifyDataSetChanged();
        }

        public void Filter(string filterText)
        {
            FilteredItems = new ObservableCollection<MemberViewModel>(GetFilteredItems(filterText));
            NotifyDataSetChanged();
        }

        private List<MemberViewModel> GetFilteredItems(string filterText)
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