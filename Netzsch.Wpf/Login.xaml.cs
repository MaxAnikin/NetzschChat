using System.Windows;
using Netzsch.Client;
using Netzsch.Models;

namespace Netzsch.Wpf;

public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();
    }

    private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!await NetzschChatClient.Instance.Login(txtEmail.Text.Trim(), txtPassword.Password.Trim()))
        {
            MessageBox.Show($"Authentication failed", "Login");
            return;
        }

        var mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
    {
        var registerForm = new Register();
        registerForm.Show();
        Close();
    }
}