using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKTickets.Models
{
    public class Screen
    {
        [Key]
        public int ScreenId { get;set; }

        public string ScreenName { get;set; }


        [Required(ErrorMessage = "Please Enter the Theater Id !!!")]
        public int TheaterId { get; set; }

        [ForeignKey("TheaterId")]
        public Theater? Theater { get; set; }

        [Required(ErrorMessage = "Please enter the PremiumCapacity")]
        [Range(10, 100, ErrorMessage = "PremiumCapacity must be between 10 to 100")]
        public int PremiumCapacity { get; set; }

        [Required(ErrorMessage = "Please enter the EliteCapacity")]
        [Range(10, 100, ErrorMessage = "EliteCapacity  must be between 10 to 100")]
        public int EliteCapacity { get; set; }

        [Required(ErrorMessage = "Please enter the PremiumPrice")]
        [Range(100, 1000, ErrorMessage = "PremiumPrice  must be between 100 to 1000")]
        public int PremiumPrice { get; set; }

        [Required(ErrorMessage = "Please enter the ElitePrice")]
        [Range(100, 1000, ErrorMessage = "ElitePrice  must be between 100 to 1000")]
        public int ElitePrice { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
