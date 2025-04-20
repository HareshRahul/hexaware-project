using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using EcommerceApp.entity;
using EcommerceApp.exception;

namespace EcommerceApp.dao
{
    public class UserRepositoryImpl : IUserRepository
    {
        // Registers a new user
        public bool RegisterUser(User user)
        {
            try
            {
                // Validate email format
                if (!IsValidEmail(user.Email))
                {
                    throw new InvalidUserException("Failed to create user: Invalid email format.");
                }

                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    // Hash the password before saving it
                    string hashedPassword = PasswordHelper.HashPassword(user.Password);

                    string query = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@Name, @Email, @Password, @Role)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", user.Name);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);  // Store hashed password
                    cmd.Parameters.AddWithValue("@Role", user.Role.ToString());

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (InvalidUserException ex)
            {
                // Handle custom InvalidUserException (invalid email format)
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            catch (SqlException ex)
            {
                // Log SQL exception (e.g., connection issues, query issues)
                Console.WriteLine($"Error occurred while registering user: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return false;
            }
        }

        // Logs in a user by verifying email and password
        public bool Login(string email, string password, out User loggedUser)
        {
            loggedUser = null;

            try
            {
                // Validate email format
                if (!IsValidEmail(email))
                {
                    throw new InvalidUserException("Failed to log in: Invalid email format.");
                }

                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "SELECT * FROM Users WHERE Email = @Email";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string storedHashedPassword = reader["Password"].ToString();

                        // Verify if the password matches the hashed password stored in the database
                        if (PasswordHelper.VerifyPassword(storedHashedPassword, password))
                        {
                            loggedUser = new User
                            {
                                UserId = (int)reader["UserId"],
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = storedHashedPassword,  // You may choose not to store the password in the user object
                                Role = Enum.Parse<UserRole>(reader["Role"].ToString())
                            };
                            return true;
                        }
                    }
                }
            }
            catch (InvalidUserException ex)
            {
                // Handle custom InvalidUserException
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (SqlException ex)
            {
                // Log SQL exception (e.g., connection issues, query issues)
                Console.WriteLine($"Error occurred while logging in: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            return false;
        }

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                return emailRegex.IsMatch(email);
            }
            catch (Exception ex)
            {
                // Handle any unexpected error that might occur during regex matching
                Console.WriteLine($"Error validating email: {ex.Message}");
                return false;
            }
        }

        // Retrieves a user by UserId
        public User GetUserById(int userId)
        {
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "SELECT * FROM Users WHERE UserId = @UserId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = (int)reader["UserId"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = Enum.Parse<UserRole>(reader["Role"].ToString())
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log SQL exception (e.g., connection issues, query issues)
                Console.WriteLine($"Error occurred while retrieving user: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            return null;
        }
        public bool DeleteCustomerByAdmin(int deleteId, string enteredPassword)
        {
            // Validate admin password (Note: You should replace this with a secure mechanism)
            const string adminPassword = "admin123"; // Replace with a secure check, e.g., hashed password
            if (enteredPassword != adminPassword)
            {
                Console.WriteLine("Invalid password. Access denied.");
                return false;
            }

            // Get the user by ID
            var user = GetUserById(deleteId);
            if (user != null && user.Role == UserRole.Customer)
            {
                return DeleteUser(deleteId); // Attempt to delete user
            }
            else
            {
                Console.WriteLine("Customer not found or user is not a customer.");
                return false;
            }
        }
        // Retrieves all users
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "SELECT * FROM Users";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserId = (int)reader["UserId"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = Enum.Parse<UserRole>(reader["Role"].ToString())
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log SQL exception (e.g., connection issues, query issues)
                Console.WriteLine($"Error occurred while retrieving users: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            return users;
        }

        // Deletes a user by UserId
        public bool DeleteUser(int userId)
        {
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "DELETE FROM Users WHERE UserId = @UserId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                // Log SQL exception (e.g., connection issues, query issues)
                Console.WriteLine($"Error occurred while deleting user: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return false;
            }
        }
    }
}
