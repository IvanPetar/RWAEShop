using System.Security.Cryptography;
using System.Text;

namespace RWAEShopMVC.Utils
{
    public static class PasswordHashProvider
    {
        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var provider = RandomNumberGenerator.Create())
            {
                provider.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string GetHash(string password, string salt)
        {
            var sha = SHA256.Create();
            var combinedBytes = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
