using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SimplifiedIMDBApi.DataAccess;
using SimplifiedIMDBApi.Entities;
using SimplifiedIMDBApi.Enums;
using SimplifiedIMDBApi.IRepository;

namespace SimplifiedIMDBApi.Implementations.RepositoryImplementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly SimpleMoviesDbContext _MovieDbCont;

        public AdminRepository(SimpleMoviesDbContext MovieDbCont)
        {
                _MovieDbCont = MovieDbCont;
        }

        public async Task<User> BanUser(int userId, int adminId, DateTime ban)
        {
            var user = await _MovieDbCont.Users.FindAsync(userId);
            //var admin = await _MovieDbCont.Admin.FindAsync(adminId);
            var admin = await _MovieDbCont.Users.FindAsync(adminId);
            if (user != null && admin != null && admin.UserRole == Roles.Admin)
            {
                user.Banlength = BanLength(ban);
                user.IsBanned = true;
                await _MovieDbCont.SaveChangesAsync();
            }

            return user;
        }

        public TimeSpan BanLength(DateTime banlength)
        {
            return banlength - DateTime.Now;
        }


        public async Task<User> DeleteUserAsync(int userId)
        {
            var user = await _MovieDbCont.Users.FindAsync(userId);

            if (user != null)
            {
                _MovieDbCont.Users.Remove(user);
                await _MovieDbCont.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User> DeleteUserByUserName(string username)
        {
            var user = await _MovieDbCont.Users.FindAsync(username);

            if (user != null)
            {
                _MovieDbCont.Users.Remove(user);
                await _MovieDbCont.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User> GetAdminByIdAsync(int userId)
        {
            var user = await _MovieDbCont.Users.FindAsync(userId);
            if(user != null && user.UserRole == Enums.Roles.Admin)
            {
                return user;
            }
            return null;
        }

        public async Task<User> GetAdminByUsernameAsync(string username)
        {
            var user =  await _MovieDbCont.Users.FirstOrDefaultAsync(u =>u.UserName  == username && u.UserRole == Roles.Admin);
            return user;
        }

        public async Task<IEnumerable<User>> GetAllAdminsAsync()
        {
            return await _MovieDbCont.Users
                              .Where(u => u.UserRole == Roles.Admin)
                              .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _MovieDbCont.Users
                              .Where(u => u.UserRole == Roles.User)
                              .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            if (userId == 0 || userId < 0) return null;
            var user = await _MovieDbCont.Users.FindAsync(userId);
            return user;
        }

        public async Task CreateAdminAsync(User user)
        {
            try
            {
                _MovieDbCont.Users.Add(user);
                await _MovieDbCont.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to create admin", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ooooppss something went wrong O_o .", ex);
            }
        }

        public async Task CreateUsr(User user)
        {
            try
            {
                if (user != null)
                {
                    _MovieDbCont.Users.Add(user);
                    await _MovieDbCont.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("error ocured could not add user", ex);
            }
        }

        public async Task<IEnumerable<User>> SelectAllUserWithSpecificAge(int lowend, int highend)
        {
            var selectedUsers = new List<User>();
            var today = DateTime.UtcNow;
            if (lowend != 0 && highend != 0 && lowend!=highend)
            {
                foreach (var user in _MovieDbCont.Users)
                {
                    int age = today.Year - user.Age.Year;
                    if (age >= lowend && age <= highend)
                    {
                        selectedUsers.Add(user);
                    }
                }
            }
            _MovieDbCont.SaveChanges();
            return selectedUsers;
        }
    }
}
