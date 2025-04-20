using System;
using EcommerceApp.exception;

namespace EcommerceApp.entity
{
    public class WishlistItem
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; }

        public WishlistItem() { }

        public WishlistItem(int wishlistId, int userId, int productId, DateTime createdAt)
        {
            try
            {
                if (userId <= 0)
                    throw new InvalidWishlistItemException("UserId must be greater than 0.");

                if (productId <= 0)
                    throw new InvalidWishlistItemException("ProductId must be greater than 0.");

                if (createdAt > DateTime.Now)
                    throw new InvalidWishlistItemException("CreatedAt cannot be a future date.");

                WishlistId = wishlistId;
                UserId = userId;
                ProductId = productId;
                CreatedAt = createdAt;
            }
            catch (InvalidWishlistItemException ex)
            {
                Console.WriteLine($"[WishlistItem Error] {ex.Message}");
            }
        }

        public override string ToString()
        {
            return $"WishlistId: {WishlistId}, UserId: {UserId}, ProductId: {ProductId}, Added: {CreatedAt}";
        }
    }
}
