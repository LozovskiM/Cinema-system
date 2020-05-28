using CinemaSystem.Interfaces;
using CinemaSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace CinemaSystem.Services
{
    public class MovieService : IMovieService
    {
        private readonly CinemaDBContext _db;

        public MovieService(CinemaDBContext db)
        {
            _db = db;
        }

        public bool CheckMovieExists(int movieId)
        {
            return _db.Movies.Any(m => (m.Id == movieId && !m.IsDeleted));
        }

        public bool CheckMovieExists(MovieFullView movie)
        {
            return _db.Movies.Any(m => (m.Title == movie.Title && !m.IsDeleted));
        }

        public bool CheckMovieDate(MovieFullView movie)
        { 
            return movie.EndingDate > movie.ReleaseDate;
        }

        public int CreateMovie(MovieFullView movie)
        {
            var newMovie = new Movie
            {
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                EndingDate = movie.EndingDate,
                Description = movie.Description
            };

            _db.Movies.Add(newMovie);
            _db.SaveChanges();

            return newMovie.Id;
        }

        public void EditMovie(MovieFullView movie, int movieId)
        {
            var editMovie = _db.Movies.Find(movieId);

            editMovie.Title = movie.Title;
            editMovie.ReleaseDate = movie.ReleaseDate;
            editMovie.EndingDate = movie.EndingDate;
            editMovie.Description = movie.Description;

            _db.SaveChanges();
        }

        public IEnumerable<MovieView> GetMovies()
        {
            return _db.Movies
                .Where(m => m.IsDeleted == false)
                .Select(m => new MovieView
                {
                    Id = m.Id,
                    Title = m.Title
                });
        }

        public MovieFullView GetMovie(int movieId)
        {
            var movie = _db.Movies.Find(movieId);

            return new MovieFullView
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                EndingDate = movie.EndingDate,
                Description = movie.Description
            };
        }

        public void DeleteMovie(int movieId)
        {
            var deleteMovie = _db.Movies.Find(movieId);

            deleteMovie.IsDeleted = true;

            _db.SaveChanges();
        }

    }
}
