using Entities.Models;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for Reservations.xaml
    /// </summary>
    public partial class Reservations : Window
    {
        private MainWindow mainWindow;
        /// <summary>
        /// rest client
        /// </summary>
        public WCFWebService.IFlightTicketsRest client;

        private string clientID;
        public ReservationSimple SelectedReservation { get; set; }

        public Reservations(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            client = mainWindow.client;
            clientID = mainWindow.clientID;
            this.Title = "Your reservations";
            this.DataContext = this;
            IsVisibleChanged += Reservations_IsVisibleChanged;
        }

        private void Reservations_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                ReservationsCollection.ItemsSource = client.GetReservations(clientID);
            }
        }

        private void DownloadPDF_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                Stream file = client.GetReservationFile(SelectedReservation.ReservationID.ToString());
                if (file != null)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.FileName = "Receipt";
                    saveDialog.DefaultExt = ".pdf";
                    saveDialog.Filter = "Pdf documents (.pdf)|*.pdf";

                    bool? result = saveDialog.ShowDialog();

                    if (result.HasValue && result == true)
                    {
                        using (var fileStream = new FileStream(saveDialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            file.CopyTo(fileStream);
                        }
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

        private void RemoveReservation_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                client.DeleteReservation(SelectedReservation.ReservationID.ToString());
                ReservationsCollection.ItemsSource = client.GetReservations(clientID);
                MessageBox.Show("Reservation has been removed.");
            }
            else MessageBox.Show("Choose a reservation to remove.");
        }
    }
}
