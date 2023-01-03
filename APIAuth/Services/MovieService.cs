using APIAuth.Models;
using APIAuth.Repositories;

namespace APIAuth.Services
{

    public class MovieService : IMovieService
    {
        public Movie Create(Movie movie)
        {
            movie.Id = MovieRepository.Movies.Count + 1;
            MovieRepository.Movies.Add(movie);
            return movie;
        }

        public bool Delete(int id)
        {
            var movie = MovieRepository.Movies.FirstOrDefault(x => x.Id == id);
            if(movie == null) return false;

            MovieRepository.Movies.Remove(movie);
            return true;
        }

        public Movie Get(int id)
        {
            var movie = MovieRepository.Movies.Find(x => x.Id == id);
            if(movie is null) return null;
            return movie;
        }

        public List<Movie> List()
        {
            var movie = MovieRepository.Movies;
            return movie;
        }

        public Movie Update(Movie newMovie)
        {
            var oldMovie = MovieRepository.Movies.FirstOrDefault(x => x.Id == newMovie.Id);
            if(oldMovie is null) return null;

            oldMovie.Title = newMovie.Title;
            oldMovie.Description = newMovie.Description;
            oldMovie.Rating = newMovie.Rating;

            return oldMovie;
        }
    }
}
