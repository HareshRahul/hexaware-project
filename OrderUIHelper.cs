using System;
using System.Linq;
using EcommerceApp.dao;
using EcommerceApp.entity;
using EcommerceApp.exception;

namespace EcommerceApp.util
{
    public static class OrderUIHelper
    {
        public static void DisplayUserOrders(ICustomerRepository customerRepo, IProductRepository productRepo, int userId)
        {
            try
            {
                var orders = customerRepo.GetUserOrders(userId);
                var coupons = customerRepo.GetAllCoupons();

                if (orders.Count == 0)
                {
                    Console.WriteLine("No orders found.");
                    return;
                }

                Console.WriteLine("\n--- Your Orders ---");
                foreach (var order in orders)
                {
                    decimal finalPrice = order.TotalPrice;
                    decimal discount = 0;

                    if (!string.IsNullOrWhiteSpace(order.AppliedCouponCode))
                    {
                        var coupon = coupons.FirstOrDefault(c =>
                            c.CouponCode.Equals(order.AppliedCouponCode, StringComparison.OrdinalIgnoreCase));

                        if (coupon != null)
                        {
                            if (coupon.DiscountType == DiscountType.Flat)
                                discount = coupon.DiscountValue;
                            else if (coupon.DiscountType == DiscountType.Percent)
                                discount = order.TotalPrice * coupon.DiscountValue / 100;

                            finalPrice -= discount;

                            Console.WriteLine($"Order ID: {order.OrderId}, Original: {order.TotalPrice}, " +
                                              $"Discount: ₹{discount}, Final: {finalPrice}, Address: {order.ShippingAddress}, " +
                                              $"Coupon: {order.AppliedCouponCode}");
                        }
                        else
                        {
                            Console.WriteLine($"Order ID: {order.OrderId}, Total Price: {order.TotalPrice} " +
                                              $"(Unknown coupon '{order.AppliedCouponCode}'), Address: {order.ShippingAddress}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Order ID: {order.OrderId}, Total Price: {order.TotalPrice}, " +
                                          $"Address: {order.ShippingAddress}, No Coupon Applied");
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }
    }
}
