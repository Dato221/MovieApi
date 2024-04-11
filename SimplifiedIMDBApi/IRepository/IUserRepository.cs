using SimplifiedIMDBApi.Entities;

namespace SimplifiedIMDBApi.IRepository
{
    public interface IUserRepository
    {
        public Task CreateUserAsync(User user);
        
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<Movies> SearchMovieWithName(string movieName);
        public Task<User> CheckUserRole(string uName);
        public Task<IEnumerable<Movies>> ClearFavorite(string username, Movies mv);
    }
}
