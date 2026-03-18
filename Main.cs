
using TUI_Messaging_App.TUI_Messaging_App.Router;
using TUI_Messaging_App.TUI_Messaging_App.Services;
using TUI_Messaging_App.TUI_Messaging_App.View;



namespace TUI_Messaging_App.TUI_Messaging_App
{
    public class MainProgram
    {
        public static void Main(string[] args)
        {

            DatabaseService dbService = new DatabaseService();
            dbService.initializeDB();

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
            var signinController = new SignInView();


            router.RegisterRoute("logout", authController.authView);
            router.RegisterRoute("home" , homeController.displayHomeView);
            router.RegisterRoute("signin" , signinController.signInView);



            // ============================= starting the application ============================= //
            router.Run("home");



    







        }
    }
}
