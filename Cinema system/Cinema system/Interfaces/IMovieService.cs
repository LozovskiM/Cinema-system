using System.Collections.Generic;
using CinemaSystem.Models;

namespace CinemaSystem.Interfaces
{
    public interface IMovieService
    {
        bool CheckMovieExists(int movieId);

        bool CheckMovieExists(MovieFullView movie);

        bool CheckMovieDate(MovieFullView movie);

        int CreateMovie(MovieFullView movie);

        IEnumerable<MovieView> GetMovies();

        MovieFullView GetMovie(int movieId);

        void EditMovie(MovieFullView movie, int movieId);

        void DeleteMovie(int movieId);
    }
}
