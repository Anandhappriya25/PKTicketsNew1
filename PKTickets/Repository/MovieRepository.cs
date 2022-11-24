using PKTickets.Interfaces;
using PKTickets.Models;
using PKTickets.Models.DTO;

namespace PKTickets.Repository
{
    public class MovieRepository : IMovieRepository
    {

        private readonly PKTicketsDbContext db;
        public MovieRepository(PKTicketsDbContext db)
        {
            this.db = db;
        }

        public List<Movie> GetAllMovies()
        {
            return db.Movies.Where(x => x.IsPlaying == true).ToList();
        }

        public Movie MovieById(int id)
        {
            var movieExist = db.Movies.Where(x => x.IsPlaying == true).FirstOrDefault(x => x.MovieId == id);
            return movieExist;
        }

        public List<Movie> MovieByTitle(string title)
        {
            var movieExists = db.Movies.Where(x => x.IsPlaying == true).Where(x => x.Title == title).ToList();
            return movieExists;
        }

        public List<Movie> MovieByGenre(string genre)
        {
            var movieExists = db.Movies.Where(x => x.IsPlaying == true).Where(x => x.Genre == genre).ToList();
            return movieExists;
        }

        public List<Movie> MovieByLanguage(string language)
        {
            var movieExists = db.Movies.Where(x => x.IsPlaying == true).Where(x => x.Genre == language).ToList();
            return movieExists;
        }

        public List<Movie> MovieByDirector(string name)
        {
            var movieExists = db.Movies.Where(x => x.IsPlaying == true).Where(x => x.Director == name).ToList();
            return movieExists;
        }

        public Messages CreateMovie(Movie movie)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var movieExist = MovieByTitle(movie.Title).FirstOrDefault(x => x.Director == movie.Director);
            if (movieExist != null)
            {
                messages.Message = ""+movie.Title+" Movie is already Registered.";
                return messages;
            }
            else
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "" + movie.Title +" Movie is Successfully Added";
                return messages;
            }
        }

       
        public Messages DeleteMovie(int movieId)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var movie = MovieById(movieId);
            if (movie == null)
            {
                messages.Message = "Movie Id ("+ movieId + ") is not found";
                return messages;
            }
            else
            {
                movie.IsPlaying = false;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "" + movie.Title + " Movie is Successfully Removed";
                return messages;
            }
        }

        public Messages UpdateMovie(Movie movie)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var movieExist = MovieById(movie.MovieId);
            var directorExist = MovieByTitle(movie.Title).FirstOrDefault(x => x.Director == movie.Director);
            if (movieExist == null)
            {
                messages.Message = "Movie Id is not found";
                return messages;
            }
            else if (directorExist != null && directorExist.MovieId != directorExist.MovieId)
            {
                messages.Message = "This Movie is already registered with Director"+movie.Director+"";
                return messages;
            }
            else
            {
                movieExist.Director = movie.Director;
                movieExist.Title = movie.Title;
                movieExist.Duration = movie.Duration;
                movieExist.Genre = movie.Genre;
                movieExist.CastAndCrew = movie.CastAndCrew;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "" + movie.Title + " Movie is Successfully Updated";
                return messages;
            }
        }
        //public MovieDTO SchedulesByMovieId(int id)
        //{
        //    MovieDTO movie = new MovieDTO();
        //    var moviename = db.Movies.FirstOrDefault(x => x.MovieId == id);
        //    movie.MovieName = moviename.Title;
        //    movie.Theaters = obj2;
        //    TheaterDetails obj2=new TheaterDetails();
        //    obj2.Screens = null;
        //    var scheduleList = db.Schedules.Where(x => x.MovieId == id).DistinctBy(x => x.ScreenId).ToList();
        //    List<Screen> screens = new List<Screen>();
        //    foreach (var schedule in scheduleList)
        //    {
        //        var obj = db.Screens.FirstOrDefault(x => x.ScreenId == schedule.ScreenId);
        //        screens.Add(obj);
        //    }
        //    var theaters = screens.DistinctBy(x => x.TheaterId).ToList();


        //    return movie;
        //}

        #region PrivateMethods

        //private List<ScheduleDetails> SchedulesList(int id)
        //{
        //    DateTime date = DateTime.Now;
        //    var timeValue = TimesValue(date);
        //    var screens = (from movie in db.Movies

        //                   join schedule in db.Schedules on movie.MovieId equals schedule.MovieId
        //                   join screen in db.Screens on schedule.ScreenId equals screen.ScreenId
        //                   join theater in db.Theaters on screen.TheaterId equals theater.TheaterId
        //                   join showTime in db.ShowTimes on schedule.ShowTimeId equals showTime.ShowTimeId
        //                   where movie.MovieId == id && (schedule.Date == date.Date && showTime.ShowTiming > timeValue || schedule.Date > date.Date)
        //                   select new ScheduleDetails()
        //                   {
        //                       ScheduleId = schedule.ScheduleId,
        //                       Date = schedule.Date,
        //                       ShowTime = TimingConvert.ConvertToString(showTime.ShowTiming),
        //                       AvailablePremiumSeats = schedule.AvailablePreSeats,
        //                       AvailableEliteSeats = schedule.AvailableEliSeats
        //                   }).ToList();

        //    return screens;
        //}
        private int TimesValue(DateTime date)
        {
            TimeSpan time = new TimeSpan(date.Hour, date.Minute, 0);
            return (TimingConvert.ConvertToInt(Convert.ToString(time)));
        }
        #endregion

    }
}
