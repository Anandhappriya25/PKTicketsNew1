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

        [Required(ErrorMessage = "Please enter the FirstSection")]
        [Range(10, 100, ErrorMessage = "FirstSection seats must be between 10 to 100")]
        public int FirstSection { get; set; }

        [Required(ErrorMessage = "Please enter the SecondSection")]
        [Range(10, 100, ErrorMessage = "SecondSection seats must be between 10 to 100")]
        public int SecondSection { get; set; }

        [Required(ErrorMessage = "Please enter the SecondSection")]
        [Range(10, 100, ErrorMessage = "SecondSection seats must be between 10 to 100")]
        public int ThirdSection { get; set; }

        [Range(10, 100, ErrorMessage = "FourthSection seats must be between 10 to 100")]
        public int? FourthSection { get; set; } = 0;

        [Range(10, 100, ErrorMessage = "BalconySection seats must be between 10 to 100")]
        public int? BalconySection { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
