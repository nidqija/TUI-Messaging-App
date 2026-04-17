using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace TUI_Messaging_App.TUI_Messaging_App.Services
{

    public class CalendarEvent
    {
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    internal class GoogleCalendar
    {
        public async Task AddGoogleEventAsync(string groupId, CalendarEvent details)
        {
            // Token is stored per user so each person authorizes once.
            // On first call a browser window opens for the user to sign in with Google.
            // Subsequent calls reuse the saved token silently.
            string tokenPath = Path.Combine("Tokens", SessionInitializer.Username);

            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { CalendarService.Scope.CalendarEvents },
                    SessionInitializer.Username,
                    CancellationToken.None,
                    new FileDataStore(tokenPath, true)
                );
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "TUI-Messaging-App"
            });

            // Google Calendar requires IANA timezone IDs. On Windows, TimeZoneInfo.Local.Id returns
            // Windows-style IDs, so we convert using the built-in TZConvert mapping.
            string ianaTimeZone;
            try
            {
                ianaTimeZone = TimeZoneInfo.Local.HasIanaId
                    ? TimeZoneInfo.Local.Id
                    : TimeZoneInfo.TryConvertWindowsIdToIanaId(TimeZoneInfo.Local.Id, out string ianaId)
                        ? ianaId
                        : "UTC";
            }
            catch
            {
                ianaTimeZone = "UTC";
            }

            var newEvent = new Event()
            {
                Summary = details.title,
                Start = new EventDateTime
                {
                    DateTime = details.start,
                    TimeZone = ianaTimeZone
                },
                End = new EventDateTime
                {
                    DateTime = details.end,
                    TimeZone = ianaTimeZone
                }
            };

            await service.Events.Insert(newEvent, "primary").ExecuteAsync();
        }
    }
}
