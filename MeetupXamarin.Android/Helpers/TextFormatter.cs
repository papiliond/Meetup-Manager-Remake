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

namespace MeetupXamarin.Android.Helpers
{
    public static class TextFormatter
    {
        public static string ShortenText(string str, int maxLength)
        {
            if (str.Length > maxLength)
            {
                str = str.Remove(str.Length - (str.Length - maxLength));
                str = str + "...";
            }

            return str;
        }

    }
}