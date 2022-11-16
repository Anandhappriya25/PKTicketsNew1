using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKTickets.Models
{
    public class Theater
    {
        [Key]
        public int TheaterId { get;set; }

        [Required(ErrorMessage = "Please Enter the Theater Name !!!")]
        [StringLength(20, ErrorMessage = "Theater Name length must be below 20 characters")]
        public string TheaterName { get;set; }

        [Required(ErrorMessage = "Please Enter the Location !!!")]
        [StringLength(20, ErrorMessage = "Location length must be below 20 characters")]
        public string Location { get;set; } 


        [Required(ErrorMessage = "Please enter the NormalPrice")]
        [Range(50, 1000, ErrorMessage = "Normal Price must be between 50 to 300")]
        public int NormalPrice { get; set; }

        [Range(100, 2000, ErrorMessage = "Balcony Price must be between 100 to 500")]
        public int? BalconyPrice { get; set; }


        [Required(ErrorMessage = "Please enter the Screens Count")]
        [Range(1, 10, ErrorMessage = "Screen Count  must be between 1 to 10")]
        public int  Screens { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
