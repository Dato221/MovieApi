namespace SimplifiedIMDBApi.Entities
{
    public class Actor
    {
        public string ActorId { get; set; }
        public string ActorName { get; set; }
        public string ActorBio {  get; set; }
        public DateTime ActorBirthDay { get; set; }
        //movie actor was cast
        public List<Movies> MovieRoles { get; set; }
        public ICollection<Movies> Movies { get; set; }
        public ICollection<ActorMovies> ActorMovies { get; set; }
        public ICollection<Director> Directors { get; set; }
    }
}
