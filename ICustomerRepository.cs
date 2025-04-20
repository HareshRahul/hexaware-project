using System.Collections.Generic;
using EcommerceApp.entity;

namespace EcommerceApp.dao
{
    public interface ICustomerRepository
    {
        bool AddToCart(int userId, int productId, int quantity);
        bool RemoveFromCart(int userId, int productId);
        List<Cart> GetCartItems(int userId);

        bool AddToWishlist(int userId, int productId);
        bool RemoveFromWishlist(int userId, int productId);
        List<WishlistItem> GetWishlist(int userId);

        bool ApplyCoupon(string code, out Coupon validCoupon);
        List<Coupon> GetAllCoupons();
        void DisplayCartItems(int userId, IProductRepository productRepo);

        bool PlaceOrder(int userId, string address, string appliedCouponCode, out int orderId);
        List<Order> GetUserOrders(int userId);
        List<OrderItem> GetOrderItems(int orderId);
    }
}
