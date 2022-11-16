using System.ComponentModel.DataAnnotations;

namespace PKTickets.Models
{
    public class PayType
    {
        [Key]
        public int TypeId { get; set; }

        [Required(ErrorMessage = "Please Enter the Reservation Type !!!")]
        [StringLength(20, ErrorMessage = "Type Name length must be below 20 characters")]
        public string Type { get; set; }
        public bool IsActive { get; set; }=true;

    }
}
