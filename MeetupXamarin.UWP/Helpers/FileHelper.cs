using MeetupXamarin.Core.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MeetupXamarin.UWP.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }
    }
}
