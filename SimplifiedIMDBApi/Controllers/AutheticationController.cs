using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplifiedIMDBApi.Entities;
using SimplifiedIMDBApi.Enums;
using SimplifiedIMDBApi.Implementations.ServiceImplementations;
using SimplifiedIMDBApi.IRepository;
using SimplifiedIMDBApi.Models;
using SimplifiedIMDBApi.Services;
using System.ComponentModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SimplifiedIMDBApi.Controllers
{
    public class AutheticationController : Controller
    {
        private readonly IVerifyPassword _checkPass;
        private readonly IMovieRepository _movieRepository;
        private readonly IAdminRepository adminRepository;
        private readonly IUserRepository userRepository;
        private readonly IPassHandler passwordHandler;
        private readonly ITokenGenerator tokenGenerator;
        private readonly ILogger logger;

        public AutheticationController(IUserRepository userRepository, IAdminRepository adminRepository, IPassHandler passwordHandler, ITokenGenerator tokenGenerator, ILogger logger, IVerifyPassword checkPass, IMovieRepository movieRepository)
        {
            this.userRepository = userRepository;
            this.passwordHandler = passwordHandler;
            this.tokenGenerator = tokenGenerator;
            this.adminRepository = adminRepository;
            this.logger = logger;
            _checkPass = checkPass;
            _movieRepository = movieRepository;
        }



        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginModel login)
        {
            try
            {
                User user = new User();
                if (await userRepository.GetUserByUsernameAsync(login.Username) == null) return NotFound("user not found");

                if( user.IsBanned) 
                { 
                    return Json(new { message = "You are banned and not allowed to access this resource.", user.Banlength }); 
                }


                if (!passwordHandler.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                {
                    BadRequest("invalid credentials");
                }
                
                var tken = tokenGenerator.CreateToken(user);
                user.MovieSugestions = (IEnumerable<Movies>)_movieRepository.GetMovieSugestion(user.UserId);
                return Ok(tken);
            }
            catch (Exception ex)
            {
                logger.LogError("Error ocured");
                logger.LogWarning(ex, "warning");
                return BadRequest();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegistrationModel register)
        {
            try
            {
                User user = await userRepository.CheckUserRole(register.Username);
                if (user != null) return BadRequest("user aleready exist");
                if (!(_checkPass.CheckConfirmPassword(register.Password, register.ConfirmPassword))) return BadRequest("Password does not match");
                passwordHandler.CreateSaltAndHash(register.Password, out var PasswordHash, out var PasswordSalt);
                user.PasswordHash = PasswordHash;
                user.PasswordSalt = PasswordSalt;
                switch (register.SetRoles)
                {
                    case Roles.Admin:
                        await userRepository.CreateUserAsync(user);
                        break;
                    case Roles.User:
                        await adminRepository.CreateAdminAsync(user);
                        break;
                }
                user.MovieSugestions = (IEnumerable<Movies>)_movieRepository.GetMovieSugestion(user.UserId);
                var token = tokenGenerator.CreateToken(user);
                return Ok(token);
            }
            catch(Exception e)
            {
                logger.LogError("Error ocured");
                logger.LogWarning(e.Message, "warning");
                return BadRequest("something went wrong");
            }
        }
    }
}
