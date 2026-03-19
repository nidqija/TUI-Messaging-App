using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Model;

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
            if (username == null || password == null)
            {
                Console.WriteLine("Please fill in all fields.");
                return false;
            }
            Console.WriteLine($"Handling sign in for username: {username}");
          
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {

                UserModel userModel = new UserModel();
                if (userModel.validateUser(username, password))
                {
                    Console.WriteLine("Login Successful!");
                    return true;
                }
                  return false;
            }
            else
            {
                Console.WriteLine("Invalid credentials. Please try again.");
                return false;
            }
        }


        public bool handleSignOut()
        {


            return true;
        }
    }
}
