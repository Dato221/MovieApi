namespace SimplifiedIMDBApi.Entities
{
    public class Director
    {
        public int DirectorId { get; set; }
        public string DirectorName { get; set; } 
        public DateTime DirectorBirthDay { get; set; }
        public string Bio {  get; set; }
        public List<Movies> DirectedMovies { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Actor> Actors { get; set; }
    }
}
