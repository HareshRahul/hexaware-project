using System;
using EcommerceApp.exception;

namespace EcommerceApp.entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; }
        public string AppliedCouponCode { get; set; }

        public Order() { }

        public Order(int orderId, int userId, DateTime orderDate, decimal totalPrice, string shippingAddress, string appliedCouponCode)
        {
            try
            {
                if (totalPrice <= 0)
                    throw new ArgumentException("Total price must be greater than 0.");

                if (string.IsNullOrWhiteSpace(shippingAddress))
                    throw new ArgumentException("Shipping address is required.");

                if (orderDate > DateTime.Now)
                    throw new ArgumentException("Order date cannot be in the future.");

                OrderId = orderId;
                UserId = userId;
                OrderDate = orderDate;
                TotalPrice = totalPrice;
                ShippingAddress = shippingAddress;
                AppliedCouponCode = appliedCouponCode;
            }
            catch (Exception ex)
            {
                throw new InvalidOrderException("Failed to create Order: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"OrderId: {OrderId}, UserId: {UserId}, Date: {OrderDate}, Total: ₹{TotalPrice}, Coupon: {AppliedCouponCode}, Address: {ShippingAddress}";
        }
    }
}
