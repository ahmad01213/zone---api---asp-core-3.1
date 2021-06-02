using System;
using System.Security.Cryptography;
using System.Text;

namespace SecuringWebApiUsingJwtAuthentication.Helpers
{
    public class HashingHelper
    {
        public static string HashUsingPbkdf2(string password, string salt)
        {
            using var bytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(salt))), 10000, HashAlgorithmName.SHA256);
            var derivedRandomKey = bytes.GetBytes(32);
            var hash = Convert.ToBase64String(derivedRandomKey);
            return hash;
        }
    }
}