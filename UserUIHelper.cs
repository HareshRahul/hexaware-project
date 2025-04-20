using System;
using EcommerceApp.dao;
using EcommerceApp.entity;

namespace EcommerceApp.util
{
    public static class UserUIHelper
    {
 

        public static void RegisterUser(IUserRepository userRepo)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            Console.Write("Enter Role (Customer/Admin): ");
            string roleStr = Console.ReadLine();

            if (Enum.TryParse(roleStr, true, out UserRole role))  // case-insensitive parse
            {
                User user = new User(0, name, email, password, role);
                bool registered = userRepo.RegisterUser(user);
                Console.WriteLine(registered ? "User registered successfully!" : "Registration failed.");
            }
            else
            {
                Console.WriteLine("Invalid role. Please enter 'Customer' or 'Admin'.");
            }
        }
        public static User LoginUser(IUserRepository userRepo)
        {
            Console.Write("Email: ");
            string loginEmail = Console.ReadLine();
            Console.Write("Password: ");
            string loginPass = Console.ReadLine();

            if (userRepo.Login(loginEmail, loginPass, out User loggedUser))
            {
                Console.WriteLine($"\nWelcome {loggedUser.Name}! Role: {loggedUser.Role}\n");
                return loggedUser;
            }
            else
            {
                Console.WriteLine("Invalid credentials. Try again.\n");
                return null;
            }
        }
    }
}
