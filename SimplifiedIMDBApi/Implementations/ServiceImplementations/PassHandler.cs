using SimplifiedIMDBApi.Services;
using System.Security.Cryptography;
using System.Text;

namespace SimplifiedIMDBApi.Implementations.ServiceImplementations
{
    public class PassHandler : IPassHandler
    {
        public void CreateSaltAndHash(string password, out byte[] Passhash, out byte[] Passsalt)
        {
            using var hmac = new HMACSHA512();
            Passhash = hmac.Key;
            Passsalt = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public bool VerifyPasswordHash(string password, byte[] Passhash, byte[] Passsalt)
        {
            using var hmac = new HMACSHA512(Passsalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return HashArrayEquals(Passhash, computeHash);
        }

        public static bool HashArrayEquals(byte[] arr1, byte[] arr2)
        {
            if (arr1.Length != arr2.Length) { return false; }
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i]) { return false; }
            }
            return true;
        }
    }
 }
