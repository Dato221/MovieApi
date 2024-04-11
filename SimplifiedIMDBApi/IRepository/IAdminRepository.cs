using SimplifiedIMDBApi.Entities;

namespace SimplifiedIMDBApi.IRepository
{
    public interface IAdminRepository
    {
        public Task CreateUsr(User user);
        public Task<User> GetAdminByIdAsync(int userId);
        public Task<User> GetAdminByUsernameAsync(string username);
        public Task<IEnumerable<User>> GetAllAdminsAsync();
        public Task<User> DeleteUserAsync(int userId);
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User> BanUser(int userId, int adminId, DateTime ban);
        public Task<User> GetUserByIdAsync(int userId);
        public Task<User> DeleteUserByUserName(string username);
        public Task CreateAdminAsync(User user);
        public TimeSpan BanLength(DateTime banlength);
        public Task<IEnumerable<User>> SelectAllUserWithSpecificAge(int lowend, int highend);
    }
}
