using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CoreBoard.Helpers
{
    public static class Crypto
    {
        const int IterationCount = 1000;
        const int SaltSize = 128;
        const int SubkeySize = 256;

        public static string SHA1(string input)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            return Hash(sha1, input);
        }

        public static string SHA256(string input)
        {
            var sha256 = System.Security.Cryptography.SHA256.Create();
            return Hash(sha256, input);
        }
        
        private static string Hash(HashAlgorithm alg, string input) =>
            BitConverter.ToString(alg.ComputeHash(Encoding.UTF8.GetBytes(input))).ToLower().Replace("-", "");

        public static string HashPassword(string password)
        {
            var salt = new byte[SaltSize / 8];

            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(salt);
            }
            return Convert.ToBase64String(GenerateSubkey(password, salt));
        }
        
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            var bytes = Convert.FromBase64String(hashedPassword);
            var salt = bytes.Skip(1).Take(SaltSize / 8).ToArray();

            return hashedPassword == Convert.ToBase64String(GenerateSubkey(password, salt));
        }

        private static byte[] GenerateSubkey(string password, byte[] salt)
        {
            var subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, IterationCount, SubkeySize / 8);

            using (var ms = new MemoryStream())
            {
                ms.WriteByte(0);
                ms.Write(salt, 0, salt.Length);
                ms.Write(subkey, 0, subkey.Length);

                return ms.ToArray();
            }
        }
    }
}
