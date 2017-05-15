using System;
using System.Windows;
using System.Windows.Controls;
using WPFClient.FlightTickets;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FlightTicketsClient client;
        public int clientID = -1;
        FlightSearch searchWindow;
        Reservations reservationsWindow;
        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if(client != null)
            {
                client.Close();
            }
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoginBox.Text != "" && PasswordBox.Password != "")
                {
                    client = new FlightTicketsClient();
                    client.ClientCredentials.UserName.UserName = LoginBox.Text;
                    client.ClientCredentials.UserName.Password = PasswordBox.Password;
                    clientID = await client.GetClientIDAsync(LoginBox.Text, PasswordBox.Password);
                    if (clientID != -1)
                    {
                        searchWindow = new FlightSearch(this);
                        reservationsWindow = new Reservations(this);
                        ShowFlightSearch();
                    }
                    else MessageBox.Show("Login or Password is invalid.");
                }
            }
            catch
            {
                MessageBox.Show("Login or Password is invalid.");
            }

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public void ShowFlightSearch()
        {
            reservationsWindow.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Collapsed;
            searchWindow.Visibility = Visibility.Visible;
        }

        public void ShowReservations()
        {
            reservationsWindow.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
            searchWindow.Visibility = Visibility.Collapsed;
        }
    }
}
