using Entities.Models;
using Entities.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WCFWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IFlightTickets
    {
        [OperationContract]
        int GetClientID(string login, string password);

        [OperationContract]
        ICollection<Flight> GetFlights(CityEnum cityFrom, CityEnum cityTo, DateTime departureTime);

        [OperationContract]
        ICollection<ReservationSimple> GetReservations(int userID);

        [OperationContract]
        int SetReservation(Flight flight, int userID);

        [OperationContract]
        void DeleteReservation(int ReservationID);

        [OperationContract]
        byte[] GetReservationFile(int ReservationID);
    }

    [ServiceContract]
    public interface IFlightTicketsRest
    {
        [WebInvoke(Method = "POST", UriTemplate = "user")]
        [OperationContract]
        int GetClientID(UserLogin user);

        [WebGet(UriTemplate = "flights/{cityFrom}/{cityTo}/{departureTime}")]
        [OperationContract]
        ICollection<Flight> GetFlights(string cityFrom, string cityTo, string departureTime);

        [WebGet(UriTemplate = "reservations/{userID}")]
        [OperationContract]
        ICollection<ReservationSimple> GetReservations(string userID);

        [WebInvoke(Method = "POST", UriTemplate = "reservation/{userID}")]
        [OperationContract]
        int SetReservation(Flight flight, string userID);

        [WebInvoke(Method = "DELETE", UriTemplate = "reservation/{ReservationID}")]
        [OperationContract]
        ReservationSimple DeleteReservation(string ReservationID);

        [WebGet(UriTemplate = "reservationFile/{ReservationID}")]
        [OperationContract]
        Stream GetReservationFile(string ReservationID);
    }
}
