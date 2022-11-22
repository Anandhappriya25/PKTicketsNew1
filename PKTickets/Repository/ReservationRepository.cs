﻿using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;
using System.Linq;
using System.Xml.Linq;

namespace PKTickets.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly PKTicketsDbContext db;
        public ReservationRepository(PKTicketsDbContext db)
        {
            this.db = db;
        }

        public List<Reservation> ReservationList()
        {
            return db.Reservations.Where(x => x.IsActive == true).ToList();
        }

        public Reservation ReservationById(int id)
        {
            var reservation = db.Reservations.Where(x => x.IsActive == true).FirstOrDefault(x => x.ReservationId == id);
            return reservation;
        }
        public List<Reservation> ReservationsByShowId(int id)
        {
            return ReservationList().Where(x => x.ScheduleId == id).ToList();
        }

        public Messages DeleteReservation(int id)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var reservationExist = ReservationById(id);
            if (reservationExist == null)
            {
                messages.Message = "Reservation Id("+id+") is not found";
            }
            DateTime date = DateTime.Now;
            TimeSpan time = new TimeSpan(0, date.Hour, date.Minute);
            var timing = TimingConvert.ConvertToInt(Convert.ToString(time));
            var schedule = db.Schedules.FirstOrDefault(x => x.ScheduleId == reservationExist.ScheduleId);
            if (date.Date > schedule.Date)
            {
                messages.Message = "You are UpTo Time ,so can't Cancel The Reservation";
                return messages;
            }
            var showTiming = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
            if (date.Date < schedule.Date)
            {
                return DeleteSave(reservationExist, schedule);
            }
            else
            {
                if (timing < showTiming.ShowTiming)
                {
                    messages.Message = "You are UpTo Time ,so can't Cancel The Reservation";
                    return messages;
                }
                else
                {
                    return DeleteSave(reservationExist,schedule);
                }
            }
               
        }

        public Messages CreateReservation(Reservation reservation)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var user = db.Users.Where(x => x.IsActive == true).FirstOrDefault(x => x.UserId == reservation.UserId);
            if(user == null)
            {
                messages.Message = "User Id("+reservation.UserId+") is Not found";
                return messages;
            }
            var schedule = db.Schedules.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScheduleId == reservation.ScheduleId);
            if (schedule == null)
            {
                messages.Message = "Schedule Id("+reservation.ScheduleId+") is Not found";
                return messages;
            }
            else if(schedule.AvailablePreSeats- reservation.PremiumTickets < 0)
            {
                messages.Message = "Only "+schedule.AvailablePreSeats+" Premium Tickets are Available";
                return messages;
            }
            else if (schedule.AvailableEliSeats - reservation.EliteTickets < 0)
            {
                messages.Message = "Only " + schedule.AvailableEliSeats + " Elite Tickets are Available";
                return messages;
            }
            else
            {
                var tickets= reservation.PremiumTickets+ reservation.EliteTickets;
                schedule.AvailablePreSeats = schedule.AvailablePreSeats - reservation.PremiumTickets;
                schedule.AvailableEliSeats = schedule.AvailableEliSeats - reservation.EliteTickets;
                db.Reservations.Add(reservation);
                db.SaveChanges();
                messages.Success=true;
                messages.Message = "Successfully " + tickets+" Tickets are Reserved";
                return messages;
            }
            
        }


        public Messages UpdateReservation(Reservation reservation)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var reservationExist = ReservationById(reservation.ReservationId);
            if (reservationExist == null)
            {
                messages.Message = "Reservation Id("+reservation.ReservationId+") is Not found";
                return messages;
            }
            DateTime date = DateTime.Now;
            TimeSpan time = new TimeSpan(0, date.Hour, date.Minute);
            var timing = TimingConvert.ConvertToInt(Convert.ToString(time));
            var schedule = db.Schedules.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScheduleId == reservation.ScheduleId);
            if(date.Date > schedule.Date)
            {
                messages.Message = "You are UpTo Time ,so can't Update The Reservation";
                return messages;
            }
            var showTime = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
            if (date.Date < schedule.Date)
            {
                return UpdateSave(reservation, reservationExist, schedule);
            }
            else 
            {
                if (timing > showTime.ShowTiming)
                {
                    messages.Message = "You are UpTo Time ,so can't Update The Reservation";
                    return messages;
                }
                else
                {
                        return UpdateSave(reservation,reservationExist,schedule);
                }
             }
                    
        }
          

        




    #region Private Methods
      private Messages UpdateSave(Reservation reservation, Reservation reservationExist, Schedule schedule)
      {
        Messages messages = new Messages();
        messages.Success = false;
        var premiumSeats = schedule.AvailablePreSeats - (reservationExist.PremiumTickets - reservation.PremiumTickets);
        var eliteSeats = schedule.AvailableEliSeats - (reservationExist.EliteTickets - reservation.EliteTickets);
        var premium = schedule.AvailablePreSeats - reservationExist.PremiumTickets;
        var elite = schedule.AvailableEliSeats - reservationExist.EliteTickets;
        if (premiumSeats < 0)
        {
            messages.Message = "This Show Do not have that much of Premium seats,only " + premium + "Premium Tickets available";
            return messages;
        }
        else if (eliteSeats < 0)
        {
            messages.Message = "This Show Do not have that much of Elite seats,only " + elite + "Elite Tickets available";
            return messages;
        }
        else
        {
            schedule.AvailablePreSeats = premiumSeats;
            schedule.AvailableEliSeats = eliteSeats;
            reservationExist.PremiumTickets = reservation.PremiumTickets;
            reservationExist.EliteTickets = reservation.EliteTickets;
            db.SaveChanges();
            messages.Success = true;
            messages.Message = " Tickets are Successfully Updated";
            return messages;


        }
      }
        private Messages DeleteSave( Reservation reservationExist, Schedule schedule)
        {
            Messages messages = new Messages();
            schedule.AvailablePreSeats = schedule.AvailablePreSeats + reservationExist.PremiumTickets;
            schedule.AvailableEliSeats = schedule.AvailableEliSeats + reservationExist.EliteTickets;
            reservationExist.IsActive = false;
            db.SaveChanges();
            messages.Success = true;
            messages.Message = "Reservation is succssfully deleted";
            return messages;
        }

        #endregion

    }
}

