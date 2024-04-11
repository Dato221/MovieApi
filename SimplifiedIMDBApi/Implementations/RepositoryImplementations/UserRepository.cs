using Microsoft.EntityFrameworkCore;
using SimplifiedIMDBApi.DataAccess;
using SimplifiedIMDBApi.Entities;
using SimplifiedIMDBApi.Enums;
using SimplifiedIMDBApi.IRepository;

namespace SimplifiedIMDBApi.Implementations.RepositoryImplementations
{
    public class UserRepository : IUserRepository
    {
        private readonly SimpleMoviesDbContext _MovieDbCont;
        private readonly IAdminRepository admin;
        public UserRepository(SimpleMoviesDbContext movieDbCont, IAdminRepository admin)
        {
            _MovieDbCont = movieDbCont;
            this.admin = admin;
        }


        public async Task CreateUserAsync(User user)
        {
            try
            {
                _MovieDbCont.Users.Add(user);
                await _MovieDbCont.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to create user", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ooppss something went wrong O_o .", ex);
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _MovieDbCont.Users.FindAsync(username);
            if(user!=null && user.UserRole == Enums.Roles.User)
            {
                return user;
            }
            return null;
        }

        public async Task<Movies> SearchMovieWithName(string movieName)
        {
            var movies = await _MovieDbCont.Movies.FindAsync(movieName);
            if(movies ==  null)
            {
                return null;
            }
            return movies;
        }
        public async Task<IEnumerable<Movies>> AddMovieTOFavoritelist(string moviename, string username)
        {
            var mv =  _MovieDbCont.Movies.Find(moviename);
            var usr =  _MovieDbCont.Users.Find(username);
            if (mv == null || usr == null)
            {
                return null;
            }
             usr.FavoriteMovies.Add(mv);
             _MovieDbCont.SaveChanges();
            return usr.FavoriteMovies;
           
        }
        public async Task<IEnumerable<Movies>> ClearFavorite(string username, Movies mv)
        {
            var usr = await _MovieDbCont.Users.FindAsync(username);
            if(usr!=null)
            {
                foreach(var movie in  usr.FavoriteMovies) {
                    if(movie==mv) usr.FavoriteMovies.Remove(movie);
                    _MovieDbCont.SaveChanges();
                }
            }
            return usr.FavoriteMovies;
        }
        public async Task<User> CheckUserRole(string uName)
        {
            User? user = new User();
            user = (user.UserRole) switch
            {
                Roles.Admin => await admin.GetAdminByUsernameAsync(uName),
                Roles.User => await GetUserByUsernameAsync(uName),
                _ => null
            };
            return user;
        }
    }
}
