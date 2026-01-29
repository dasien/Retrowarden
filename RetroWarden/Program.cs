using Terminal.Gui.App;
using Retrowarden.Views;

namespace Retrowarden
{
    public class Program
    {
        static void Main(string[] args)
        {
            IApplication app = Application.Create();
            app.Init();

            try
            {
                MainView mainView = new MainView(app, true);
                app.Run(mainView);
                mainView.Dispose();
            }
            finally
            {
                app.RequestStop();
                app.Dispose();
            }
        }
    }
}