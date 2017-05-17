
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Models
{
    public class OtherServices
    {
        [JsonProperty("twitter")]
        public Twitter Twitter { get; set; }
        [JsonProperty("tumblr")]
        public Tumblr Tumblr { get; set; }
    }
}