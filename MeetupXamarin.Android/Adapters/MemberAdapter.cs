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
using MeetupXamarin.Android.Activities;
using Android.Util;

namespace MeetupXamarin.Android.Adapters
{
    class MemberAdapter : BaseAdapter<MemberViewModel>, CompoundButton.IOnCheckedChangeListener
    {
        ObservableCollection<MemberViewModel> Items { get; set; }
        ObservableCollection<MemberViewModel> FilteredItems { get; set; }

        EventActivity Activity;

        public MemberAdapter (Activity activity, ObservableCollection<MemberViewModel> items)
        {
            Activity = (EventActivity)activity;
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
            var checkBox = view.FindViewById<CheckBox>(Resource.Id.checkIn_checkbox);

            checkBox.SetOnCheckedChangeListener(null);

            var memberPhotoBitmap = ImageFormatter.GetBitmapFromUrl(FilteredItems[position].ThumbLink);
            view.FindViewById<RoundedImageView>(Resource.Id.thumblinkPhoto).SetImageBitmap(memberPhotoBitmap);
            view.FindViewById<TextView>(Resource.Id.memberName).Text = TextFormatter.ShortenText(FilteredItems[position].Name, 18);
            checkBox.Checked = FilteredItems[position].CheckedIn;
            checkBox.Tag = position;
            checkBox.SetOnCheckedChangeListener(this);

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

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            var position = (int)buttonView.Tag;
            Activity.MemberCheckedIn?.Invoke(position);
        }

    }
}