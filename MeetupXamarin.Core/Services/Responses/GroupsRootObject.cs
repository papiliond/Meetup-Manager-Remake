
using System.Collections.Generic;
using MeetupXamarin.Core.Models;
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Services.Responses
{
    public class GroupsRootObject
    {
        [JsonProperty("results")]
        public List<Group> Groups { get; set; }
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }
}
