﻿using PKTickets.Interfaces;
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
                messages.Message = "" + movie.Title + " Movie is Successfully deleted";
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
                messages.Message = "Movie Id("+movie.MovieId+") is not found";
                return messages;
            }
            else if (directorExist != null && directorExist.MovieId != directorExist.MovieId)
            {
                messages.Message = "This Movie is already registered with Director"+movie.Director+"";
                return messages;
            }
            else if (languageExist != null && languageExist.MovieId != movieExist.MovieId)
            {
                messages.Message = "This Movie is already registered with Language"+movie.Language + "";
                return messages;
            }
            else
            {
                movieExist.Language = movie.Language;
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

     
    }
}
