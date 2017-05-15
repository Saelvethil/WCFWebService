using Entities.Models;
using Entities.Models.Enums;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class FlightsInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<FlightsDBContext>
    {
        protected override void Seed(FlightsDBContext context)
        {
            for (int day = 1; day <= 31; day++)
            {
                var flights = new List<Flight>
                {
                new Flight{CityFrom=CityEnum.Warsaw,CityTo=CityEnum.Berlin,DepartureTime=new DateTime(2016, 5, day, 14, 51, 00)},
                new Flight{CityFrom=CityEnum.Warsaw,CityTo=CityEnum.London,DepartureTime=new DateTime(2016, 5, day, 17, 12, 00)},
                new Flight{CityFrom=CityEnum.Warsaw,CityTo=CityEnum.Prague,DepartureTime=new DateTime(2016, 5, day, 13, 23, 00)},
                new Flight{CityFrom=CityEnum.Berlin,CityTo=CityEnum.Warsaw,DepartureTime=new DateTime(2016, 5, day, 12, 46, 00)},
                new Flight{CityFrom=CityEnum.Berlin,CityTo=CityEnum.London,DepartureTime=new DateTime(2016, 5, day, 15, 16, 00)},
                new Flight{CityFrom=CityEnum.Berlin,CityTo=CityEnum.Prague,DepartureTime=new DateTime(2016, 5, day, 18, 37, 00)},
                new Flight{CityFrom=CityEnum.London,CityTo=CityEnum.Warsaw,DepartureTime=new DateTime(2016, 5, day, 07, 26, 00)},
                new Flight{CityFrom=CityEnum.London,CityTo=CityEnum.Berlin,DepartureTime=new DateTime(2016, 5, day, 19, 54, 00)},
                new Flight{CityFrom=CityEnum.London,CityTo=CityEnum.Prague,DepartureTime=new DateTime(2016, 5, day, 16, 36, 00)},
                new Flight{CityFrom=CityEnum.Prague,CityTo=CityEnum.Warsaw,DepartureTime=new DateTime(2016, 5, day, 06, 57, 00)},
                new Flight{CityFrom=CityEnum.Prague,CityTo=CityEnum.Berlin,DepartureTime=new DateTime(2016, 5, day, 21, 13, 00)},
                new Flight{CityFrom=CityEnum.Prague,CityTo=CityEnum.London,DepartureTime=new DateTime(2016, 5, day, 16, 43, 00)},
                };
                flights.ForEach(f => context.Flights.Add(f));
            }
            context.SaveChanges();

            var users = new List<User>
            {
                new User { Name="Thomas", Surname="Black", Login="Thomas", Password="1234", Address="Room 67, 14 Tottenham Court Road, London, England, W1T 1JY"},
                new User { Name="Dude", Surname="Lebowsky", Login="Lebowsky", Password="Dude", Address="Hollywood Star Lanes - 5227 Santa Monica Blvd., Hollywood, Los Angeles, California, USA"}
            };
            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
    }
}
