using SimplifiedIMDBApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SimplifiedIMDBApi.Entities
{

    public class User : BaseEntity
    {
        public int UserId { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string DisplayName => DisplayName is null || DisplayName == string.Empty ? UserName : DisplayName;
        // gender
        public DateTime Age { get; set; }
        public Roles UserRole { get; set; }
        public bool IsBanned { get; set; }
        public List<Movies> FavoriteMovies { get; set; } 
        public List<Movies> Watched { get; set; } 
        public TimeSpan Banlength { get; set; }
        public IEnumerable<Movies> MovieSugestions { get; set; }
        public ICollection<Movies> Movies { get; set; }
        public ICollection<MovieUser> UserMovies { get; set; }
        public ICollection <AdminUser> AdminUsers { get; set; }
        public ICollection<Director> Directors { get; set; }
        public ICollection<Actor> Actors { get; set; }
        public ICollection<MovieDirector> Director { get; set; }
    }
}

