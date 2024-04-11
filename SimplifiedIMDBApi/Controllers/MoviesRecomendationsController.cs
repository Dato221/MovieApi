using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplifiedIMDBApi.IRepository;

namespace SimplifiedIMDBApi.Controllers
{
    public class MoviesRecomendationsController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        public MoviesRecomendationsController(IMovieRepository movieRepository, IUserRepository userRepository)
        {
            _movieRepository = movieRepository;
            _userRepository = userRepository;
        }

        [Authorize(Roles ="Admin")]
        [Authorize(Roles = "User")]
        [HttpGet("movie recomendations")]
        public async Task<IActionResult> MovieRecomendations(string username)  
        {
            var usr = await _userRepository.GetUserByUsernameAsync(username);
            if(usr == null)
            {
                return NotFound("no such user");
            }
            return Ok(usr.MovieSugestions);
        }
    }
}
