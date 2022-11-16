using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Interfaces
{
    public interface IShowRepository
    {
        public List<Show> ShowsList();
        public Show ShowById(int id);
        public List<Show> ShowsByMovieId(int id);
        public Messages DeleteShow(int id);
        public Messages CreateShow(Show show);
        public Messages UpdateShow(Show show);
    }
}
