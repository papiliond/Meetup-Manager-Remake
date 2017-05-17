using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using MeetupXamarin.Core.Interfaces;
using MeetupXamarin.Core;
using MeetupXamarin.Navigation;

namespace MeetupXamarin.Android.Services
{
    public class MessageDialog : IMessageDialog
    {

        public static void SendMessage(Activity activity, string message, string title = null)
        {
            var builder = new AlertDialog.Builder(activity);
            builder
            .SetTitle(title ?? string.Empty)
            .SetMessage(message)
            .SetPositiveButton(Resource.String.ok, delegate
            {

            });

            AlertDialog alert = builder.Create();
            alert.Show();
        }

        public void SendMessage(string message, string title = null)
        {
            var activity = IoC.Resolve<ICurrentActivityHolder>().Current;
            var builder = new AlertDialog.Builder(activity);
            builder
                .SetTitle(title ?? string.Empty)
                .SetMessage(message)
                .SetPositiveButton(Resource.String.ok, delegate
                {

                });

            AlertDialog alert = builder.Create();
            alert.Show();
        }

        public void SendToast(string message)
        {
            var activity = IoC.Resolve<ICurrentActivityHolder>().Current;
            activity.RunOnUiThread(() =>
            {
                Toast.MakeText(activity, message, ToastLength.Long).Show();
            });

        }

        public void SendConfirmation(string message, string title, System.Action<bool> confirmationAction)
        {
            var activity = IoC.Resolve<ICurrentActivityHolder>().Current;
            var builder = new AlertDialog.Builder(activity);
            builder
            .SetTitle(title ?? string.Empty)
            .SetMessage(message)
            .SetPositiveButton(Resource.String.ok, delegate
            {
                confirmationAction(true);
            }).SetNegativeButton(Resource.String.cancel, delegate
            {
                confirmationAction(false);
            });

            AlertDialog alert = builder.Create();
            alert.Show();
        }

        public void AskForString(string message, string title, System.Action<string> returnString)
        {
            var activity = IoC.Resolve<ICurrentActivityHolder>().Current;
            var builder = new AlertDialog.Builder(activity);
            builder.SetIcon(Resource.Drawable.ic_launcher);
            builder.SetTitle(title ?? string.Empty);
            builder.SetMessage(message);
            var view = View.Inflate(activity, Resource.Layout.dialog_add_member, null);
            builder.SetView(view);

            var textBoxName = view.FindViewById<EditText>(Resource.Id.name);

            builder.SetCancelable(true);
            builder.SetNegativeButton(Resource.String.cancel, delegate
            {
            });//do nothing on cancel

            builder.SetPositiveButton(Resource.String.ok, delegate
            {

                if (string.IsNullOrWhiteSpace(textBoxName.Text))
                    return;

                returnString(textBoxName.Text.Trim());
            });


            var alertDialog = builder.Create();
            alertDialog.Show();


        }
    }
}