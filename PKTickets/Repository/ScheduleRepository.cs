using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;
using System.Collections;
using System.Linq;
using System.Xml.Linq;

namespace PKTickets.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly PKTicketsDbContext db;
        public ScheduleRepository(PKTicketsDbContext db)
        {
            this.db = db;
        }
        public List<Schedule> SchedulesList()
        {
            return db.Schedules.ToList();
        }
        public List<Schedule> AvailableSchedulesList()
        {
            DateTime date = DateTime.Now;
            var timeValue = TimesValue(date);
            var list = db.Schedules.Where(x => x.IsActive == true).Where(x => x.Date == date.Date).ToList();
            var scheduleList = new List<Schedule>();
            foreach (Schedule movie in list)
            {
                var times = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == movie.ShowTimeId);
                if (times.ShowTiming > timeValue)
                {
                    scheduleList.Add(movie);
                }
            }
            var list2 = db.Schedules.Where(x => x.IsActive == true).Where(x => x.Date > date.Date).ToList();
            if(list2.Count()>0)
            {
                foreach (Schedule movie in list2)
                {
                    scheduleList.Add(movie);
                }
            }
            return scheduleList;
        }
        public Movie MovieById(int id)
        {
            var movie = db.Movies.Where(x => x.IsPlaying == true).FirstOrDefault(x => x.MovieId == id);
            return movie;
        }
        public Schedule ScheduleById(int id)
        {
            var schedule = db.Schedules.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScheduleId == id);
            return schedule;
        }

        public List<Schedule> SchedulesByMovieId(int id)
        {
            var scheduleList = AvailableSchedulesList().Where(x => x.MovieId == id).ToList();
            return scheduleList;
        }


        public Messages DeleteSchedule(int id)
        {
            Messages messages = new Messages();
            messages.Success = false;
            DateTime date = DateTime.Now;
            var timeValue = TimesValue(date);
            var scheduleExist = ScheduleById(id);
            if (scheduleExist == null)
            {
                messages.Message = "The Schedule Id ("+ id+") is not found";
            }
            var timeExist = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == scheduleExist.ShowTimeId);
            if (date.Date >= scheduleExist.Date && timeExist.ShowTiming > timeValue)
            {
                scheduleExist.IsActive = false;
                    db.SaveChanges();
                    messages.Success = true;
                    messages.Message = "The Schedule Id ("+ id+") is Removed From reservation"; 
            }
            else
            {
                messages.Message = "The Reservation is Already Started to Schedule Id ("+ id + ") ,so can't Delete";
            }
            return messages;
        }

        public Messages CreateSchedule(Schedule schedule)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var screen=db.Screens.Where(x=>x.IsActive==true).FirstOrDefault(x => x.ScreenId == schedule.ScreenId);
            if (screen == null)
            {
                messages.Message = "The Screen Id("+ schedule.ScreenId + ") is not Registered";
                return messages;
            }
            var scheduleExist = SchedulesByMovieId(schedule.MovieId).Where(x => x.ScreenId == schedule.ScreenId).Where
               (x => x.Date == schedule.Date).FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
            if (scheduleExist != null)
            {
                messages.Message = "This Schedule is already Registered Please check the Fields";
                return messages;
            }
            DateTime date = DateTime.Now;
            var timeValue = TimesValue(date);
            var timeExist = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
            if(date.Date <= schedule.Date && timeExist.ShowTiming > timeValue)
            {
                schedule.PremiumSeats = screen.PremiumCapacity;
                schedule.EliteSeats = screen.EliteCapacity;
                schedule.AvailablePreSeats = screen.PremiumCapacity;
                schedule.AvailableEliSeats = screen.EliteCapacity;
                db.Schedules.Add(schedule);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Schedule Id ("+ schedule.ScheduleId + ")Is added Successfully";
                return messages;
            }
            else
            {

                messages.Message = "The date("+schedule.Date+")entered is Invalid,Kindly Check the Date.";
                return messages;
            }
        }

        public Messages UpdateSchedule(Schedule schedule)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var scheduleExist = ScheduleById(schedule.ScheduleId);
            if (scheduleExist == null)
            {
                messages.Message = "The Schedule Id is not found";
                return messages;
            }
            DateTime date = DateTime.Now;
            var timeValue = TimesValue(date);
            var timeExist = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
            if (date.Date >= schedule.Date && timeExist.ShowTiming > timeValue)
            {
                messages.Message = "The Reservation Is Already started fot this Schedule Id ("+ schedule.ScheduleId + ") So can't Update" ;
                return messages;
            }
            else
            {
                scheduleExist.MovieId = schedule.MovieId;
                    db.SaveChanges();
                    messages.Success = true;
                    messages.Message = "The Schedule Id ("+ schedule.ScheduleId + ") is Successfully Updated";
            }
                return messages;
        }

        public SchedulesListDTO SchedulesListByScreenId(int id)
        {
            var screens = db.Screens.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScreenId == id);
            var theater = db.Theaters.FirstOrDefault(x => x.TheaterId == screens.TheaterId);
            
            SchedulesListDTO list = new SchedulesListDTO();
            list.TheaterName = theater.TheaterName;
            list.ScreenName = screens.ScreenName; 
            list.PremiumCapacity = screens.PremiumCapacity;
            list.EliteCapacity = screens.EliteCapacity;
            list.Schedules = SchedulesListScreenId(id);
            return list;
        }
        public Screen ScreenById(int id)
        {
            var screen = db.Screens.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScreenId == id);
            return screen;
        }
        public TheatersSchedulesDTO TheaterSchedulesById(int id)
        {
            var theater = db.Theaters.FirstOrDefault(x => x.TheaterId == id);
            var screens = db.Screens.Where(x => x.IsActive == true).Where(x => x.TheaterId == id).ToList();

            TheatersSchedulesDTO list = new TheatersSchedulesDTO();
            list.TheaterName = theater.TheaterName; 
            list.ScreensCount =screens.Count();

           List<ScreenSchedulesDTO> schedules=new List<ScreenSchedulesDTO>();
            foreach (var screen in screens)
            {
                ScreenSchedulesDTO scheduleList = new ScreenSchedulesDTO();
                scheduleList.ScreenId=screen.ScreenId;
                scheduleList.ScreenName=screen.ScreenName;  
                scheduleList.PremiumCapacity=screen.PremiumCapacity;    
                scheduleList.EliteCapacity=screen.EliteCapacity;
                scheduleList.Schedules = SchedulesListScreenId(screen.ScreenId);
                schedules.Add(scheduleList);
            }
            list.Screens = schedules;
            return list;
        }
        public Theater TheaterById(int id)
        {
            var theater = db.Theaters.Where(x => x.IsActive == true).FirstOrDefault(x => x.TheaterId == id);
            return theater;
        }

        public MovieDTO DetailsByMovieId(int id)
        {
            MovieDTO movie = new MovieDTO();
            var moviename = db.Movies.Where(x => x.IsPlaying == true).FirstOrDefault(x => x.MovieId == id);
            movie.MovieName = moviename.Title;
            List<Schedule> schedulesList = AvailableSchedulesList().Where(x=> x.MovieId == id).ToList();    
                //db.Schedules.Where(x => x.IsActive == true).Where(x => x.MovieId == id).ToList();
            List<Schedule> uniqueSchedulesByScreen = schedulesList.DistinctBy(x => x.ScreenId).ToList();
            List<Screen> screenList = new List<Screen>();
            foreach (var schedule in uniqueSchedulesByScreen)
            {
                Screen screen = db.Screens.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScreenId == schedule.ScreenId);
                screenList.Add(screen);
            }
            List<Screen> uniqueScreen = screenList.DistinctBy(x => x.ScreenId).ToList();
            List<Theater> theaterList = new List<Theater>();
            foreach (var screen in uniqueScreen)
            {
                Theater theater = db.Theaters.Where(x => x.IsActive == true).FirstOrDefault(x => x.TheaterId == screen.TheaterId);
                theaterList.Add(theater);
            }
            List<Theater> uniqueTheater = theaterList.DistinctBy(x => x.TheaterId).ToList();
            foreach (var theater in uniqueTheater)
            {
                TheaterDetails theaters = new TheaterDetails();
                theaters.TheaterId = theater.TheaterId;
                theaters.TheaterName = theater.TheaterName;
                List<Screen> theaterScreens = new List<Screen>();
                foreach (var screen in uniqueScreen)
                {
                    if (screen.TheaterId == theater.TheaterId)
                    {
                        theaterScreens.Add(screen);
                    }
                }
                foreach (var screen in theaterScreens)
                {
                    ScreenDetails newScreen = new ScreenDetails();
                    newScreen.ScreenId = screen.ScreenId;
                    newScreen.ScreenName = screen.ScreenName;
                    newScreen.PremiumCapacity = screen.PremiumCapacity;
                    newScreen.EliteCapacity = screen.EliteCapacity;
                    //newScreen.Schedules=newScreen.Schedules == null ? new List<ScheduleDetails>() : newScreen.Schedules;
                    foreach (var schedule in schedulesList)
                    {
                        if (schedule.ScreenId == screen.ScreenId)
                        {
                            ScheduleDetails schedules = new ScheduleDetails();
                            schedules.ScheduleId = schedule.ScheduleId;
                            schedules.Date = schedule.Date;
                            var time = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
                            schedules.ShowTime = TimingConvert.ConvertToString(time.ShowTiming);
                            schedules.AvailablePremiumSeats = schedule.AvailablePreSeats;
                            schedules.AvailableEliteSeats = schedule.AvailableEliSeats;
                            newScreen.Schedules.Add(schedules);
                        }
                    }
                    theaters.Screens.Add(newScreen);
                }
                movie.Theaters.Add(theaters);
            }
            return movie;
        }
        #region PrivateMethods

        private List<SchedulesDTO> SchedulesListScreenId(int id)
        {
            DateTime date = DateTime.Now;
            var timeValue = TimesValue(date);
            var screens = (from screen in db.Screens
                           join schedule in db.Schedules on screen.ScreenId equals schedule.ScreenId
                           join showTime in db.ShowTimes on schedule.ShowTimeId equals showTime.ShowTimeId
                           join movie in db.Movies on schedule.MovieId equals movie.MovieId
                           where screen.ScreenId == id &&  schedule.IsActive == true &&((schedule.Date==date.Date && showTime.ShowTiming >timeValue) || (schedule.Date>date.Date) )
                           select new SchedulesDTO()
                           {
                               ScheduleId = schedule.ScheduleId,
                               MovieName=movie.Title,
                               Date = schedule.Date,
                               ShowTime = TimingConvert.ConvertToString(showTime.ShowTiming),
                               AvailablePremiumSeats = schedule.AvailablePreSeats,
                               AvailableEliteSeats = schedule.AvailableEliSeats
                           }).ToList();

            return screens;
        }
   
        private int TimesValue(DateTime date)
        {
            TimeSpan time = new TimeSpan(date.Hour, date.Minute, 0);
            return (TimingConvert.ConvertToInt(Convert.ToString(time)));
        }
        #endregion
    }
}
