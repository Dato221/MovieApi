namespace SimplifiedIMDBApi.Services
{
    public interface IPassHandler
    {
        public void CreateSaltAndHash(string password, out byte[] Passhash, out byte[] Passsalt);
        public bool VerifyPasswordHash(string password, byte[] Passhash, byte[] Passsalt);
    }
}
