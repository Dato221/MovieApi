using SimplifiedIMDBApi.Services;
using System.ComponentModel.DataAnnotations;

namespace SimplifiedIMDBApi.Implementations.ServiceImplementations
{
    public class VerifyPassword : IVerifyPassword
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool CheckConfirmPassword(string pass1, string pass2)
        {
            if(pass1 != null && pass2 != null)
            {
                Password = pass1;
                ConfirmPassword = pass2;
            }
            if (Password.Equals(ConfirmPassword))
            {
                return true;
            }
            return false;
        }
    }
}
