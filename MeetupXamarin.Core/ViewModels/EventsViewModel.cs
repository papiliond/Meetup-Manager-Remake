using MeetupXamarin.Core.Commanding;
using MeetupXamarin.Core.Helpers;
using MeetupXamarin.Core.Interfaces;
using MeetupXamarin.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetupXamarin.Core.ViewModels
{
    public class EventsViewModel : BaseViewModel
    {
        public string GroupName { get; set; }

        string groupId;
        public string GroupId
        {
            get
            {
                return groupId;
            }
            set
            {
                groupId = value;
            }
        }

        public ObservableCollection<Event> Events { get; set; }

        public ObservableCollection<Grouping<string, Event>> EventsGrouped { get; set; }

        public EventsViewModel(Group group) : base()
        {
            GroupName = group.Name;
            groupId = group.Id.ToString();
            Events = new ObservableCollection<Event>();
            EventsGrouped = new ObservableCollection<Grouping<string, Event>>();
        }

        public override void ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            Events.Clear();
            OnPropertyChanged("IsBusy");
            CanLoadMore = true;
            this.LoadMoreCommand.Execute(null);
        }

        protected override async Task ExecuteLoadMoreCommand()
        {
            if (!CanLoadMore || IsBusy)
                return;

            IsBusy = true;
            var index = Events.Count == 0 ? 0 : Events.Count - 1;
            try
            {
                var eventResults = await meetupService.GetEvents(groupId, Events.Count);
                foreach (var e in eventResults.Events)
                {
                    Events.Add(e);
                }

                CanLoadMore = eventResults.Events.Count == 100;

                if (Events.Count == 0)
                    messageDialog.SendToast("There are no events for this group.");

                Sort();
            }
            catch (Exception e)
            {
                if (Settings.Insights)
                    Xamarin.Insights.Report(e);
            }
            finally
            {
                FinishedLoad?.Invoke();
                FinishedFirstLoad?.Invoke(index);
                IsBusy = false;
            }
        }

        private void Sort()
        {
            EventsGrouped.Clear();
            var sorted = from e in Events orderby e.Time descending
                         group e by e.Year into eGroup
                         select new Grouping<string, Event>(eGroup.Key, eGroup);

            foreach (var sort in sorted)
                EventsGrouped.Add(sort);
        }

        Command<Event> goToEventCommand;

        public ICommand GoToEventCommand
        {
            get { return goToEventCommand ?? (goToEventCommand = new Command<Event>(ExecuteGoToEventCommand)); }
        }

        void ExecuteGoToEventCommand(Event @event)
        {
            if (IsBusy)
                return;

            object[] args = new object[] { @event, GroupId, GroupName };

            IoC.Resolve<INavigationService>().Navigate<EventViewModel>(args);
        }

        Command goToStatsCommand;

        public Command GoToStatsCommand
        {
            get { return goToStatsCommand ?? (goToStatsCommand = new Command(ExecuteGoToStatsCommand)); }
        }

        public void ExecuteGoToStatsCommand()
        {
            if (IsBusy)
                return;

            //page.Navigation.PushAsync(new StatisticsView(GroupId, GroupName));
        }


    }
}
