using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.ObjectModel;
using MeetupXamarin.Core.ViewModels;
using MeetupXamarin.Android.Helpers;
using ImageViews.Rounded;

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
            var view = convertView ?? Activity.LayoutInflater.Inflate(Resource.Layout.memberlist_item, parent, false);

            var memberPhotoBitmap = ImageFormatter.GetBitmapFromUrl(FilteredItems[position].ThumbLink);
            view.FindViewById<RoundedImageView>(Resource.Id.thumblinkPhoto).SetImageBitmap(memberPhotoBitmap);
            view.FindViewById<TextView>(Resource.Id.memberName).Text = FilteredItems[position].Name;
            view.FindViewById<CheckBox>(Resource.Id.checkIn_checkbox).Checked = FilteredItems[position].CheckedIn;

            return view;
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