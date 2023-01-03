using APIAuth.Models;

namespace APIAuth.Services
{
    public interface IMovieService
    {
        public Movie Create(Movie movie);
        public Movie Get(int id);
        public List<Movie> List();
        public Movie Update(Movie newMovie);
        public bool Delete(int id);
    }
}
