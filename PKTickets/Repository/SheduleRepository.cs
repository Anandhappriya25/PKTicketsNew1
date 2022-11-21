using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;
using System.Linq;
using System.Xml.Linq;

namespace PKTickets.Repository
{
    public class SheduleRepository : ISheduleRepository
    {
        private readonly PKTicketsDbContext db;
        public SheduleRepository(PKTicketsDbContext db)
        {
            this.db = db;
        }

        public List<Schedule> ShowsList()
        {
            DateTime date = DateTime.Now;
            TimeSpan time = new TimeSpan(date.Hour, date.Minute,0);
            var time2 = Convert.ToString(time);
            var time3 = TimingConvert.ConvertToInt(time2);
            var list = db.Schedules.Where(x => x.IsActive == true).Where(x => x.Date == date.Date).ToList();
            var showList = new List<Schedule>();
            foreach (Schedule movie in list)
            {
                var times = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == movie.ShowTimeId);
                if (times.ShowTiming > time3)
                {
                    showList.Add(movie);
                }
            }
            var list2 = db.Schedules.Where(x => x.IsActive == true).Where(x => x.Date > date.Date).ToList();
            if(list2.Count()>0)
            {
                foreach (Schedule movie in list2)
                {
                    showList.Add(movie);
                }
            }
            return showList;
        }

        public Schedule ShowById(int id)
        {
            var show = db.Schedules.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScheduleId == id);
            return show;
        }

        public List<Schedule> ShowsByMovieId(int id)
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
                showExist.IsActive = false;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "The Show is Removed";
            }
            return messages;
        }

        public Messages CreateShow(Schedule show)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var screen=db.Screens.Where(x=>x.IsActive==true).FirstOrDefault(x => x.ScreenId == show.ScreenId);
            if (screen == null)
            {
                messages.Message = "The Screen Id You Entered is wrong";
                return messages;
            }
            DateTime date = DateTime.Now;
            if (show.Date < date)
            {
                messages.Message = "The date you are entered Is wrong.";
                return messages;
            }
            var currentMovieExist = ShowsByMovieId(show.MovieId).Where(x => x.ScreenId == show.ScreenId).Where
                (x => x.Date == show.Date).FirstOrDefault(x => x.ShowTimeId == show.ShowTimeId);
            if (currentMovieExist != null)
            {
                messages.Message = "This Show is already Registered";
            }
            else
            {
                show.PremiumSeats = screen.PremiumCapacity;
                show.EliteSeats = screen.EliteCapacity;
                show.AvailablePreSeats = screen.PremiumCapacity;
                show.AvailableEliSeats = screen.EliteCapacity;
                db.Schedules.Add(show);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Show Is added Successfully";
            }
            return messages;
        }

        public Messages UpdateShow(Schedule show)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var showExist = ShowById(show.ScheduleId);
            if (showExist == null)
            {
                messages.Message = "The Show Id is not found";
                return messages;
            }
            //var reserved = db.Seats.FirstOrDefault(x => x.ShowsId == show.ShowId);
            //if (reserved != null)
            //{
            //    messages.Message = "The Show is already  Reserved so can not update ";
            //}
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
