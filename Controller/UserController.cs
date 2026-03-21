using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Model;
using TUI_Messaging_App.TUI_Messaging_App.Services;

namespace TUI_Messaging_App.TUI_Messaging_App.Controller
{
    internal class UserController
    {


        public bool handleSignUp(String username, String password, String email)
        {
            if (username == null || password == null || email == null)
            {
                Console.WriteLine("Please fill in all fields.");
                return false;
            }

            if (!email.Contains("@"))
            {
                Console.WriteLine("Please enter a valid email address.");
                return false;
            }


            Console.WriteLine($"Handling sign up for username: {username}, email: {email}");
            UserModel userModel = new UserModel();


            if (userModel.createUser(username, password, email))
            {
                Console.WriteLine("User created successfully!");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to create user. Please try again.");
                return false;
            }

        }


        public bool handleSignIn(String username, String password)
        {
           UserModel userModel = new UserModel();
            var user = userModel.validateUser(username, password);
            if (user != null)
            {
                Console.WriteLine($"User {username} logged in successfully!");
                SessionInitializer.Login(user.id, user.username, user.email);
                return true;
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.");
                return false;
            }
        }


        public bool handleSignOut()
        {
            SessionInitializer.Logout();
            Console.WriteLine("User logged out successfully!");
            return true;
        }
    }
}
