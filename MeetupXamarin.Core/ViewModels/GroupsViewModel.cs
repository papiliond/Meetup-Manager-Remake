using MeetupXamarin.Core.Commanding;
using MeetupXamarin.Core.Helpers;
using MeetupXamarin.Core.Interfaces;
using MeetupXamarin.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetupXamarin.Core.ViewModels
{
    public class GroupsViewModel : BaseViewModel
    {

        public GroupsViewModel()
        {
            groups = new ObservableCollection<Group>();
        }

        private ObservableCollection<Group> groups;
        public ObservableCollection<Group> Groups
        {
            get { return groups; }
            set { SetProperty(ref groups, value); }
        }

        public override void ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            groups.Clear();
            OnPropertyChanged("Groups");
            CanLoadMore = true;
            this.LoadMoreCommand.Execute(null);
        }

        protected override async Task ExecuteLoadMoreCommand()
        {
            if (!CanLoadMore || IsBusy)
                return;

            IsBusy = true;

            var index = Groups.Count == 0 ? 0 : Groups.Count - 1;
            try
            {
                var groupResults = await meetupService.GetGroups(Settings.UserId, groups.Count);
                foreach (var group in groupResults.Groups)
                {
                    if (group.GroupPhoto == null)
                    {
                        group.GroupPhoto = new GroupPhoto
                        {
                            PhotoId = 0,
                            HighResLink = "http://refractored.com/default.png",
                            PhotoLink = "http://refractored.com/default.png",
                            ThumbLink = "http://refractored.com/default.png"
                        };
                    }
                    Groups.Add(group);
                }

                OnPropertyChanged("Groups");
                CanLoadMore = groupResults.Groups.Count == 100;

                if (Groups.Count == 0)
                    messageDialog.SendToast("You do not have any groups.");
            }
            catch(Exception e)
            {
                if (Settings.Insights)
                    Xamarin.Insights.Report(e);
            } finally
            {
                FinishedFirstLoad?.Invoke(index);
                IsBusy = false;
            }
        }

        private void GoToAbout()
        {
            //TODO: Navigate
            //ShowViewModel<AboutViewModel> ();
        }

        Command<Group> goToGroupCommand;

        public ICommand GoToGroupCommand
        {
            get
            {
                return goToGroupCommand ?? (goToGroupCommand = new Command<Group>(g => ExecuteGoToGroupCommand(g)));
            }
        }

        void ExecuteGoToGroupCommand(Group @group)
        {
            if (IsBusy)
                return;

            IoC.Resolve<INavigationService>().Navigate<EventsViewModel>(new object[] { @group });
        }

    }

}
