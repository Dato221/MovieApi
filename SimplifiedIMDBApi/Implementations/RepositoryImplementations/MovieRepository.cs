using Microsoft.EntityFrameworkCore;
using SimplifiedIMDBApi.DataAccess;
using SimplifiedIMDBApi.Entities;
using SimplifiedIMDBApi.IRepository;

namespace SimplifiedIMDBApi.Implementations.RepositoryImplementations
{
    public class MovieRepository : IMovieRepository
    {
       private readonly SimpleMoviesDbContext _MovDbCont;
       private readonly ILogger _logger;
        public MovieRepository(SimpleMoviesDbContext movDbCont, ILogger logger)
        {
            _MovDbCont = movDbCont;
            _logger = logger;
        }

        public async Task CreateMovie(Movies movie)
        {
            try
            {
                _MovDbCont.Movies.Add(movie);
                await _MovDbCont.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to add movie", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ooppss something went wrong O_o .", ex);
            }
        }
        public async Task DeleteMovie(string movieName)
        {
            try
            {
                var movie = await _MovDbCont.Movies.FindAsync(movieName);
                if (movie != null)
                {
                    _MovDbCont.Movies.Remove(movie);
                    _MovDbCont.SaveChanges();
                }
            }catch(DbUpdateException ex)
            {
                throw new Exception("something went wrong", ex);
            }catch (Exception ex)
            {
                throw new Exception($"could not remove {movieName}", ex);
            }
        }

        public async Task DeleteMovie(int movieId)
        {
            try
            {
                var movie = await _MovDbCont.Movies.FindAsync(movieId);
                if (movie != null)
                {
                    _MovDbCont.Movies.Remove(movie);
                    _MovDbCont.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("something went wrong", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"could not remove with specified {movieId} ", ex);
            }
        }
        public async Task<Movies> FindMovieId(int id)
        {
            var movie = await _MovDbCont.Movies.FindAsync(id);
            return movie;
        }
        public async Task<IEnumerable<Movies>> GetMovieSugestion(int id)
        {
            var userGenres = await _MovDbCont.Users
                .Where(u => u.UserId == id)
                .SelectMany(u => u.FavoriteMovies.Select(m => m.Genres))
                .ToListAsync();

            var genreCounts = userGenres
                .GroupBy(g => g)
                .Select(g => new { Genre = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(4);

            
            var topGenres = genreCounts.Select(g => g.Genre).ToList();
            //exclude top genres and pick random 10 movies with favorite 4 gnere
            var recommendations = await _MovDbCont.Movies
            .Where(m => !m.Users.Any(u => u.UserId == id))
            .Where(m => topGenres.Contains(m.Genres))
            .Take(10)
            .ToListAsync();

            return recommendations;
        }


        public async Task<IEnumerable<Movies>> ViewRandomListOFmovies()
        {
            int amount = 20;
            Random r = new Random();
            var latestMovies = await _MovDbCont.Movies
                .OrderByDescending(m => m.ReleaseDate)
                .Take(amount)
                .ToListAsync();
            //shufle movies
            for (int n = latestMovies.Count-1; n > 1; n--)
            {
                int j = r.Next(0, n);
                Movies temp = latestMovies[n];
                latestMovies[n] = latestMovies[j];
                latestMovies[j] = temp;
            }
            return latestMovies;
        }


        public async Task UpdateMovie(Movies movie)
        {
            _MovDbCont.Movies.Update(movie);
            await _MovDbCont.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movies>> AddMovieToFavoritelist(string moviename, string username)
        {
            try
            {
                var usr = await _MovDbCont.Users.FindAsync(username);
                var mv = await _MovDbCont.Movies.FindAsync(moviename);
                if (usr != null && mv != null)
                {
                    usr.FavoriteMovies.Add(mv);
                    _MovDbCont.SaveChanges();
                    return usr.FavoriteMovies;
                }
                return Enumerable.Empty<Movies>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error ocured while trying to add movie");
                return Enumerable.Empty<Movies>();
            }
        }
    }
}
