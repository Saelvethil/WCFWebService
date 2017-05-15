using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class User
    {
        public User()
        {
            Flights = new List<Flight>();
        }

        public int UserID { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [StringLength(255)]
        [Required]
        public string Surname { get; set; }

        [StringLength(255)]
        [Required]
        public string Login { get; set; }
        
        [StringLength(255)]
        [Required]
        public string Password { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }
    }

    public class UserLogin
    {
        [StringLength(255)]
        [Required]
        public string Login { get; set; }

        [StringLength(255)]
        [Required]
        public string Password { get; set; }
    }
}
