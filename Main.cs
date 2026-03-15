
using TUI_Messaging_App.TUI_Messaging_App.View;


namespace TUI_Messaging_App.TUI_Messaging_App
{
    public class MainProgram
    {
        public static void Main(string[] args)
        {
            HomeView homeView = new HomeView();
            homeView.displayHomeView();

        }
    }
}
