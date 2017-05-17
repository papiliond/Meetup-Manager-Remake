// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MeetupXamarin.Core.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

        #region Setting Constants

        const string AccessTokenKey = "access_token";
        static string AccessTokenDefault = string.Empty;

        const string RefreshTokenKey = "refresh_token";
        static string RefreshTokenDefault = string.Empty;

        const string KeyValidUntilKey = "key_valid";
        const long KeyValidUntilDefault = 0;

        const string InsightsKey = "insights";
        const bool InsightsDefault = true;

        const string ShowAllEventsKey = "show_all_events";
        const bool ShowAllEventsDefault = false;

        const string UserIdKey = "user_id";
        static string UserIdDefault = string.Empty;

        const string UserNameKey = "user_name";
        static string UserNameDefault = string.Empty;

        const string OrganizerModeKey = "organizer_mode";
        const bool OrganizerModeDefault = false;

        #endregion

        #region Properties

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(AccessTokenKey, AccessTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(AccessTokenKey, value);
            }
        }

        public static string RefreshToken
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(RefreshTokenKey, RefreshTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(RefreshTokenKey, value);
            }
        }

        public static bool Insights
        {
            get
            {
                //return AppSettings.GetValueOrDefault<bool>(InsightsKey, InsightsDefault);
                return false;
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(InsightsKey, value);
            }
        }

        public static bool ShowAllEvents
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(ShowAllEventsKey, ShowAllEventsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(ShowAllEventsKey, value);
            }
        }

        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(UserIdKey, UserIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(UserIdKey, value);
            }
        }

        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(UserNameKey, UserNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(UserNameKey, value);
            }
        }

        public static long KeyValidUntil
        {
            get
            {
                return AppSettings.GetValueOrDefault<long>(KeyValidUntilKey, KeyValidUntilDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<long>(KeyValidUntilKey, value);
            }
        }

        public static bool OrganizerMode
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(OrganizerModeKey, OrganizerModeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(OrganizerModeKey, value);
            }
        }

        #endregion

    }
}