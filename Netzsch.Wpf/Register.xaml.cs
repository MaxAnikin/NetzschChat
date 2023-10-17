using System.Windows;
using Netzsch.Client;
using Netzsch.Models;

namespace Netzsch.Wpf;

public partial class Register : Window
{
    public Register()
    {
        InitializeComponent();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private async void OkButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (txtPassword.Password != txtRepeatPassword.Password)
        {
            System.Windows.MessageBox.Show($"Passwords do not match", "Info");
            return;
        }

        var user = new User() { Email = txtEmail.Text, Name = txtName.Text, Password = txtPassword.Password };
        await NetzschChatClient.Instance.CreateUser(user);
        await NetzschChatClient.Instance.Login(user.Email, user.Password);

        var mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}