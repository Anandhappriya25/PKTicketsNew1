using PKTickets.Interfaces;
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
            //var reservationExist = ReservationById(id);
            //if (reservationExist == null)
            //{
            //    messages.Message = "Reservation Id is not found";
            //}
            //DateTime date = DateTime.Now;
            //TimeSpan time = new TimeSpan(0, date.Hour, date.Minute);
            //var timing = TimingConvert.ConvertToInt(Convert.ToString(time));
            //var seat = db.Seats.FirstOrDefault(x => x.SeatId == reservationExist.SeatId);
            //var reservedMovie = db.Shows.FirstOrDefault(x => x.ShowId == seat.ShowsId);

            //if (date > reservedMovie.Date)
            //{
            //    messages.Message = "Reservation Cancelation time is finished for this Id";
            //    return messages;
            //}
            //var reservedShow = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == reservedMovie.ShowTimeId);
            //if (timing < reservedShow.ShowTiming)
            //{
            //    messages.Message = "Reservation Cancelation time is finished for this Id";
            //    return messages;
            //}

            //else
            //{
            //    reservationExist.IsActive = false;
            //    seat.FSAvailable = seat.FSAvailable + reservationExist.FSBooked;
            //    seat.SSAvailable = seat.FSAvailable + reservationExist.SSBooked;
            //    seat.TSAvailable = seat.FSAvailable + reservationExist.TSBooked;
            //    seat.FOSAvailable = seat.FSAvailable + reservationExist.FOSBooked;
            //    seat.BSAvailable = seat.FSAvailable + reservationExist.BSBooked;
            //    seat.FSReserved = seat.FSReserved - reservationExist.FSBooked;
            //    seat.SSReserved = seat.SSReserved - reservationExist.SSBooked;
            //    seat.TSReserved = seat.TSReserved - reservationExist.TSBooked;
            //    seat.FOSReserved = seat.FOSReserved - reservationExist.FOSBooked;
            //    seat.BSReserved = seat.BSReserved - reservationExist.BSBooked;
            //    db.SaveChanges();
            //    messages.Success = true;
            //    messages.Message = "Reservation is succssfully deleted";
            //}
            return messages;
        }

        public Messages CreateReservation(ReservationDTO reservationDTO)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var user = db.Users.Where(x => x.IsActive == true).FirstOrDefault(x => x.UserId == reservationDTO.UserId);
            if(user == null)
            {
                messages.Message = "User Id is Not found";
                return messages;
            }
            var schedule = db.Schedules.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScheduleId == reservationDTO.ScheduleId);
            if (schedule == null)
            {
                messages.Message = "Schedule Id is Not found";
                return messages;
            }
            else if(schedule.AvailablePreSeats- reservationDTO.PremiumTickets < 0)
            {
                messages.Message = "Only "+schedule.AvailablePreSeats+" Premium Tickets Available";
                return messages;
            }
            else if (schedule.AvailableEliSeats - reservationDTO.EliteTickets < 0)
            {
                messages.Message = "Only " + schedule.AvailableEliSeats + " Elite Tickets Available";
                return messages;
            }
            else
            {
                Reservation reservation=new Reservation();
                reservation.UserId=reservationDTO.UserId;
                reservation.ScheduleId=reservationDTO.ScheduleId;
                reservation.NumberOfTickets=reservationDTO.PremiumTickets + reservationDTO.EliteTickets;
                schedule.AvailablePreSeats = schedule.AvailablePreSeats - reservationDTO.PremiumTickets;
                schedule.AvailableEliSeats = schedule.AvailableEliSeats - reservationDTO.EliteTickets;
                db.Reservations.Add(reservation);
                db.SaveChanges();
                messages.Success=true;
                messages.Message = "Successfully" + reservation.NumberOfTickets +" Tickets Reserved";
                return messages;
            }
            
        }


        public Messages UpdateReservation(ReservationDTO reservationDTO)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var reservationExist = ReservationById(reservationDTO.ReservationId);
            if (reservationExist == null)
            {
                messages.Message = "Reservation Id is Not found";
                return messages;
            }
            DateTime date = DateTime.Now;
            TimeSpan time = new TimeSpan(0, date.Hour, date.Minute);
            var timing = TimingConvert.ConvertToInt(Convert.ToString(time));
            var schedule = db.Schedules.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScheduleId == reservationDTO.ScheduleId);
            if(date.Date > schedule.Date)
            {
                messages.Message = "Reservation Updating time is finished for this Id";
                return messages;
            }
            var showTime = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == schedule.ShowTimeId);
            if (timing > showTime.ShowTiming)
            {
                messages.Message = "Reservation Updating time is finished for this Id";
                return messages;
            }
            


            //if (date > reservedMovie.Date)
            //{
            //   
            //}
            //var reservedShow = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == reservedMovie.ShowTimeId);
            //if (timing < reservedShow.ShowTiming)
            //{
            //    messages.Message = "Reservation Updating time is finished for this Id";
            //    return messages;
            //}
            //var seat = db.Seats.FirstOrDefault(x => x.ShowsId == reservation.ShowId);
            //if (seat.FSAvailable + (reservationExist.FSBooked - reservation.FSBooked) < 0)
            //{
            //    messages.Message = "This Show Do not have that much of FirstSection seats";
            //    return messages;
            //}
            //else if (seat.SSAvailable + (reservationExist.SSBooked - reservation.SSBooked) < 0)
            //{
            //    messages.Message = "This Show Do not have that much of SecondSection seats";
            //    return messages;
            //}
            //else if (seat.TSAvailable + (reservationExist.TSBooked - reservation.TSBooked) < 0)
            //{
            //    messages.Message = "This Show Do not have that much of ThirdSection seats";
            //    return messages;
            //}
            //else if (seat.FOSAvailable + (reservationExist.FOSBooked - reservation.FOSBooked) < 0)
            //{
            //    messages.Message = "This Show Do not have that much of FourthSection seats";
            //    return messages;
            //}
            //else if (seat.BSAvailable + (reservationExist.BSBooked - reservation.BSBooked) < 0)
            //{
            //    messages.Message = "This Show Do not have that much of BalconySection seats";
            //    return messages;
            //}
            //else
            //{
            //    seat.FSAvailable = seat.FSAvailable + (reservationExist.FSBooked - reservation.FSBooked);
            //    seat.SSAvailable = seat.SSAvailable + (reservationExist.SSBooked - reservation.SSBooked);
            //    seat.TSAvailable = seat.TSAvailable + (reservationExist.TSBooked - reservation.TSBooked);
            //    seat.FOSAvailable = seat.FOSAvailable + (reservationExist.FOSBooked - reservation.FOSBooked);
            //    seat.BSAvailable = seat.BSAvailable + (reservationExist.BSBooked - reservation.BSBooked);
            //    seat.FSReserved = seat.FSReserved - (reservationExist.FSBooked - reservation.FSBooked);
            //    seat.SSReserved = seat.SSReserved - (reservationExist.SSBooked - reservation.SSBooked);
            //    seat.TSReserved = seat.TSReserved - (reservationExist.TSBooked - reservation.TSBooked);
            //    seat.FOSReserved = seat.FOSReserved - (reservationExist.FOSBooked - reservation.FOSBooked);
            //    seat.BSReserved = seat.BSReserved - (reservationExist.BSBooked - reservation.BSBooked);
            //    reservationExist.FSBooked = reservation.FSBooked;
            //    reservationExist.SSBooked = reservation.SSBooked;
            //    reservationExist.TSBooked = reservation.TSBooked;
            //    reservationExist.FOSBooked = reservation.FOSBooked;
            //    reservationExist.BSBooked = reservation.BSBooked;
            //    reservationExist.NumberOfTickets = (int)(reservation.FSBooked + reservation.SSBooked + reservation.TSBooked + reservation.FOSBooked + reservation.BSBooked);
            //    db.SaveChanges();
            //    messages.Success = true;
            //    messages.Message = " Tickets are Successfully Updated";}
            return messages;

        }




        //#region Private Methods
        //private User UserById(int id)
        //{
        //    var user = db.Users.Where(x => x.IsActive == true).FirstOrDefault(x => x.UserId == id);
        //    return user;
        //}
        //private Schedule ShowById(int id)
        //{
        //    var show = db.Schedules.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScheduleId == id);
        //    return show;
        //}



        //private Screen ScreenById(int id)
        //{
        //    var screen = db.Screens.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScreenId == id);
        //    return screen;
        //}

    }
}

