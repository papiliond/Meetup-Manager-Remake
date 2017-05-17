using MeetupXamarin.Core;
using MeetupXamarin.Core.Models;
using System.Collections.Generic;
using MeetupXamarin.Core.Interfaces;
using MeetupXamarin.Core.Services;
using MeetupXamarin.Core.Services.Responses;

namespace MeetupXamarin.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IoC.Register<IMessageDialog>(new MessageDialog());
            IoC.Register<IMeetupService>(new MeetupService());
            IMeetupService meetupService = IoC.Resolve<IMeetupService>();
        }
    }
}
