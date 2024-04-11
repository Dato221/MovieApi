namespace SimplifiedIMDBApi.Entities
{
    public class ActorMovies
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int MovieId { get; set; }
        public Movies Movies  { get; set; }
    }
}
