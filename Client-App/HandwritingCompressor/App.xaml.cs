using HandwritingCompressor.Modules;
using HandwritingsCompressor.Modules;
using System.Configuration;
using System.Data;
using System.Windows;

namespace HandwritingCompressor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var keyValidator = new KeyValidator();
            //validate key
            var key = ProductKeyManager.Get();
            bool isDemo = true;
            if(!string.IsNullOrEmpty(key))
                isDemo = !keyValidator.Validate(key);

            // Create an instance of your main window
            MainWindow mainWindow = new MainWindow(isDemo);
            //MainWindow mainWindow = new MainWindow(false);


            // Optionally set properties on mainWindow before showing it
            mainWindow.Title = "My Custom Main Window";

            // Show the main window
            mainWindow.Show();
        }
    }

}
