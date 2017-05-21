using Android.App;
using Android.Views;
using Android.Widget;
using MeetupXamarin.Core.Models;
using ImageViews.Rounded;
using System.Collections.ObjectModel;
using MeetupXamarin.Android.Helpers;

namespace MeetupXamarin.Android.Adapters
{
    public class GroupAdapter : BaseAdapter<Group>
    {
        public ObservableCollection<Group> Items { get; set; }
        private Activity activity;

        public GroupAdapter (Activity activity, ObservableCollection<Group> items)
        {
            this.activity = activity;
            Items = items;
        }

        #region BaseAdapter implementation
        public override Group this[int position]
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
            return Items[position].Id;
        }
        #endregion

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.grouplist_item, parent, false);

            var groupPhotoBitmap = ImageFormatter.GetBitmapFromUrl(Items[position].PhotoLink);
            view.FindViewById<RoundedImageView>(Resource.Id.groupPhoto).SetImageBitmap(groupPhotoBitmap);

            var groupName = TextFormatter.ShortenText(Items[position].Name, 28);
            view.FindViewById<TextView>(Resource.Id.groupName).Text = groupName;
            view.FindViewById<TextView>(Resource.Id.memberCount).Text = Items[position].Members.ToString();

            return view;
        }

        public void UpdateListView ()
        {
            NotifyDataSetChanged();
        }





    }
}