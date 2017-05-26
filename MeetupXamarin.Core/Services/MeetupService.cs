using System;
using System.Threading.Tasks;
using MeetupXamarin.Core.Interfaces;
using MeetupXamarin.Core.Services.Responses;
using MeetupXamarin.Core.Models;
using MeetupXamarin.Core.Services.Request;
using System.Net.Http;
using MeetupXamarin.Core.Helpers;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MeetupXamarin.Core.Services
{
    public class MeetupService : IMeetupService
    {
        readonly IMessageDialog messageDialog;

        public MeetupService()
        {
            messageDialog = IoC.Resolve<IMessageDialog>();
        }

        HttpClient CreateClient()
        {
            return new HttpClient(new ModernHttpClient.NativeMessageHandler());
        }

        #region Id's and Url's

        public static string ClientId = "bb3t1a30l09lctvj6v809lgtr8";
        public static string ClientSecret = "736l0butl9j2uvt3kfdm2i499t";
        public static string AuthorizeUrl = "https://secure.meetup.com/oauth2/authorize";
        public static string RedirectUrl = "http://www.refractored.com/login_success.html";
        public static string AccessTokenUrl = "https://secure.meetup.com/oauth2/access";

        const string GetEventsUrl = @"https://api.meetup.com/2/events?offset={0}&group_id={1}&page=100&status=upcoming,past&desc=true&access_token={2}&only=name,id,time,yes_rsvp_count";
        const string GetGroupsUrl = @"https://api.meetup.com/2/groups?offset={0}&member_id={1}&page=100&order=name&access_token={2}&only=name,id,group_photo,members";
        const string GetGroupsOrganizerUrl = @"https://api.meetup.com/2/groups?offset={0}&organizer_id={1}&page=100&order=name&access_token={2}&only=name,id,group_photo,members";
        const string GetRSVPsUrl = @"https://api.meetup.com/2/rsvps?offset={0}&event_id={1}&page=100&order=name&rsvp=yes&access_token={2}&only=member,member_photo,guests";
        const string GetUserUrl = @"https://api.meetup.com/2/member/self?access_token={0}&only=name,id,photo";

        #endregion

        public async Task<GroupsRootObject> GetGroups(string memberId, int skip)
        {
            var offset = skip / 100;
            if (!await RenewAccessToken())
            {
                messageDialog.SendToast("Unable to get groups, please re-login.");
                return new GroupsRootObject { Groups = new List<Group>() };
            }

            using (var client = CreateClient())
            {
                if (client.DefaultRequestHeaders.CacheControl == null)
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();

                client.DefaultRequestHeaders.CacheControl.NoCache = true;
                client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                client.DefaultRequestHeaders.CacheControl.NoStore = true;
                client.Timeout = new TimeSpan(0, 0, 30);
                var request = string.Format(Settings.OrganizerMode ? GetGroupsOrganizerUrl : GetGroupsUrl, offset, memberId, Settings.AccessToken);

                var response = await client.GetStringAsync(request).ConfigureAwait(false);
               
                return await DeserializeObjectAsync<GroupsRootObject>(response);
            }
        }

        public async Task<EventsRootObject> GetEvents(string groupId, int skip)
        {
            var offset = skip / 100;
            if (!await RenewAccessToken())
            {
                messageDialog.SendToast("Unable to get events, please re-login.");
                return new EventsRootObject() { Events = new List<Event>() };
            }

            using (var client = CreateClient())
            {
                if (client.DefaultRequestHeaders.CacheControl == null)
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();

                client.DefaultRequestHeaders.CacheControl.NoCache = true;
                client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                client.DefaultRequestHeaders.CacheControl.NoStore = true;
                client.Timeout = new TimeSpan(0, 0, 30);

                var request = string.Format(GetEventsUrl, offset, groupId, Settings.AccessToken);
                if (!Settings.ShowAllEvents)
                    request += "&time=-100m,2m";

                var response = await client.GetStringAsync(request);
                return await DeserializeObjectAsync<EventsRootObject>(response);
            }       
        }

        public async Task<RSVPsRootObject> GetRSVPs(string eventId, int skip)
        {
            var offset = skip / 100;
            if (!await RenewAccessToken())
            {
                messageDialog.SendToast("Unable to get RSVPs, please re-login.");
                return new RSVPsRootObject() { RSVPs = new List<RSVP>() };
            }

            using (var client = CreateClient())
            {
                if (client.DefaultRequestHeaders.CacheControl == null)
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();

                client.DefaultRequestHeaders.CacheControl.NoCache = true;
                client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                client.DefaultRequestHeaders.CacheControl.NoStore = true;
                client.Timeout = new TimeSpan(0, 0, 30);
                var request = string.Format(GetRSVPsUrl, offset, eventId, Settings.AccessToken);
                var response = await client.GetStringAsync(request);
                return await DeserializeObjectAsync<RSVPsRootObject>(response);
            }
        }

        public async Task<LoggedInUser> GetCurrentMember()
        {
            if (!await RenewAccessToken())
            {
                messageDialog.SendToast("Unable to get current member.");
                return new LoggedInUser();
            }

            using (var client = CreateClient())
            {
                if (client.DefaultRequestHeaders.CacheControl == null)
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();

                client.DefaultRequestHeaders.CacheControl.NoCache = true;
                client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                client.DefaultRequestHeaders.CacheControl.NoStore = true;
                client.Timeout = new TimeSpan(0, 0, 30);
                var request = string.Format(GetUserUrl, Settings.AccessToken);
                var response = await client.GetStringAsync(request);

                //should use async, but has issue for some reason and throws exception (Original author)
                return JsonConvert.DeserializeObject<LoggedInUser>(response);
            }
        }

        public async Task<RequestTokenObject> GetToken(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            using (var client = CreateClient())
            {
                try
                {
                    if (client.DefaultRequestHeaders.CacheControl == null)
                        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();

                    client.DefaultRequestHeaders.CacheControl.NoCache = true;
                    client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                    client.DefaultRequestHeaders.CacheControl.NoStore = true;
                    client.Timeout = new TimeSpan(0, 0, 30);

                    var content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("client_id", ClientId),
                            new KeyValuePair<string, string>("client_secret", ClientSecret),
                            new KeyValuePair<string, string>("grant_type", "authorization_code"),
                            new KeyValuePair<string, string>("redirect_uri", RedirectUrl),
                            new KeyValuePair<string, string>("code", code),
                        });

                    var result = await client.PostAsync("https://secure.meetup.com/oauth2/access", content);
                    var response = await result.Content.ReadAsStringAsync();
                    var refreshResponse = await DeserializeObjectAsync<RequestTokenObject>(response);
                    return refreshResponse;
                }
                catch (Exception ex)
                {
                    if (Settings.Insights)
                        Xamarin.Insights.Report(ex);
                }
            }

            return null;
        }

        public async Task<bool> RenewAccessToken()
        {
            if (string.IsNullOrWhiteSpace(Settings.AccessToken))
                return false;

            if (DateTime.UtcNow < new DateTime(Settings.KeyValidUntil))
                return true;

            using (var client = CreateClient())
            {
                try
                {
                    if (client.DefaultRequestHeaders.CacheControl == null)
                        client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();

                    client.DefaultRequestHeaders.CacheControl.NoCache = true;
                    client.DefaultRequestHeaders.IfModifiedSince = DateTime.UtcNow;
                    client.DefaultRequestHeaders.CacheControl.NoStore = true;
                    client.Timeout = new TimeSpan(0, 0, 30);

                    var content = new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("client_id", ClientId),
                        new KeyValuePair<string, string>("client_secret", ClientSecret),
                        new KeyValuePair<string, string>("grant_type", "refresh_token"),
                        new KeyValuePair<string, string>("refresh_token", Settings.RefreshToken),
                    });

                    var result = await client.PostAsync("https://secure.meetup.com/oauth2/access", content);
                    var response = await result.Content.ReadAsStringAsync();
                    var refreshResponse = await DeserializeObjectAsync<RefreshRootObject>(response);
                    Settings.AccessToken = refreshResponse.AccessToken;
                    var nextTime = DateTime.UtcNow.AddSeconds(refreshResponse.ExpiresIn).Ticks;
                    Settings.KeyValidUntil = nextTime;
                    Settings.RefreshToken = refreshResponse.RefreshToken;
                }
                catch (Exception ex)
                {
                    if (Settings.Insights)
                        Xamarin.Insights.Report(ex);
                    return false;
                }
            }

            return true;
        }

        public Task<T> DeserializeObjectAsync<T>(string value)
        {
            return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value));
        }
    }
}
