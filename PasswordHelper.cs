using System;
using System.Security.Cryptography;
using System.Text;

namespace EcommerceApp.dao
{
    // Password Helper Class for Hashing Passwords
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            string inputHashed = HashPassword(inputPassword);
            return inputHashed == hashedPassword;
        }
    }
}
