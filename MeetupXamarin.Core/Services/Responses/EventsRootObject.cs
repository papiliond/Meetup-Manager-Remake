
using System;
using System.Collections.Generic;
using MeetupXamarin.Core.Models;
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Services.Responses
{
	public class EventsRootObject
	{
		[JsonProperty("results")]
		public List<Event> Events { get; set; }

		[JsonProperty("meta")]
		public Meta Metadata { get; set; }
	}
}

