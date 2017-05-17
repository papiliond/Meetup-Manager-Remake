
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Models
{
    public class Organizer
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("member_id")]
        public int MemberId { get; set; }
    }
}
