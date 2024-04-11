using SimplifiedIMDBApi.Enums;

namespace SimplifiedIMDBApi.Models
{
    public record LoginModel(string Username, string Password, Roles SetRole);
}
