using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplifiedIMDBApi.Entities;
using SimplifiedIMDBApi.Enums;
using SimplifiedIMDBApi.IRepository;

namespace SimplifiedIMDBApi.Controllers
{
    public class CrudMoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;

        public CrudMoviesController(IMovieRepository movieRepository, IUserRepository userRepository, IAdminRepository adminRepository)
        {
            _movieRepository = movieRepository;
            _userRepository = userRepository;
            _adminRepository = adminRepository;
        }

        
        [HttpPost("create movie")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CreateMovie(string movietitle, string description, DateTime release, MovieGenres genre)
        {
            var mv = await _userRepository.SearchMovieWithName(movietitle);
            if(mv!=null) {
                return BadRequest($"movie named {movietitle} already exist");
            }
            Movies movies = new Movies();
            movies.MovieTitle = movietitle;
            movies.Description = description;
            movies.ReleaseDate = release;
            movies.Genres = genre;
            return Ok(_movieRepository.CreateMovie(movies));
        }

        [HttpGet("find movie by Id")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> FindMoviebyId(int id)
        {
           var getMovie = await _movieRepository.FindMovieId(id); 
            if(getMovie!=null)
            {
                Ok(getMovie);
            }
            return NotFound();
        }

        [HttpGet("find movie by Name")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult FindMoviebyTitle(string title)
        {
            var getMovie = _userRepository.SearchMovieWithName(title);
            if(getMovie==null)
            {
                return NotFound($"movie with {title} not found");
            }

            return Ok(getMovie);
        }

        [HttpPut("edit movie")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateMovie(string findMovie, string updateTitle, string updateDescription, DateTime updateDate, MovieGenres updaeGenre)
        {
            if (string.IsNullOrEmpty(findMovie) || string.IsNullOrEmpty(updateTitle) || string.IsNullOrEmpty(updateDescription))
            {
                return BadRequest("invalid input");
            }
            var getMovie = await _userRepository.SearchMovieWithName(findMovie);
            if (getMovie != null)
            {
                getMovie.MovieTitle = updateTitle;
                getMovie.Description = updateDescription;
                getMovie.ReleaseDate = updateDate;
                getMovie.Genres = updaeGenre;
                try
                {
                    await _movieRepository.UpdateMovie(getMovie);
                    return Ok(getMovie);
                }
                catch (Exception ex)
                {
                    return StatusCode(500,"An error occurred while updating the movie");
                    throw new Exception(ex.Message);
                }
            }
            return NotFound();
        }

        [HttpDelete("delete Movie by Name")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(string name)
        {
            if (name != null)
            {
                await _movieRepository.DeleteMovie(name);
                return Ok();
            }
            return NotFound();
        }
        [HttpDelete("delete Movie by Id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = _movieRepository.FindMovieId(id);
            if (movie != null)
            {
                await _movieRepository.DeleteMovie(id);
                return Ok();
            }
            return NotFound();
        }
    }
}
