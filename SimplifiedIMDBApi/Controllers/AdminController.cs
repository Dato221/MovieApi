using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using SimplifiedIMDBApi.Entities;
using SimplifiedIMDBApi.Enums;
using SimplifiedIMDBApi.Implementations.RepositoryImplementations;
using SimplifiedIMDBApi.IRepository;
using SimplifiedIMDBApi.Services;

namespace SimplifiedIMDBApi.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository adminRepository;
        private readonly IUserRepository userRepository;
        private readonly IMovieRepository movieRepository;
        private readonly IPassHandler passHandler;

        public AdminController(IAdminRepository adminRepository, IUserRepository userRepository, IMovieRepository movieRepository, IPassHandler passHandler)
        {
            this.adminRepository = adminRepository;
            this.userRepository = userRepository;
            this.movieRepository = movieRepository;
            this.passHandler = passHandler;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("get user info by username")]
        public async Task<IActionResult> GetUserInfo(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("Username is required.");
            }

            var user = await userRepository.GetUserByUsernameAsync(userName);   

            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("ban user")]
        public async Task<IActionResult> BanUnbanUser(string userName, bool ban, DateTime banlength)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest();
            }
            var getuser = await userRepository.GetUserByUsernameAsync(userName);
            if (getuser == null)
            {
                return NotFound();
            }
            getuser.IsBanned = ban;
            getuser.Banlength = adminRepository.BanLength(banlength);
            return Ok(getuser);
        }
        //delete user
        [Authorize(Roles="Admin")]
        [HttpDelete("delete user")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    await adminRepository.DeleteUserByUserName(username);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw new Exception("Error ocured while trying to delete user ", ex);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("get all users")]
        public Task<IEnumerable<User>> GetAllUsers()
        {
              return adminRepository.GetAllUsersAsync();
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("ctreate user")]
        public async Task<IActionResult> CreateUsers(string uname, string pass, string email, Roles rl)
        {
            User user = new User();
            user.UserName = uname;
            passHandler.CreateSaltAndHash(pass, out var PasswordHash, out var PasswordSalt);
            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;
            user.EmailAddress = email;
            user.UserRole = rl;
            await adminRepository.CreateUsr(user);
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get all users with between specific age")]
        public async Task<IActionResult> GetUsersByAge(int from, int to)
        {
            try
            {
                var users = await adminRepository.SelectAllUserWithSpecificAge(from, to);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
                throw new Exception($"Error occurred: {ex.Message}");
            }
        }
    }
}
