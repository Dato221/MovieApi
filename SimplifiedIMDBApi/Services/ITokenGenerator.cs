using SimplifiedIMDBApi.Entities;

namespace SimplifiedIMDBApi.Services
{
    public interface ITokenGenerator
    {
        public string CreateToken(User user);
    }
}
