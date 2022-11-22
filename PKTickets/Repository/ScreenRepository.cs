using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;
using System.Linq;
using System.Xml.Linq;

namespace PKTickets.Repository
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly PKTicketsDbContext db;
        public ScreenRepository(PKTicketsDbContext db)
        {
            this.db = db;
        }

        public List<Screen> GetAllScreens()
        {
            return db.Screens.Where(x => x.IsActive == true).ToList();
        }

        public Screen ScreenById(int id)
        {
            var screen = db.Screens.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScreenId == id);
            return screen;
        }


        public Messages AddScreen(Screen screen)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var theaterExist = db.Theaters.Where(x => x.IsActive == true).FirstOrDefault(x => x.TheaterId == screen.TheaterId);
            if (theaterExist == null)
            {
                messages.Message = "Theater Id("+screen.TheaterId+") is Not Registered.";
                return messages;
            }
            else
            {
                db.Screens.Add(screen);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Screen ("+screen.ScreenName+") is succssfully Added";
                return messages;
            }
        }

        public Messages UpdateScreen(Screen screen)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var ScreenExist = ScreenById(screen.ScreenId);
            if (ScreenExist == null)
            {
                messages.Message = "Screen Id("+ screen.ScreenId + ") is not found";
                return messages;
            }
            else
            {
                ScreenExist.ScreenName = screen.ScreenName;
                ScreenExist.ElitePrice=screen.ElitePrice;
                ScreenExist.PremiumPrice = screen.PremiumPrice;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Screen ("+ screen.ScreenName + ") is succssfully Updated";
                return messages;
            }
        }
        public Messages RemoveScreen(int screenId)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var ScreenExist = ScreenById(screenId);
            if (ScreenExist == null)
            {
                messages.Message = "Screen Id(" + screenId + ") is not found";
                return messages;
            }
            else
            {
                ScreenExist.IsActive = false;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Screen ("+ ScreenExist .ScreenName+ ") is succssfully Deleted";
                return messages;
            }
        }

        public List<Screen> TheaterScreens(int id)
        {
            var screens = (from theater in db.Theaters
                           join screen in db.Screens on theater.TheaterId equals screen.TheaterId
                           where theater.TheaterId == id
                           select new Screen()
                           {
                               ScreenId = screen.ScreenId,
                               ScreenName = screen.ScreenName,
                               PremiumPrice = screen.PremiumPrice,
                               ElitePrice = screen.ElitePrice,
                               PremiumCapacity = screen.PremiumCapacity,
                               EliteCapacity = screen.EliteCapacity,
                           }).ToList();

            return screens;
        }

    }
}
