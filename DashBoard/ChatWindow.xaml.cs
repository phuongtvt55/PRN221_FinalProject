using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DashBoard
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        HubConnection hubConnection;
        public ChatWindow()
        {
            InitializeComponent();
            hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:44311/chathub").Build();
            hubConnection.Closed += HubConnection_Closed;
        }

        private async Task HubConnection_Closed(Exception arg)
        {
            await Task.Delay(new Random().Next(0, 5) * 100);
            await hubConnection.StartAsync();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var newMessage = $"{user}: {message}";
                lb.Items.Add(newMessage);
            });
            try
            {
                await hubConnection.StartAsync();
                lb.Items.Add("Connection started.");
            }
            catch (Exception ex)
            {
                lb.Items.Add(ex.Message);
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await hubConnection.InvokeAsync("SendMessage", "Admin", txtMess.Text);
            }
            catch (Exception ex) { }
        }
    }
}
