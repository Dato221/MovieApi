using SimplifiedIMDBApi.Entities;

namespace SimplifiedIMDBApi.IRepository
{
    public interface IMovieRepository
    {
        public Task CreateMovie(Movies movies);
        public Task UpdateMovie(Movies movie);
        public Task DeleteMovie(string movieName);
        public Task DeleteMovie(int movieId);
        public Task<IEnumerable<Movies>> ViewRandomListOFmovies();

        public Task<IEnumerable<Movies>> GetMovieSugestion(int id);
        public Task<IEnumerable<Movies>> AddMovieToFavoritelist(string moviename, string username);

        public Task<Movies> FindMovieId(int id);
    }
}
