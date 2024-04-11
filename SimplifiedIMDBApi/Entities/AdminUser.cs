namespace SimplifiedIMDBApi.Entities
{
    public class AdminUser
    {
        public int UserId { get; set; }
        public User Users { get; set; }
        public int AdminId { get; set; }
        public Admin Admin { get; set; }
    }
}
