using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Interfaces
{
    public interface ISheduleRepository
    {
        public List<Schedule> ShowsList();
        public Schedule ShowById(int id);
        public List<Schedule> ShowsByMovieId(int id);
        public Messages DeleteShow(int id);
        public Messages CreateShow(Schedule show);
        public Messages UpdateShow(Schedule show);
    }
}
