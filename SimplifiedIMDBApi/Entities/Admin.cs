namespace SimplifiedIMDBApi.Entities
{
    public class Admin : User
    {
        public string Admn = "Admin";
        public TimeSpan BanLendgth {  get; set; }   
        public ICollection<User> BanList { get; set; }
        public ICollection<AdminUser> AdminUser { get; set; }  
    }
}
