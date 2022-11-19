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
                messages.Message = "Movie is already Registered.";
                return messages;
            }
            else
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Movie is succssfully added";
                return messages;
            }
        }

        public Messages DubMovie(DubDTO dub)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var movieExist = db.Movies.Where(x => x.Title == dub.Title).FirstOrDefault(x => x.Director == dub.Director);
            if (movieExist == null)
            {
                messages.Message = "Movie Title and Director Name is Not Registered.";
                return messages;
            }
            var dubExist = MovieByTitle(dub.Title).FirstOrDefault(x => x.Language == dub.Language);
            if (dubExist != null)
            {
                messages.Message = "Movie is already Dubbed to this Language.";
                return messages;
            }
            else
            {
                Movie movie = new Movie();
                movie.Title = movieExist.Title;
                movie.Language = dub.Language;
                movie.Duration = movieExist.Duration;
                movie.Genre = movieExist.Genre;
                movie.Director = movieExist.Director;
                movie.CastAndCrew = movieExist.CastAndCrew;
                db.Movies.Add(movie);
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Movie is succssfully Dubbed";
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
                messages.Message = "Movie Id is not found";
                return messages;
            }
            else
            {
                movie.IsPlaying = false;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Movie is succssfully deleted";
                return messages;
            }
        }

        public Messages UpdateMovie(Movie movie)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var movieExist = MovieById(movie.MovieId);
            var languageExist = MovieByTitle(movie.Title).FirstOrDefault(x => x.Language == movie.Language);
            var directorExist = MovieByTitle(movie.Title).FirstOrDefault(x => x.Director == movie.Director);
            if (movieExist == null)
            {
                messages.Message = "Movie Id is not found";
                return messages;
            }
            else if (directorExist != null && directorExist.MovieId != directorExist.MovieId)
            {
                messages.Message = "This Movie is already registered with this Director";
                return messages;
            }
            else if (languageExist != null && languageExist.MovieId != movieExist.MovieId)
            {
                messages.Message = "Movie Language is already registered";
                return messages;
            }
            else
            {
                movieExist.Language = movie.Language;
                movieExist.Title = movie.Title;
                movieExist.Duration = movie.Duration;
                movieExist.Genre = movie.Genre;
                movieExist.CastAndCrew = movie.CastAndCrew;
                db.SaveChanges();
                messages.Success = true;
                messages.Message = "Movie is succssfully updated";
                return messages;
            }
        }

        public Messages DubMovieUpdate(Movie movie)
        {
            Messages messages = new Messages();
            messages.Success = false;
            var movieExist = MovieById(movie.MovieId);
            if (movieExist == null)
            {
                messages.Message = "Movie Id is Not Registered.";
                return messages;
            }
            var movies = MovieByTitle(movieExist.Title).Where(x => x.CastAndCrew == movie.CastAndCrew).ToList();
            if (movies.Count > 1)
            {
                movies.ForEach(c =>
                {
                    c.Title = movie.Title;
                    c.Duration = movie.Duration;
                    c.Genre = movie.Genre;
                    c.CastAndCrew = movie.CastAndCrew;
                    c.Director = movie.Director;
                });
                db.UpdateRange(movies);
                db.SaveChanges();
                //foreach (Movie dubMovie in movies)
                //{
                //    dubMovie.Title = movie.Title;
                //    dubMovie.Duration = movie.Duration;
                //    dubMovie.Genre = movie.Genre;
                //    dubMovie.CastAndCrew = movie.CastAndCrew;
                //}   
                messages.Success = true;
                messages.Message = "Movie is succssfully updated";
                return messages;
            }
            else
            {
                messages.Message = "Movie is Not Dubbed.";
                return messages;
            }
        }
    }
}
