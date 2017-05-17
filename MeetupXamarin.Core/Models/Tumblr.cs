
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Models
{
    public class Tumblr
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }
}