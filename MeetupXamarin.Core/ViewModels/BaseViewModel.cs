using MeetupXamarin.Core.Interfaces;
using System;
using MeetupXamarin.Core.Interfaces.Database;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Windows.Input;
using MeetupXamarin.Core.Commanding;
using System.Threading.Tasks;

namespace MeetupXamarin.Core.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        internal readonly IMeetupService meetupService;
        internal readonly IMessageDialog messageDialog;
        internal readonly Random random;
        internal readonly IDataService dataService;
        internal readonly INavigationService navigationService;

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel ()
        {
            meetupService = IoC.Resolve<IMeetupService>();
            messageDialog = IoC.Resolve<IMessageDialog>();
            dataService = IoC.Resolve<IDataService>();
            navigationService = IoC.Resolve<INavigationService>();
            random = new Random();
        }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref canLoadMore, value);
                IsBusyChanged?.Invoke(isBusy);
            }
        }

        public Action<bool> IsBusyChanged { get; set; }

        private bool canLoadMore = false;
        public bool CanLoadMore
        {
            get { return canLoadMore; }
            set
            {
                SetProperty(ref canLoadMore, value);
            }
        }


        Command refreshCommand;

        public Action ExecutedRefreshCommand { get; set; }

        public ICommand RefreshCommand
        {
            get
            {
                return refreshCommand ?? (refreshCommand = new Command(() => 
                {
                    ExecutedRefreshCommand?.Invoke();
                    ExecuteRefreshCommand(); }
                ));
            }
        }

        public virtual void ExecuteRefreshCommand() { }


        Command loadMoreCommand;

        public Action<int> FinishedFirstLoad { get; set; }

        public ICommand LoadMoreCommand
        {
            get { return loadMoreCommand ?? (loadMoreCommand = new Command(async () => await ExecuteLoadMoreCommand())); }
        }

        protected virtual async Task ExecuteLoadMoreCommand()
        {
            await Task.FromResult(default(object));
        }


        public ICommand NavigateToAboutCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (isBusy)
                        return;
                    navigationService.Navigate<AboutViewModel>();
                });
            }
        }

        protected void SetProperty<T>(ref T backingStore,
            T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyName);
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
