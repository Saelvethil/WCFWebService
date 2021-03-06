﻿using Entities.Models;
using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for FlightSearch.xaml
    /// </summary>
    public partial class FlightSearch : Window
    {
        private MainWindow mainWindow;
        /// <summary>
        /// rest client
        /// </summary>
        public WCFWebService.IFlightTicketsRest client;
        private string clientID;
        public CityEnum CityFrom { get; set; }

        public CityEnum CityTo { get; set; }

        public DateTime DepartureTime { get; set; } = DateTime.Now;

        public Flight SelectedFlight { get; set; }

        public IEnumerable<CityEnum> Cities
        {
            get
            {
                return Enum.GetValues(typeof(CityEnum))
                    .Cast<CityEnum>();
            }
        }

        public FlightSearch(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            client = mainWindow.client;
            clientID = mainWindow.clientID;
            this.Title = "Find Flight";
            this.DataContext = this;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var date = DepartureTime.ToString();
            var flights = client.GetFlights(CityFrom.ToString(), CityTo.ToString(), DepartureTime.ToString());
            FlightsCollection.ItemsSource = flights;
        }

        private void ReserveTicket_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFlight != null)
            {
                try
                {
                    int number = client.SetReservation(SelectedFlight, clientID);
                    MessageBox.Show("Ticket has been reserved. Your reservation number is : " + number);
                }
                catch
                {
                    MessageBox.Show("An error has occured.");
                }

            }
            else MessageBox.Show("Select flight.");
        }

        private void BrowseReservations_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ShowReservations();
        }
    }
}
