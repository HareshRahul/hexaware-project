using EcommerceApp.dao;
using EcommerceApp.entity;
using System;
using System.Linq;
using EcommerceApp.exception;

namespace EcommerceApp.util
{
    public class CustomerUIHelper
    {
        public static void DisplayCustomerOrders(ICustomerRepository customerRepo, int userId)
        {
            var orders = customerRepo.GetUserOrders(userId);
            var allCoupons = customerRepo.GetAllCoupons();

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
            }
            else
            {
                Console.WriteLine("\n--- Your Orders ---");
                foreach (var order in orders)
                {
                    decimal finalP = order.TotalPrice * 0.9m; // Base discount
                    decimal discount = 10; // Default discount
                    string orderCouponCode = order.AppliedCouponCode;
                    string deliveryDate = DateTime.Now.AddDays(2).ToShortDateString();

                    Coupon appliedCoupon = null;
                    if (!string.IsNullOrWhiteSpace(orderCouponCode))
                    {
                        appliedCoupon = allCoupons.FirstOrDefault(c =>
                            c.CouponCode.Equals(orderCouponCode, StringComparison.OrdinalIgnoreCase));

                        if (appliedCoupon != null)
                        {
                            if (appliedCoupon.DiscountType == DiscountType.Flat)
                                discount = appliedCoupon.DiscountValue;
                            else if (appliedCoupon.DiscountType == DiscountType.Percent)
                                discount = finalP * appliedCoupon.DiscountValue / 100;

                            finalP -= discount;
                        }
                    }

                    Console.WriteLine("-------------------------------");
                    Console.WriteLine($"Order ID       : {order.OrderId}");
                    Console.WriteLine($"Total Price    : {order.TotalPrice}");
                    Console.WriteLine($"Discount       : {discount}");
                    Console.WriteLine($"Final Price    : {finalP}");
                    Console.WriteLine($"Coupon Applied : {orderCouponCode ?? "None"}");
                    Console.WriteLine($"Address        : {order.ShippingAddress}");
                    Console.WriteLine($"Delivery Date  : {deliveryDate}");
                }
            }
        }
        public static void ProcessOrder(ICustomerRepository customerRepo, IProductRepository productRepo, User user)
        {
            var userCart = customerRepo.GetCartItems(user.UserId);
            var productList = productRepo.GetAllProducts();

            if (userCart.Count == 0)
            {
                Console.WriteLine("Cart is empty. Cannot place order.");
                return;
            }

            decimal totalPrice = 0;
            Console.WriteLine("\n--- Order Summary ---");
            foreach (var item in userCart)
            {
                var product = productList.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    Console.WriteLine($"Product: {product.Name}, Quantity: {item.Quantity}, Price: {product.Price * item.Quantity}");
                    totalPrice += product.Price * item.Quantity;
                }
            }

            Console.WriteLine($"\nTotal before discount: ₹{totalPrice}");

            Console.Write("Enter Shipping Address: ");
            string shipping = Console.ReadLine();

            Console.Write("Enter Coupon Code (optional): ");
            string couponCode = Console.ReadLine()?.Trim().ToLower();

            decimal finalPrice = totalPrice;
            decimal discountAmount = 0;

            if (couponCode == "haresh10")
            {
                discountAmount = totalPrice * 0.10m;
                finalPrice = totalPrice - discountAmount;
                Console.WriteLine($"Coupon Applied! You saved ₹{discountAmount}");
            }
            else if (!string.IsNullOrWhiteSpace(couponCode))
            {
                Console.WriteLine("Invalid coupon code. No discount applied.");
            }

            Console.WriteLine($"Final Amount to Pay: ₹{finalPrice}");

            Console.WriteLine("\n--- Payment Options ---");
            Console.WriteLine("1. Credit Card\n2. UPI\n3. COD");
            Console.Write("Choose Payment Method (1-3): ");
            string payChoice = Console.ReadLine();
            string paymentMethod = payChoice switch
            {
                "1" => "Credit Card",
                "2" => "UPI",
                "3" => "Cash on Delivery",
                _ => "Unknown"
            };

            if (paymentMethod == "Unknown")
            {
                Console.WriteLine("Invalid payment method. Order cancelled.");
                return;
            }

            Console.WriteLine($"Processing payment via {paymentMethod}...");
            Thread.Sleep(1000);
            Console.WriteLine("Payment successful!");

            bool ordered = customerRepo.PlaceOrder(user.UserId, shipping, couponCode, out int orderId);

            if (ordered)
            {
                Console.WriteLine("\n=========== RECEIPT ===========");
                Console.WriteLine($"Order ID       : {orderId}");
                Console.WriteLine($"Customer       : {user.Name}");
                Console.WriteLine($"Shipping Addr  : {shipping}");
                Console.WriteLine($"Payment Method : {paymentMethod}");
                Console.WriteLine($"Date           : {DateTime.Now.ToShortDateString()}");
                Console.WriteLine("-------------------------------");
                foreach (var item in userCart)
                {
                    var product = productList.FirstOrDefault(p => p.ProductId == item.ProductId);
                    if (product != null)
                    {
                        Console.WriteLine($"{product.Name} x {item.Quantity} = ₹{product.Price * item.Quantity}");
                    }
                }
                Console.WriteLine("-------------------------------");
                Console.WriteLine($"Total          : {totalPrice}");
                Console.WriteLine($"Discount       : {discountAmount}");
                Console.WriteLine($"Final Amount   : {finalPrice}");
                Console.WriteLine("===============================\n");
            }
            else
            {
                Console.WriteLine("Order failed.");
            }
        }
        public static void ViewWishlist(ICustomerRepository customerRepo, IProductRepository productRepo, int userId)
        {
            try
            {
                var wishlist = customerRepo.GetWishlist(userId);
                if (wishlist.Count == 0)
                {
                    Console.WriteLine("Your wishlist is empty.");
                }
                else
                {
                    Console.WriteLine("\n--- Your Wishlist ---");
                    var allProducts = productRepo.GetAllProducts();
                    foreach (var item in wishlist)
                    {
                        var product = allProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
                        if (product != null)
                            Console.WriteLine(product);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }

        public static void RemoveFromWishlist(ICustomerRepository customerRepo, int userId)
        {
            try
            {
                Console.Write("Enter Product ID to remove from wishlist: ");
                if (int.TryParse(Console.ReadLine(), out int removeId))
                {
                    bool removed = customerRepo.RemoveFromWishlist(userId, removeId);
                    Console.WriteLine(removed ? "Removed from wishlist." : "Item not found in wishlist.");
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e);
            }
        }

        public static void PromptWishlistAdd(ICustomerRepository customerRepo, int userId, string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input) && input.EndsWith("W"))
                {
                    if (int.TryParse(input[..^1], out int productId))
                    {
                        bool added = customerRepo.AddToWishlist(userId, productId);
                        Console.WriteLine(added ? "Added to wishlist!" : "Failed to add to wishlist.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid ProductId format.");
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
