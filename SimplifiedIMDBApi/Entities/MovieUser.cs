namespace SimplifiedIMDBApi.Entities
{
    public class MovieUser
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int MovieId { get; set; }
        public Movies Movie { get; set; }
    }
}
