using SimplifiedIMDBApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplifiedIMDBApi.Entities
{
    public class Movies
    {
        public int MovieId { get; set; }

        [Required]
        [StringLength(255)]
        public string MovieTitle { get; set; }

        public string Description { get; set; }

        [Range(0.0, 10.0)]
        public decimal? MovieScore { get; set; } 

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; } 

        //public string PosterUrl { get; set; } 
        public MovieGenres Genres { get; set; }
        public int Budget {  get; set; }
        public int BoxOffice {  get; set; }
        public List<Actor> MovioeCast { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<MovieUser> UserMovies { get; set; }
        public ICollection<Actor> Actors { get; set; }
        public ICollection<ActorMovies> ActorMovies { get; set; }
    }
}
