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

        public Schedule ScheduleById(int id)
        {
            var schedule = db.Schedules.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScheduleId == id);
            return schedule;
        }

        public List<Schedule> SchedulesByMovieId(int id)
        {

            var scheduleList = SchedulesList().Where(x => x.MovieId == id).ToList();
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
                messages.Message = "The Schedule Id is not found";
            }
            var timeExist = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == scheduleExist.ShowTimeId);
            if (date.Date >= scheduleExist.Date && timeExist.ShowTiming > timeValue)
            {
                scheduleExist.IsActive = false;
                    db.SaveChanges();
                    messages.Success = true;
                    messages.Message = "The Schedule is Removed From reservation"; 
            }
            else
            {
                messages.Message = "The Reservation is Already Started so cannot delete";
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
                messages.Message = "The Screen Id You Entered is wrong";
                return messages;
            }
            var scheduleExist = SchedulesByMovieId(schedule.MovieId).Where(x => x.ScreenId == schedule.ScreenId).Where
               (x => x.Date == schedule.Date).FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
            if (scheduleExist != null)
            {
                messages.Message = "This Schedule is already Registered";
                return messages;
            }
            DateTime date = DateTime.Now;
            var timeValue = TimesValue(date);
            var timeExist = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
            if(date.Date >= schedule.Date && timeExist.ShowTiming > timeValue)
            {
                schedule.PremiumSeats = screen.PremiumCapacity;
                schedule.EliteSeats = screen.EliteCapacity;
                schedule.AvailablePreSeats = screen.PremiumCapacity;
                schedule.AvailableEliSeats = screen.EliteCapacity;
                db.Schedules.Add(schedule);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Schedule Is added Successfully";
                return messages;
            }
            else
            {

                messages.Message = "The date you are entered Is wrong.";
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
                messages.Message = "TheReservation Is Already started you cannot update";
                return messages;
            }
            else
            {
                scheduleExist.MovieId = schedule.MovieId;
                    db.SaveChanges();
                    messages.Success = true;
                    messages.Message = "The Schedule is succssfully Updated";
            }
                return messages;
        }
        #region
        private int TimesValue(DateTime date)
        {
            TimeSpan time = new TimeSpan(date.Hour, date.Minute, 0);
            return (TimingConvert.ConvertToInt(Convert.ToString(time)));
        }
        #endregion
    }
}
