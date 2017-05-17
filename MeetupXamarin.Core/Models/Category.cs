﻿
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Models
{
    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("shortname")]
        public string Shortname { get; set; }
    }

}
