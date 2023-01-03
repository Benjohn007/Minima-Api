using APIAuth.Models;

namespace APIAuth.Repositories
{
    public class MovieRepository
    {
        public static List<Movie> Movies = new()
        {
            new()
            {
                Id = 1,
                Title = "SexLife",
                Description = "Love and sex Life Movie and married couples",
                Rating = 7.6
            },
             new()
            {
                Id = 2,
                Title = "Enola Holmes",
                Description = "A young girl Passionate about finding herself",
                Rating = 6.6
            },
              new()
            {
                Id = 3,
                Title = "Far from home",
                Description = "Movie about a young Artist strieving to make a life for hmself at all cost",
                Rating = 8.6
            },
               new()
            {
                Id = 4,
                Title = "Enternal",
                Description = "Saga of an enternal race and power of immortality",
                Rating = 8.1
            },
                new()
            {
                Id = 5,
                Title = "SpiderMan",
                Description = "The Aftermath of the earthquack that storm",
                Rating = 7.6
            },
        };
    }
}
