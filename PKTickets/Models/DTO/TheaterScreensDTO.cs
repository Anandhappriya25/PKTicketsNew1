using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PKTickets.Models.DTO
{
    public class TheaterScreensDTO
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public int TheaterId { get; set; }
        public int FirstSection { get; set; }
        public int SecondSection { get; set; }
        public int ThirdSection { get; set; }
        public int? FourthSection { get; set; }
        public int? BalconySection { get; set; }     
 

    }
}
