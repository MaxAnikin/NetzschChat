using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using Netzsch.Client;
using Netzsch.Models;

namespace Netzsch.Wpf;

public partial class Chat : UserControl
{
    private Task _timerTask;
    private readonly User _user;
    private readonly PeriodicTimer _refreshUsersTimer = new(TimeSpan.FromMilliseconds(Constants.RefreshPeriodMs));

    public string UserEmail => _user.Email;
    public event EventHandler OnClose;

    public Chat(User user)
    {
        _user = user;

        InitializeComponent();
        InitializeBackgroundRefresh();
    }

    private void InitializeBackgroundRefresh()
    {
        _timerTask = Task.Factory.StartNew(async () =>
        {
            try
            {
                while (true)
                {
                    await _refreshUsersTimer.WaitForNextTickAsync();
                    await RefreshMessages();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }

    private async Task RefreshMessages()
    {
        var messages = await NetzschChatClient.Instance.GetLastMessages(_user.Email);
        Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() =>
            {
                lstChat.ItemsSource = messages?.Select(x =>
                        new MessageListItem(x.CreatedDate.ToString("s"), x.FromEmail, x.ToEmail, x.Text))
                    .OrderBy(x => x.CreatedDate).ToList();
                
                if (VisualTreeHelper.GetChildrenCount(lstChat) > 0)
                {
                    Border border = (Border)VisualTreeHelper.GetChild(lstChat, 0);
                    ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                    scrollViewer.ScrollToBottom();
                }
            }));
    }

    protected override async void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        await RefreshMessages();
    }

    private void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        OnOnClose();
    }

    protected virtual void OnOnClose()
    {
        OnClose?.Invoke(this, EventArgs.Empty);
    }

    private async void BtnMessage_OnClick(object sender, RoutedEventArgs e)
    {
        await NetzschChatClient.Instance.SendMessage(_user.Email, txtMessage.Text);
        txtMessage.Clear();
    }
}

internal record MessageListItem(string CreatedDate, string From, string To, string Message);