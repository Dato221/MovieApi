namespace SimplifiedIMDBApi.Services
{
    public interface IVerifyPassword
    {
        public bool CheckConfirmPassword(string str1, string str2);
    }
}
