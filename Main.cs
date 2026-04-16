using System;
using TUI_Messaging_App.TUI_Messaging_App.Router;
using TUI_Messaging_App.TUI_Messaging_App.Services;
using TUI_Messaging_App.TUI_Messaging_App.View;



namespace TUI_Messaging_App.TUI_Messaging_App
{
    public class MainProgram
    {
        public static async Task Main(string[] args)
        {

            DatabaseService dbService = new DatabaseService();
            dbService.initializeDB();

            var ollamaService = new AskOllamaServices();
            ollamaService.Start();

            var client = new HttpClient();

            try
            {
                var test = await client.GetStringAsync("http://localhost:42617/health");
                Console.WriteLine($"Docker Connection: Success! {test}");
                System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds to allow the container to fully initialize
            }
            catch
            {
                Console.WriteLine("Docker Connection: Failed. Check if the container is up.");
                System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds before proceeding, even if the connection fails
            }


            if (args.Length > 0 && args[0] == "initdb")
            {
                dbService.initializeDB();
                Console.WriteLine("Database setup complete. Exiting...");
                return;
            }


            // ============================= initializing the router engine ============================= //

            var router = new RouterEngine();


// ============================= registering routes for pages ============================= //
            var authController = new AuthView();
            var homeController = new HomeView();
            var signInController = new SignInView();
            var signupController = new SignUpView();
            var viewgroupController = new GroupChatView();
            var searchUserController = new SearchUserView();
            var viewRequestsController = new SeeRequestsView();
            var seeContactsController = new SeeContactsView();
            var chatwithContactController = new ChatwithContactView();
            var createGroupChatController = new CreateGroupView();
            var chatInGroupController = new ChatInGroupView();


            router.RegisterRoute("logout", authController.authView);
            router.RegisterRoute("home" , homeController.displayHomeView);
            router.RegisterRoute("signin" , signInController.signInView);
            router.RegisterRoute("signup" , signupController.signUpView);
            router.RegisterRoute("view groups" , viewgroupController.groupChatView);
            router.RegisterRoute("search users", searchUserController.searchUserView);
            router.RegisterRoute("view requests", viewRequestsController.seeRequestsView );
            router.RegisterRoute("view contacts", seeContactsController.seeContactView );
            router.RegisterRoute("chat with contact", chatwithContactController.chatwithContactView );
            router.RegisterRoute("create group chat", createGroupChatController.createGroupView);
            router.RegisterRoute("chat in group", chatInGroupController.chatInGroupView);




            // ============================= starting the application ============================= //
            router.Run("logout");

            }





    







        }
    }

