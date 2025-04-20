using System;
using EcommerceApp.exception; // Assuming custom exceptions are placed here

namespace EcommerceApp.entity
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Default constructor
        public Cart() { }

        // Parameterized constructor with exception handling
        public Cart(int cartId, int userId, int productId, int quantity)
        {
            try
            {
                if (cartId <= 0)
                    throw new ArgumentException("CartId must be greater than zero.");
                if (userId <= 0)
                    throw new ArgumentException("UserId must be greater than zero.");
                if (productId <= 0)
                    throw new ArgumentException("ProductId must be greater than zero.");
                if (quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero.");

                CartId = cartId;
                UserId = userId;
                ProductId = productId;
                Quantity = quantity;
            }
            catch (Exception ex)
            {
                // Log the exception or handle appropriately
                throw new InvalidCartDataException("Error initializing Cart: " + ex.Message);
            }
        }

        // Navigation property 
        public Product Product { get; set; }

        // Overriding ToString method for cart details
        public override string ToString()
        {
            return $"CartId: {CartId}, UserId: {UserId}, ProductId: {ProductId}, Quantity: {Quantity}";
        }
    }
}
