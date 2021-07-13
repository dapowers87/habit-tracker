using System;
using System.Security.Cryptography;
using Domain.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Security
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool CheckPassword(string givenPassword, string hashedPassword);
    }

    public class PasswordHasher: IPasswordHasher
    {
        private readonly IOptions<AuthenticationSettings> config;

        public PasswordHasher(IOptions<AuthenticationSettings> config)
        {
            this.config = config;
        }
        
        public string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string result = Convert.ToBase64String(hashBytes);

            return result;
        }

        public bool CheckPassword(string givenPassword, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(givenPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i=0; i < 20; i++)
            {
                if (hashBytes[i+16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}