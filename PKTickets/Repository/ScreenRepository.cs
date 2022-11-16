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
            return db.Screens.Where(x=>x.IsActive==true).ToList();
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
            var theaterExist = db.Theaters.Where(x=>x.IsActive==true).FirstOrDefault(x=>x.TheaterId == screen.TheaterId);
            if(theaterExist==null)
            {
                messages.Message = "Theater Id is Not Registered.";
                return messages;
            }
            var totalScreens = GetAllScreens().Where(x => x.TheaterId == screen.TheaterId).ToList();
            if(theaterExist.Screens>totalScreens.Count())
            {
                messages.Message = "Entered Theater Id is already added all Screens.";
                return messages;
            }
            else
            {
                db.Screens.Add(screen);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Screen is succssfully added";
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
                messages.Message = "Screen Id is not found";
                return messages;
            }
            else
            {
                ScreenExist.ScreenName= screen.ScreenName;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Screen Name is succssfully Updated";
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
                messages.Message = "Screen Id is not found";
                return messages;
            }
            else
            {
                ScreenExist.IsActive=false;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Screen is succssfully Deleted";
                return messages;
            }
        }

        public List<TheaterScreensDTO> TheaterScreens(int id)
        {
            var screens = (from theater in db.Theaters
                                  join screen in db.Screens on theater.TheaterId equals screen.TheaterId
                                  where theater.TheaterId == id
                                  select new TheaterScreensDTO()
                                  {
                                      ScreenId = screen.ScreenId,
                                      ScreenName = screen.ScreenName,
                                      FirstSection= screen.FirstSection,
                                      SecondSection= screen.SecondSection,
                                      ThirdSection= screen.ThirdSection,    
                                      FourthSection= screen.FourthSection,  
                                      BalconySection= screen.BalconySection,
                                  }).ToList();

            return screens;
        }

    }
}
