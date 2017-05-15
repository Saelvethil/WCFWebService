using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }

        [Required]
        public int FlightID { get; set; }

        [Required]
        public int UserID { get; set; }

        [MaxLength]
        public byte[] Receipt { get; set; }

        public virtual Flight Flight { get; set; }

        public virtual User User { get; set; }
    }

    public class ReservationSimple
    {
        public int ReservationID { get; set; }

        [Required]
        public int FlightID { get; set; }

        public virtual Flight Flight { get; set; }
    }
}
