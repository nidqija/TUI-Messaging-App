using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// used to store the session information of the user after they log in,
// such as their user ID, username, and email.
// This allows the application to keep track of the currently logged-in user and
// provide personalized experiences based on their information.

namespace TUI_Messaging_App.TUI_Messaging_App.Services
{
    internal class SessionInitializer
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string Email { get; set; }

        public static bool isLoggedIn { get; private set; }


        public static void Login ( int id, string username, string email) {
            UserID = id;
            Username = username;
            Email = email;
            isLoggedIn = true;
        }

         public static void Logout()
        {
            UserID = 0;
            Username = null;
            Email = null;
            isLoggedIn = false;
        }




    }

}

