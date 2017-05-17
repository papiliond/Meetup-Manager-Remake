
using System;
using MeetupXamarin.Core.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Services.Responses
{
	public class RSVPsRootObject
	{
		public RSVPsRootObject ()
		{
		}
		[JsonProperty("results")]
		public List<RSVP> RSVPs { get; set; }

		[JsonProperty("meta")]
		public Meta Metadata { get; set; }
	}
}

