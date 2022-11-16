using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;
using System.Linq;
using System.Xml.Linq;

namespace PKTickets.Repository
{
    public class ShowRepository : IShowRepository
    {
        private readonly PKTicketsDbContext db;
        public ShowRepository(PKTicketsDbContext db)
        {
            this.db = db;
        }

        public List<Show> ShowsList()
        {
            DateTime date=DateTime.Now;
            TimeSpan time = new TimeSpan(0, date.Hour, date.Minute);
            var time2=Convert.ToString(time);
            var time3 = TimingConvert.ConvertToInt(time2);
            var list= db.Shows.Where(x => x.IsActive == true).Where(x => x.Date == date.Date).ToList();
            var showList = new List<Show>();
            foreach (Show movie in list)
            {
                var times = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == movie.ShowTimeId);
              if(times.ShowTiming>= time3)
                {
                  showList.Add(movie);
                }
            }
            return showList;
        }

        public Show ShowById(int id)
        {
            var show = db.Shows.Where(x => x.IsActive == true).FirstOrDefault(x => x.ShowId==id);
            return show;
        }

        public List<Show> ShowsByMovieId(int id)
        {

            var show = ShowsList().Where(x => x.MovieId == id).ToList();
            return show;
        }

       
        public Messages DeleteShow(int id)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var showExist = ShowById(id);
            if (showExist == null)
            {
                messages.Message = "The Show Id is not found";
            }
            else
            {
                showExist.IsActive=false;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "The Show is Removed";
            }
            return messages;
        }

        public Messages CreateShow(Show show)
        {
            Messages messages = new Messages();
            messages.Success = false;
            DateTime date = DateTime.Now;
            if(show.Date<date)
            {
                messages.Message = "The date you are entered Is wrong.";
                return messages;
            }
            var currentMovieExist = ShowsByMovieId(show.MovieId).Where(x=>x.ScreenId== show.ScreenId).Where
                (x => x.Date == show.Date).FirstOrDefault(x => x.ShowTimeId == show.ShowTimeId);
            if (currentMovieExist != null)
            {
                messages.Message = "This Show is already Registered";
            }
            else
            {
                db.Shows.Add(show);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Show Is added Successfully";
            }
            return messages;
        }

        public Messages UpdateShow(Show show)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var showExist = ShowById(show.ShowId);
            if (showExist == null)
            {
                messages.Message = "The Show Id is not found";
                return messages;
            }
            var reserved = db.Seats.FirstOrDefault(x => x.ShowsId == show.ShowId);
            if (reserved != null)
            {
                messages.Message = "The Show is already  Reserved so can not update ";
            }
            else
            {
                showExist.MovieId = show.MovieId;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "The Show is succssfully Updated";
            }
            return messages;
        }

    }
}
