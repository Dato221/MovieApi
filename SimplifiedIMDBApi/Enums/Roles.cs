using System.ComponentModel;
namespace SimplifiedIMDBApi.Enums
{
    [Description("roles")]
    public enum Roles
    {
        [Description("Admin")]
        Admin,
        [Description("User")]
        User,
        [Description("Guest")]
        Guest
    }
}
