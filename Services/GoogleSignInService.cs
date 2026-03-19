using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*using Google.Apis.Auth.OAuth2;
using Google.Apis.Services; */


namespace TUI_Messaging_App.TUI_Messaging_App.Services
{
    internal class GoogleSignInService
    {

        public string[] linkscopes =
        {
            "https://www.googleapis.com/auth/userinfo.email",
            "https://www.googleapis.com/auth/userinfo.profile"
        };


       public async Task<bool> SignInWithGoogleAsync()
        {
            
            Console.WriteLine("Redirecting to Google Sign-In...");

            await Task.Delay(2000); 
            Console.WriteLine("Google Sign-In successful!");
            return true;
        }
    }
}
