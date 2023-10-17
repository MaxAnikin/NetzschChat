using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Netzsch.Client;
using Netzsch.Models;

namespace Netzsch.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PeriodicTimer _refreshUsersTimer = new(TimeSpan.FromMilliseconds(Constants.RefreshPeriodMs));
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeBackgroundRefresh();
        }

        private void InitializeBackgroundRefresh()
        {
            Task.Factory.StartNew(async () =>
            {
                while (await _refreshUsersTimer.WaitForNextTickAsync())
                {
                    await RefreshUsersInfo();
                }
            });
        }

        private void lstUsers_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedUser = (lstUsers.SelectedItem as UserListItem);
            if(selectedUser == null)
                throw new Exception("Invalid user item");
            
            bool needNew = true;
            foreach (var tbChatsItem in tbChats.Items)
            {
                if (((tbChatsItem as TabItem)?.Content as Chat)?.UserEmail == selectedUser.User.Email)
                {
                    needNew = false;
                    tbChats.SelectedItem = tbChatsItem;
                }
            }

            if (needNew)
            {
                var tabItem = new TabItem()
                {
                    Header = $"Chat - {selectedUser.User.Name}",
                    IsSelected = true
                };
                
                var chatControl = new Chat(selectedUser.User);
                chatControl.OnClose += (o, args) => { tbChats.Items.Remove(tabItem); };

                tabItem.Content = chatControl;
                
                tbChats.Items.Add(tabItem);
            }
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await RefreshUsersInfo();
        }

        public record UserListItem(User User, string Email, string Name, string OnlineStatus)
        {
            public override string ToString()
            {
                return $"{{ Email = {User.Email}, Name = {User.Name}, OnlineStatus = {OnlineStatus} }}";
            }
        }

        private async Task RefreshUsersInfo()
        {
            var users = await NetzschChatClient.Instance.GetMyUsers(CancellationToken.None);
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                    lstUsers.ItemsSource = users.Select(x => new UserListItem(x, x.Email, x.Name, (x.Online ? "Online" : "Offline"))).ToList() 
                ));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}