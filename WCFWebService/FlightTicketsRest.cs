using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entities.Models;
using Entities.Models.Enums;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ServiceModel.Web;

namespace WCFWebService
{
    public class FlightTicketsRest : IFlightTicketsRest
    {
        public int GetClientID(UserLogin user)
        {
            if (user.Login != "" && user.Password != "")
            {
                using (FlightsDBContext context = new FlightsDBContext())
                {
                    var userDB = context.Users.First(u => u.Login == user.Login);
                    if (user != null && user.Password == user.Password)
                        return userDB.UserID;
                }
            }

            return -1;
        }

        public ICollection<Flight> GetFlights(string cityFromStr, string cityToStr, string departureTimeStr)
        {
            using (FlightsDBContext context = new FlightsDBContext())
            {
                CityEnum cityFrom = ToEnum<CityEnum>(cityFromStr);
                CityEnum cityTo = ToEnum<CityEnum>(cityToStr);
                DateTime departureTime = DateTime.Parse(departureTimeStr);
                if (cityFrom != CityEnum.All && cityTo != CityEnum.All)
                {
                    return context.Flights.Where(f => f.CityFrom == cityFrom && f.CityTo == cityTo && f.DepartureTime >= departureTime).ToList();
                }
                else if (cityFrom != CityEnum.All)
                {
                    return context.Flights.Where(f => f.CityFrom == cityFrom && f.DepartureTime >= departureTime).ToList();
                }
                else if (cityTo != CityEnum.All)
                {
                    return context.Flights.Where(f => f.CityTo == cityTo && f.DepartureTime >= departureTime).ToList();
                }
                else
                {
                    return context.Flights.Where(f => f.DepartureTime >= departureTime).ToList();
                }
            }
        }

        public Stream GetReservationFile(string ReservationID)
        {
            using (FlightsDBContext context = new FlightsDBContext())
            {
                byte[] file = context.Reservations.Find(int.Parse(ReservationID)).Receipt;
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/pdf";
                WebOperationContext.Current.OutgoingResponse.ContentLength = file.Length;
                return new MemoryStream(file);
            }
        }

        public ICollection<ReservationSimple> GetReservations(string userID)
        {
            using (FlightsDBContext context = new FlightsDBContext())
            {
                int ID = int.Parse(userID);
                var temp = context.Reservations.Where(r => r.UserID == ID).ToList();
                List<ReservationSimple> result = new List<ReservationSimple>();
                foreach (var t in temp)
                {
                    result.Add(new ReservationSimple() { Flight = t.Flight, FlightID = t.FlightID, ReservationID = t.ReservationID });
                }
                return result;
            }
        }

        public int SetReservation(Flight flight, string userID)
        {
            using (FlightsDBContext context = new FlightsDBContext())
            {
                User user = context.Users.Find(int.Parse(userID));
                Reservation reservation = new Reservation()
                {
                    FlightID = flight.FlightID,
                    UserID = int.Parse(userID),
                    Receipt = GeneratePDF(flight, user)
                };
                context.Reservations.Add(reservation);
                context.SaveChanges();
                return reservation.ReservationID;
            }
        }

        public ReservationSimple DeleteReservation(string ReservationID)
        {
            using (FlightsDBContext context = new FlightsDBContext())
            {
                Reservation reservation = context.Reservations.Find(int.Parse(ReservationID));
                ReservationSimple response = new ReservationSimple()
                {
                    Flight = reservation.Flight,
                    FlightID = reservation.FlightID,
                    ReservationID = reservation.ReservationID
                };
                context.Reservations.Remove(reservation);
                context.SaveChanges();
                return response;
            }
        }

        private byte[] GeneratePDF(Flight flight, User user)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f, Font.UNDERLINE, BaseColor.RED);
                Font subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14f, BaseColor.BLUE);
                Font contentFont = FontFactory.GetFont(FontFactory.COURIER, 12f, BaseColor.BLACK);

                Paragraph header = new Paragraph("FLIGHT TICKET RESERVATION CONFIRMATION", headerFont)
                {
                    SpacingBefore = 10,
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(header);

                Paragraph flightInfo = new Paragraph("FLIGHT", subHeaderFont)
                {
                    SpacingBefore = 20,
                    Alignment = Element.ALIGN_LEFT
                };
                Paragraph cityFrom = new Paragraph("From: " + flight.CityFrom, contentFont)
                {
                    SpacingBefore = 10,
                    Alignment = Element.ALIGN_LEFT
                };
                Paragraph cityTo = new Paragraph("To: " + flight.CityTo, contentFont)
                {
                    SpacingBefore = 10,
                    Alignment = Element.ALIGN_LEFT
                };
                Paragraph departureTime = new Paragraph("Departs: " + flight.DepartureTime, contentFont)
                {
                    SpacingBefore = 10,
                    Alignment = Element.ALIGN_LEFT,
                };
                document.Add(flightInfo);
                document.Add(cityFrom);
                document.Add(cityTo);
                document.Add(departureTime);

                Paragraph userInfo = new Paragraph("PASSENGER", subHeaderFont)
                {
                    SpacingBefore = 20,
                    Alignment = Element.ALIGN_LEFT
                };
                Paragraph userLogin = new Paragraph("Login: " + user.Login, contentFont)
                {
                    SpacingBefore = 10,
                    Alignment = Element.ALIGN_LEFT
                };
                Paragraph userName = new Paragraph("Name: " + user.Name + " " + user.Surname, contentFont)
                {
                    SpacingBefore = 10,
                    Alignment = Element.ALIGN_LEFT
                };
                Paragraph userAddress = new Paragraph("Address: " + user.Address, contentFont)
                {
                    SpacingBefore = 10,
                    Alignment = Element.ALIGN_LEFT
                };
                document.Add(userInfo);
                document.Add(userLogin);
                document.Add(userName);
                document.Add(userAddress);

                document.Close();
                memoryStream.Close();
                return memoryStream.ToArray();
            }
        }

        public T ToEnum<T>(string value, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}
