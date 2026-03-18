
using TUI_Messaging_App.TUI_Messaging_App.Router;
using TUI_Messaging_App.TUI_Messaging_App.View;



namespace TUI_Messaging_App.TUI_Messaging_App
{
    public class MainProgram
    {
        public static void Main(string[] args)
        {


// ============================= initializing the router engine ============================= //

            var router = new RouterEngine();

// ============================= registering routes for pages ============================= //
            var authController = new AuthView();
            var homeController = new HomeView();


            router.RegisterRoute("logout", authController.authView);
            router.RegisterRoute("home" , homeController.displayHomeView);



// ============================= starting the application ============================= //
            router.Run("home");

    







        }
    }
}
