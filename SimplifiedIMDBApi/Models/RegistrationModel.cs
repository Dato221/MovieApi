using SimplifiedIMDBApi.Enums;

namespace SimplifiedIMDBApi.Models
{
    public record RegistrationModel(string Username, string Password, string ConfirmPassword, string Email, Roles SetRoles);
}
