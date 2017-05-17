using MeetupXamarin.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupXamarin.Core.Interfaces
{
    public interface INavigationService
    {
        void Navigate<T> (object[] args = null) where T : BaseViewModel;
        void NavigateBack();

    }
}
