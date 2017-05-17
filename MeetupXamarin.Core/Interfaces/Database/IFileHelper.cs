using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupXamarin.Core.Interfaces.Database
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
