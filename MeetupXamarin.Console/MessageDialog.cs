using MeetupXamarin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupXamarin.ConsoleApp
{
    class MessageDialog : IMessageDialog
    {
        public void AskForString(string message, string title, Action<string> returnString)
        {
            throw new NotImplementedException();
        }

        public void SendConfirmation(string message, string title, Action<bool> confirmationAction)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message, string title = null)
        {
            throw new NotImplementedException();
        }

        public void SendToast(string message)
        {
            System.Console.Write(message);
        }
    }
}
