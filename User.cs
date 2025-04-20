using System;
using System.Text.RegularExpressions;
using EcommerceApp.exception;

namespace EcommerceApp.entity
{
    public enum UserRole { Customer, Admin }

    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public User() { }

        public User(int userId, string name, string email, string password, UserRole role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be empty.");
                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                    throw new ArgumentException("Invalid email format.");
                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                    throw new ArgumentException("Password must be at least 6 characters long.");

                UserId = userId;
                Name = name;
                Email = email;
                Password = password;
                Role = role;
            }
            catch (Exception ex)
            {
                throw new InvalidUserException("Failed to create user: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"UserId: {UserId}, Name: {Name}, Email: {Email}, Role: {Role}";
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
