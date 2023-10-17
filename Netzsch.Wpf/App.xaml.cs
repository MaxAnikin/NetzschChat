using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Netzsch.Client;

namespace Netzsch.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException += AppDispatcherUnhandledException;
            base.OnStartup(e);
            NetzschChatClient.Instance.Initialize("https://localhost:7147/api/", CancellationToken.None);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show($"An error occured: {e.Exception.Message}", "Error");
            e.Handled = true;
        }
    }
}