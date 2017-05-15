using Entities.Models;
using Microsoft.Win32;
using System;
using System.Windows;
using WPFClient.FlightTickets;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : Window
    {
        private MainWindow mainWindow;
        private FlightTicketsClient client;
        private int clientID;
        public ReservationSimple SelectedReservation { get; set; }

        public Reservations(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            client = mainWindow.client;
            clientID = mainWindow.clientID;
            this.Title = "Your reservations";
            this.DataContext = this;
            Closed += Reservations_Closed;
            IsVisibleChanged += Reservations_IsVisibleChanged;
        }

        private async void Reservations_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                ReservationsCollection.ItemsSource = await client.GetReservationsAsync(clientID);
            }
        }

        private void Reservations_Closed(object sender, EventArgs e)
        {
            client.Close();
        }

        private async void DownloadPDF_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                byte[] file = await client.GetReservationFileAsync(SelectedReservation.ReservationID);
                if (file != null)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.FileName = "Receipt";
                    saveDialog.DefaultExt = ".pdf";
                    saveDialog.Filter = "Pdf documents (.pdf)|*.pdf";

                    bool? result = saveDialog.ShowDialog();

                    if (result.HasValue && result == true)
                    {
                        System.IO.File.WriteAllBytes(saveDialog.FileName, file);
                        System.Diagnostics.Process.Start(saveDialog.FileName);
                    }
                }
            }
            else MessageBox.Show("Choose a flight to get its receipt.");
        }

        private void BrowseFlights_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ShowFlightSearch();
        }

        private async void RemoveReservation_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                await client.DeleteReservationAsync(SelectedReservation.ReservationID);
                ReservationsCollection.ItemsSource = await client.GetReservationsAsync(clientID);
                MessageBox.Show("Reservation has been removed.");
            }
            else MessageBox.Show("Choose a reservation to remove.");
        }
    }
}
