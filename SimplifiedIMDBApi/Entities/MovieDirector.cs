namespace SimplifiedIMDBApi.Entities
{
    public class MovieDirector
    {
        public int DirectorId { get; set; }
        public Director Directors { get; set; }
        public int MovieId { get; set; }
        public Movies Movies { get; set; }
    }
}

