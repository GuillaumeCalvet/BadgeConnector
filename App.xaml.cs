using System.Windows;
using Firebase.Database;
using Firebase.Database.Query;
using BadgeConnector.ViewModel;
using System.Threading.Tasks;

namespace BadgeConnector
{
    public partial class App : Application
    {
        public static FirebaseClient FirebaseClient;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            FirebaseClient = new FirebaseClient(
                "https://dotnet-8a9fb-default-rtdb.europe-west1.firebasedatabase.app/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("your-firebase-auth-token") // Replace with actual token management
                });

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel();
            mainWindow.Show();
        }
    }
}