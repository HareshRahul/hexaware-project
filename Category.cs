using System;
using EcommerceApp.exception; // Reference to the custom exceptions

namespace EcommerceApp.entity
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Category() { }

        public Category(int categoryId, string categoryName)
        {
            try
            {
                if (categoryId <= 0)
                    throw new ArgumentException("Category ID must be greater than zero.");

                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("Category name cannot be null or empty.");

                CategoryId = categoryId;
                CategoryName = categoryName;
            }
            catch (Exception ex)
            {
                throw new InvalidCategoryException("Error creating Category: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"CategoryId: {CategoryId}, Name: {CategoryName}";
        }
    }
}
