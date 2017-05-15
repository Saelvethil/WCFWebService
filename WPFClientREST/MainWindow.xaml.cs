using Entities.Models;
using System;
using System.Diagnostics;
using System.ServiceModel.Web;
using System.Windows;
using System.Windows.Controls;
using WCFWebService;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// rest client
        /// </summary>
        public IFlightTicketsRest client;
        public string clientID = String.Empty;
        FlightSearch searchWindow;
        Reservations reservationsWindow;
        public MainWindow()
        {
            InitializeComponent();
        }

        private  void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoginBox.Text != "" && PasswordBox.Password != "")
                {
                    WebChannelFactory<IFlightTicketsRest> webChannelFactory = new WebChannelFactory<IFlightTicketsRest>(new Uri("http://192.168.1.2:8734/WCFWebService/FlightTicketsRest/"));
                    client = webChannelFactory.CreateChannel();
                    try
                    {
                        UserLogin userLogin = new UserLogin()
                        {
                            Login = LoginBox.Text,
                            Password = PasswordBox.Password
                        };

                        clientID = client.GetClientID(userLogin).ToString();
                    }
                    catch(Exception ex)
                    {
                        Debug.Write(ex);
                    }

                    if (clientID != String.Empty)
                    {
                        searchWindow = new FlightSearch(this);
                        reservationsWindow = new Reservations(this);
                        ShowFlightSearch();
                    }
                    else MessageBox.Show("Login or Password is invalid.");
                }
            }
            catch(Exception ex)
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
