
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Models
{
    public class Twitter
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }
}