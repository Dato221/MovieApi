using Microsoft.AspNetCore.Mvc;
using SimplifiedIMDBApi.IRepository;

namespace SimplifiedIMDBApi.Controllers
{
    public class GuestViewController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public GuestViewController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet("movie sugestions")]
        public IActionResult MoviesForIndexPage()
        {
            return Ok(_movieRepository.ViewRandomListOFmovies());
        }
    }
}
