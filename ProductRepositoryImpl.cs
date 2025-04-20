using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using EcommerceApp.entity;

namespace EcommerceApp.dao
{
    public class ProductRepositoryImpl : IProductRepository
    {
        public bool CreateCategory(Category category)
        {
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "INSERT INTO Categories (CategoryName) VALUES (@Name)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", category.CategoryName);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions (e.g., connection issues, query errors)
                Console.WriteLine($"SQL Error in CreateCategory: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error in CreateCategory: {ex.Message}");
                return false;
            }
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                List<Category> categories = new List<Category>();
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(new Category((int)reader["CategoryId"], reader["CategoryName"].ToString()));
                    }
                }
                return categories;
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                Console.WriteLine($"SQL Error in GetAllCategories: {ex.Message}");
                return new List<Category>();
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error in GetAllCategories: {ex.Message}");
                return new List<Category>();
            }
        }

        public bool AddProduct(Product product)
        {
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "INSERT INTO Products (Name, Price, Description, StockQuantity, CategoryId) VALUES (@Name, @Price, @Desc, @Qty, @CatId)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Desc", product.Description);
                    cmd.Parameters.AddWithValue("@Qty", product.StockQuantity);
                    cmd.Parameters.AddWithValue("@CatId", product.CategoryId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                Console.WriteLine($"SQL Error in AddProduct: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error in AddProduct: {ex.Message}");
                return false;
            }
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                List<Product> products = new List<Product>();
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Products", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product(
                            (int)reader["ProductId"],
                            reader["Name"].ToString(),
                            (decimal)reader["Price"],
                            reader["Description"].ToString(),
                            (int)reader["StockQuantity"],
                            (int)reader["CategoryId"]
                        ));
                    }
                }
                return products;
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                Console.WriteLine($"SQL Error in GetAllProducts: {ex.Message}");
                return new List<Product>();
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error in GetAllProducts: {ex.Message}");
                return new List<Product>();
            }
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            try
            {
                List<Product> products = new List<Product>();
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE CategoryId = @CategoryId", conn);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product(
                            (int)reader["ProductId"],
                            reader["Name"].ToString(),
                            (decimal)reader["Price"],
                            reader["Description"].ToString(),
                            (int)reader["StockQuantity"],
                            (int)reader["CategoryId"]
                        ));
                    }
                }
                return products;
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                Console.WriteLine($"SQL Error in GetProductsByCategory: {ex.Message}");
                return new List<Product>();
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error in GetProductsByCategory: {ex.Message}");
                return new List<Product>();
            }
        }

        public bool DeleteProduct(int productId)
        {
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductId = @ProductId", conn);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                Console.WriteLine($"SQL Error in DeleteProduct: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error in DeleteProduct: {ex.Message}");
                return false;
            }
        }

        
       
        
    }
}
