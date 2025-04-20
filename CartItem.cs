using System;
using EcommerceApp.exception; // Custom exception namespace

namespace EcommerceApp.entity
{
    public class CartItem
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Constructor with input validation and exception handling
        public CartItem(string productName, decimal price, int quantity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productName))
                    throw new ArgumentException("Product name cannot be null or empty.");

                if (price < 0)
                    throw new ArgumentException("Price cannot be negative.");

                if (quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero.");

                ProductName = productName;
                Price = price;
                Quantity = quantity;
            }
            catch (Exception ex)
            {
                throw new InvalidCartItemException("Error initializing CartItem: " + ex.Message);
            }
        }
    }
}
