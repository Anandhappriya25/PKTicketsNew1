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
            return ReservationList().Where(x => x.ShowId == id).ToList();
        }

        public Messages DeleteReservation(int id)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var reservationExist = ReservationById(id);
            if (reservationExist == null)
            {
                messages.Message = "Reservation Id is not found";
            }
            DateTime date = DateTime.Now;
            TimeSpan time = new TimeSpan(0, date.Hour, date.Minute);
            var timing = TimingConvert.ConvertToInt(Convert.ToString(time));
            var seat= db.Seats.FirstOrDefault(x => x.SeatId == reservationExist.SeatId);
            var reservedMovie = db.Shows.FirstOrDefault(x => x.ShowId == seat.ShowsId);

            if (date > reservedMovie.Date)
            {
                messages.Message = "Reservation Cancelation time is finished for this Id";
                return messages;
            }
            var reservedShow = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == reservedMovie.ShowTimeId);
            if (timing < reservedShow.ShowTiming)
                {
                    messages.Message = "Reservation Cancelation time is finished for this Id";
                    return messages;
                } 
         
            else
            {
                reservationExist.IsActive = false;
                seat.FSAvailable=seat.FSAvailable+reservationExist.FSBooked;
                seat.SSAvailable = seat.FSAvailable + reservationExist.SSBooked;
                seat.TSAvailable = seat.FSAvailable + reservationExist.TSBooked;
                seat.FOSAvailable = seat.FSAvailable + reservationExist.FOSBooked;
                seat.BSAvailable = seat.FSAvailable + reservationExist.BSBooked;
                seat.FSReserved = seat.FSReserved - reservationExist.FSBooked;
                seat.SSReserved = seat.SSReserved - reservationExist.SSBooked;
                seat.TSReserved = seat.TSReserved - reservationExist.TSBooked;
                seat.FOSReserved = seat.FOSReserved - reservationExist.FOSBooked;
                seat.BSReserved = seat.BSReserved - reservationExist.BSBooked;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Reservation is succssfully deleted";
            }
            return messages;
        }

        public Messages CreateReservation(Reservation reservation)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var user = UserById(reservation.UserId);
            if (user == null)
            {
                messages.Message = "User Not Registered Please SignIn.";
                return messages;
            }
            var show = ShowById(reservation.ShowId);
            if (show == null)
            {
                messages.Message = "You entered the wrong Show id";
                return messages;
            }
            var pay = PayTypeById(reservation.PayTypeId);
            if (pay == null)
            {
                messages.Message = "You entered the wrong PayType id";
                return messages;
            }
            var seat = db.Seats.FirstOrDefault(x => x.ShowsId == reservation.ShowId);
            var screen = ScreenById(show.ScreenId);
            if (seat == null)
            {
                if(screen.FirstSection - reservation.FSBooked<0)
                {
                    messages.Message = "This Show Do not have that much of FirstSection seats";
                    return messages;
                }
                else if (screen.SecondSection - reservation.SSBooked < 0)
                {
                    messages.Message = "This Show Do not have that much of SecondSection seats";
                    return messages;
                }
                else if (screen.ThirdSection - reservation.TSBooked < 0)
                {
                    messages.Message = "This Show Do not have that much of ThirdSection seats";
                    return messages;
                }
                else if (screen.FourthSection - reservation.FOSBooked < 0)
                {
                    messages.Message = "This Show Do not have that much of FourthSection seats";
                    return messages;
                }
                else if (screen.BalconySection - reservation.BSBooked < 0)
                {
                    messages.Message = "This Show Do not have that much of BalconySection seats";
                    return messages;
                }
                else
                {
                 return Save(reservation,seat,screen);  
                }
                
            }
            else
            {
                if (seat.FSAvailable - reservation.FSBooked < 0)
                {
                    messages.Message = "This Show Do not have that much of FirstSection seats";
                    return messages;
                }
                else if (seat.SSAvailable - reservation.SSBooked < 0)
                {
                    messages.Message = "This Show Do not have that much of SecondSection seats";
                    return messages;
                }
                else if (seat.TSAvailable - reservation.TSBooked < 0)
                {
                    messages.Message = "This Show Do not have that much of ThirdSection seats";
                    return messages;
                }
                else if (seat.FOSAvailable - reservation.FOSBooked < 0)
                {
                    messages.Message = "This Show Do not have that much of FourthSection seats";
                    return messages;
                }
                else if (seat.BSAvailable - reservation.BSBooked < 0)
                {
                    messages.Message = "This Show Do not have that much of BalconySection seats";
                    return messages;
                }
                else
                {
                    return Save(reservation, seat,screen);
                }
            }   
        }


        public Messages UpdateReservation(Reservation reservation)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var reservationExist = ReservationById(reservation.ReservationId);
            if (reservationExist == null)
            {
                messages.Message = "Reservation Id is Not found";
                return messages;
            }
            DateTime date = DateTime.Now;
            TimeSpan time = new TimeSpan(0, date.Hour, date.Minute);
            var timing = TimingConvert.ConvertToInt(Convert.ToString(time));
            var reservedMovie = db.Shows.FirstOrDefault(x => x.ShowId == reservation.ShowId);
            if (date > reservedMovie.Date)
            {
                messages.Message = "Reservation Updating time is finished for this Id";
                return messages;
            }
            var reservedShow = db.ShowTimes.FirstOrDefault(x => x.ShowTimeId == reservedMovie.ShowTimeId);
            if (timing < reservedShow.ShowTiming)
            {
                messages.Message = "Reservation Updating time is finished for this Id";
                return messages;
            }
            var seat = db.Seats.FirstOrDefault(x => x.ShowsId == reservation.ShowId);
            if (seat.FSAvailable + (reservationExist.FSBooked - reservation.FSBooked) < 0)
            {
                messages.Message = "This Show Do not have that much of FirstSection seats"; 
                    return messages;
            }
            else if (seat.SSAvailable + (reservationExist.SSBooked - reservation.SSBooked) < 0)
            {
                messages.Message = "This Show Do not have that much of SecondSection seats";
                return messages;
            }
            else if (seat.TSAvailable + (reservationExist.TSBooked - reservation.TSBooked) < 0)
            {
                messages.Message = "This Show Do not have that much of ThirdSection seats";
                return messages;
            }
            else if (seat.FOSAvailable + (reservationExist.FOSBooked - reservation.FOSBooked) < 0)
            {
                messages.Message = "This Show Do not have that much of FourthSection seats";
                return messages;
            }
            else if (seat.BSAvailable + (reservationExist.BSBooked - reservation.BSBooked) < 0)
            {
                messages.Message = "This Show Do not have that much of BalconySection seats";
                return messages;
            }
            else
            {
                seat.FSAvailable = seat.FSAvailable + (reservationExist.FSBooked - reservation.FSBooked);
                seat.SSAvailable = seat.SSAvailable + (reservationExist.SSBooked - reservation.SSBooked);
                seat.TSAvailable = seat.TSAvailable + (reservationExist.TSBooked - reservation.TSBooked);
                seat.FOSAvailable = seat.FOSAvailable + (reservationExist.FOSBooked - reservation.FOSBooked);
                seat.BSAvailable = seat.BSAvailable + (reservationExist.BSBooked - reservation.BSBooked);
                seat.FSReserved = seat.FSReserved - (reservationExist.FSBooked - reservation.FSBooked);
                seat.SSReserved = seat.SSReserved - (reservationExist.SSBooked - reservation.SSBooked);
                seat.TSReserved = seat.TSReserved - (reservationExist.TSBooked - reservation.TSBooked);
                seat.FOSReserved=seat.FOSReserved - (reservationExist.FOSBooked - reservation.FOSBooked);
                seat.BSReserved = seat.BSReserved - (reservationExist.BSBooked - reservation.BSBooked);
                reservationExist.FSBooked = reservation.FSBooked;
                reservationExist.SSBooked = reservation.SSBooked;
                reservationExist.TSBooked = reservation.TSBooked;
                reservationExist.FOSBooked = reservation.FOSBooked;
                reservationExist.BSBooked = reservation.BSBooked;
                reservationExist.NumberOfTickets = (int)(reservation.FSBooked + reservation.SSBooked + reservation.TSBooked + reservation.FOSBooked + reservation.BSBooked);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = " Tickets are Successfully Updated";
                return messages;
            }
        }




        #region Private Methods
        private User UserById(int id)
        {
            var user = db.Users.Where(x => x.IsActive == true).FirstOrDefault(x => x.UserId == id);
            return user;
        }
        private Show ShowById(int id)
        {
            var show = db.Shows.Where(x => x.IsActive == true).FirstOrDefault(x => x.ShowId == id);
            return show;
        }
        private PayType PayTypeById(int id)
        {
            var type = db.PayTypes.Where(x => x.IsActive == true).FirstOrDefault(x => x.TypeId == id);
            return type;
        }

        private Screen ScreenById(int id)
        {
            var screen = db.Screens.Where(x => x.IsActive == true).FirstOrDefault(x => x.ScreenId == id);
            return screen;
        }
        private Messages Save(Reservation reservation,Seat seat,Screen screen)
        {
            Messages messages = new Messages();
            messages.Success = false;
            if (seat== null && screen!=null)
            {
                seat.ShowsId = reservation.ShowId;
                seat.FSAvailable = screen.FirstSection - reservation.FSBooked;
                seat.SSAvailable = screen.SecondSection - reservation.SSBooked;
                seat.TSAvailable = screen.ThirdSection - reservation.TSBooked;
                seat.FOSAvailable = screen.FourthSection - reservation.FOSBooked;
                seat.BSAvailable = screen.BalconySection - reservation.BSBooked;
                
            }
            else
            {
                seat.FSAvailable = seat.FSAvailable - reservation.FSBooked;
                seat.SSAvailable = seat.SSAvailable - reservation.SSBooked;
                seat.TSAvailable = seat.TSAvailable - reservation.TSBooked;
                seat.FOSAvailable = seat.FOSAvailable - reservation.FOSBooked;
                seat.BSAvailable = seat.BSAvailable - reservation.BSBooked;
                
            }
            seat.FSReserved = seat.FSReserved + reservation.FSBooked;
            seat.SSReserved = seat.SSReserved + reservation.SSBooked;
            seat.TSReserved = seat.TSReserved + reservation.TSBooked;
            seat.FOSReserved = seat.FOSReserved + reservation.FOSBooked;
            seat.BSReserved = seat.BSReserved + reservation.BSBooked;
            db.Reservations.Add(reservation);
            reservation.NumberOfTickets = (int)(reservation.FSBooked + reservation.SSBooked + reservation.TSBooked + reservation.FOSBooked + reservation.BSBooked);
            db.SaveChanges();
            messages.Success = true;
            messages.Message = +reservation.NumberOfTickets + " Tickets are Successfully Reserved";
            return messages;
        }
        #endregion
    }
}
