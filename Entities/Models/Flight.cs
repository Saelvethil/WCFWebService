using Entities.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Flight
    {
        public int FlightID { get; set; }

        [Required]
        [Display(Name = "City From")]
        public CityEnum CityFrom { get; set; }

        [Required]
        [Display(Name = "City To")]
        public CityEnum CityTo { get; set; }

        [Required]
        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; }
    }
}
