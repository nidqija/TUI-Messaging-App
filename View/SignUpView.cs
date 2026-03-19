using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUI_Messaging_App.TUI_Messaging_App.Controller;

namespace TUI_Messaging_App.TUI_Messaging_App.View
{
    internal class SignUpView
    {
        public string signUpView() {
            Console.WriteLine("Welcome to the Sign Up Page!");
            Console.WriteLine("Please enter your details to create an account.");

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            UserController userController = new UserController();

            if (userController.handleSignUp(username, password, email))
            {
                Console.WriteLine("Sign Up successful! You can now sign in.");
                return "signin";
            }


            else
            {
                Console.WriteLine("Sign Up failed. Please try again.");

                Console.WriteLine($"You entered - Username: {username}, Email: {email}, Password: {password}");

                return "signup";
            }
        }
        }
    }

