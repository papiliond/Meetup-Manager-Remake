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
using Android.Graphics;
using System.Net;

namespace MeetupXamarin.Android.Helpers
{
    public static class ImageFormatter
    {
        public static Bitmap GetBitmapFromUrl(string url)
        {
            Bitmap image = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                    image = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            }

            return image;
        }
    }
}