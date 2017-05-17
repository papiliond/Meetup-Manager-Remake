
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Models
{
    public class Self
    {
        [JsonProperty("common")]
        public Common Common { get; set; }
    }
}