using Terminal.Gui;
using Retrowarden.Views;

namespace Retrowarden
{
    public class Program
    {
        static void Main(string[] args)
        {
            Application.Init();

            MainView mainView = new MainView(true);
            
            // Run the application loop.
            Application.Run(mainView);
            mainView.Dispose();
            Application.Shutdown();
        }
    }
}