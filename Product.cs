using System;
using EcommerceApp.exception;

namespace EcommerceApp.entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }

        public Product() { }

        public Product(int productId, string name, decimal price, string description, int stockQuantity, int categoryId)
        {
            try
            {
                if (price < 0)
                    throw new ArgumentException("Price cannot be negative.");
                if (stockQuantity < 0)
                    throw new ArgumentException("Stock quantity cannot be negative.");

                ProductId = productId;
                Name = name ?? throw new ArgumentNullException(nameof(name), "Product name cannot be null.");
                Price = price;
                Description = description ?? "No description";
                StockQuantity = stockQuantity;
                CategoryId = categoryId;
            }
            catch (Exception ex)
            {
                throw new InvalidProductException("Failed to create Product: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"ProductId: {ProductId}, Name: {Name}, Price: ₹{Price}, Stock: {StockQuantity}, CategoryId: {CategoryId}";
        }
    }
}
