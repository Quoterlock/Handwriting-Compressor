using HandwritingCompressor.Modules;
using HandwritingCompressor.Modules.Interfaces;
using HandwritingsCompressor.Modules;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HandwritingCompressor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<MainWindow>();
            serviceCollection.AddSingleton<IKeyValidator, WebProductKeyValidator>();
            serviceCollection.AddSingleton<ImagesManager>();
            serviceCollection.AddScoped<ITextFileReader, EncryptedFileReader>();
            serviceCollection.AddScoped<IProductKeyManager, ProductKeyManager>();
        }
    }
}
