using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using EcommerceApp.entity;

namespace EcommerceApp.dao
{
    public class CustomerRepositoryImpl : ICustomerRepository
    {
        public bool AddToCart(int userId, int productId, int quantity)
        {
            try
            {
                // Ensure quantity is greater than 0 before attempting to insert
                if (quantity <= 0)
                {
                    Console.WriteLine("Please enter a quantity greater than zero.");
                    return false;
                }

                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "INSERT INTO Cart (UserId, ProductId, Quantity) VALUES (@UserId, @ProductId, @Quantity)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                // Check if the error relates to the CHECK constraint on Quantity column
                if (ex.Message.Contains("CK__Cart__Quantity"))
                {
                    Console.WriteLine("Please enter a quantity greater than zero.");
                }
                else
                {
                    // General error message
                    Console.WriteLine($"Error in AddToCart: {ex.Message}");
                }
                return false;
            }
        }

        public bool RemoveFromCart(int userId, int productId)
        {
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "DELETE FROM Cart WHERE UserId = @UserId AND ProductId = @ProductId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in RemoveFromCart: {ex.Message}");
                return false;
            }
        }

        public List<Cart> GetCartItems(int userId)
        {
            List<Cart> cartItems = new List<Cart>();
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "SELECT * FROM Cart WHERE UserId = @UserId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cartItems.Add(new Cart(
                            (int)reader["CartId"],
                            (int)reader["UserId"],
                            (int)reader["ProductId"],
                            (int)reader["Quantity"]
                        ));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in GetCartItems: {ex.Message}");
            }
            return cartItems;
        }

        public bool AddToWishlist(int userId, int productId)
        {
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "INSERT INTO Wishlist (UserId, ProductId) VALUES (@UserId, @ProductId)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in AddToWishlist: {ex.Message}");
                return false;
            }
        }

        public bool RemoveFromWishlist(int userId, int productId)
        {
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "DELETE FROM Wishlist WHERE UserId = @UserId AND ProductId = @ProductId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in RemoveFromWishlist: {ex.Message}");
                return false;
            }
        }

        public List<WishlistItem> GetWishlist(int userId)
        {
            List<WishlistItem> wishlist = new List<WishlistItem>();
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "SELECT * FROM Wishlist WHERE UserId = @UserId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        wishlist.Add(new WishlistItem(
                            (int)reader["WishlistId"],
                            (int)reader["UserId"],
                            (int)reader["ProductId"],
                            (DateTime)reader["CreatedAt"]
                        ));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in GetWishlist: {ex.Message}");
            }
            return wishlist;
        }

        public bool ApplyCoupon(string code, out Coupon validCoupon)
        {
            validCoupon = null;
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    string query = "SELECT * FROM Coupons WHERE CouponCode = @Code AND ExpiryDate >= GETDATE()";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Code", code);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        validCoupon = new Coupon(
                            reader["CouponCode"].ToString(),
                            Enum.Parse<DiscountType>(reader["DiscountType"].ToString()),
                            (decimal)reader["DiscountValue"],
                            (DateTime)reader["ExpiryDate"]
                        );
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in ApplyCoupon: {ex.Message}");
            }
            return false;
        }

        public List<Coupon> GetAllCoupons()
        {
            List<Coupon> coupons = new List<Coupon>();
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Coupons", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        coupons.Add(new Coupon(
                            reader["CouponCode"].ToString(),
                            Enum.Parse<DiscountType>(reader["DiscountType"].ToString()),
                            (decimal)reader["DiscountValue"],
                            (DateTime)reader["ExpiryDate"]
                        ));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in GetAllCoupons: {ex.Message}");
            }
            return coupons;
        }

        public bool PlaceOrder(int userId, string address, string appliedCouponCode, out int orderId)
        {
            orderId = 0;
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    SqlTransaction tx = conn.BeginTransaction();
                    try
                    {
                        // Get cart items
                        SqlCommand getCart = new SqlCommand("SELECT * FROM Cart WHERE UserId = @UserId", conn, tx);
                        getCart.Parameters.AddWithValue("@UserId", userId);
                        List<Cart> cartItems = new List<Cart>();
                        using (SqlDataReader reader = getCart.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cartItems.Add(new Cart(
                                    (int)reader["CartId"],
                                    (int)reader["UserId"],
                                    (int)reader["ProductId"],
                                    (int)reader["Quantity"]
                                ));
                            }
                        }

                        if (cartItems.Count == 0) return false;

                        decimal totalPrice = 0;
                        foreach (var item in cartItems)
                        {
                            SqlCommand getProduct = new SqlCommand("SELECT Price FROM Products WHERE ProductId = @Pid", conn, tx);
                            getProduct.Parameters.AddWithValue("@Pid", item.ProductId);
                            totalPrice += (decimal)getProduct.ExecuteScalar() * item.Quantity;
                        }

                        if (!string.IsNullOrEmpty(appliedCouponCode))
                        {
                            SqlCommand getCoupon = new SqlCommand("SELECT DiscountType, DiscountValue FROM Coupons WHERE CouponCode = @Code", conn, tx);
                            getCoupon.Parameters.AddWithValue("@Code", appliedCouponCode);
                            using (SqlDataReader reader = getCoupon.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string type = reader["DiscountType"].ToString();
                                    decimal value = (decimal)reader["DiscountValue"];
                                    if (type == "Flat") totalPrice -= value;
                                    else if (type == "Percent") totalPrice -= (totalPrice * value / 100);
                                }
                            }
                        }

                        SqlCommand insertOrder = new SqlCommand("INSERT INTO Orders (UserId, OrderDate, TotalPrice, ShippingAddress, AppliedCouponCode) OUTPUT INSERTED.OrderId VALUES (@Uid, GETDATE(), @Price, @Addr, @Coupon)", conn, tx);
                        insertOrder.Parameters.AddWithValue("@Uid", userId);
                        insertOrder.Parameters.AddWithValue("@Price", totalPrice);
                        insertOrder.Parameters.AddWithValue("@Addr", address);
                        insertOrder.Parameters.AddWithValue("@Coupon", (object)appliedCouponCode ?? DBNull.Value);
                        orderId = (int)insertOrder.ExecuteScalar();

                        foreach (var item in cartItems)
                        {
                            SqlCommand insertItem = new SqlCommand("INSERT INTO OrderItems (OrderId, ProductId, Quantity) VALUES (@Oid, @Pid, @Qty)", conn, tx);
                            insertItem.Parameters.AddWithValue("@Oid", orderId);
                            insertItem.Parameters.AddWithValue("@Pid", item.ProductId);
                            insertItem.Parameters.AddWithValue("@Qty", item.Quantity);
                            insertItem.ExecuteNonQuery();
                        }

                        SqlCommand clearCart = new SqlCommand("DELETE FROM Cart WHERE UserId = @UserId", conn, tx);
                        clearCart.Parameters.AddWithValue("@UserId", userId);
                        clearCart.ExecuteNonQuery();

                        tx.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        Console.WriteLine($"Error in PlaceOrder: {ex.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PlaceOrder: {ex.Message}");
                return false;
            }
        }

        public List<Order> GetUserOrders(int userId)
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Orders WHERE UserId = @UserId", conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Order(
                            (int)reader["OrderId"],
                            (int)reader["UserId"],
                            (DateTime)reader["OrderDate"],
                            (decimal)reader["TotalPrice"],
                            reader["ShippingAddress"].ToString(),
                            reader["AppliedCouponCode"].ToString()
                        ));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in GetUserOrders: {ex.Message}");
            }
            return orders;
        }
        public void DisplayCartItems(int userId, IProductRepository productRepo)
        {
            var cartItems = GetCartItems(userId);
            var allProducts = productRepo.GetAllProducts();

            if (cartItems.Count == 0)
            {
                Console.WriteLine("Your cart is empty.");
            }
            else
            {
                Console.WriteLine("\n--- Your Cart ---");
                foreach (var item in cartItems)
                {
                    var product = allProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
                    if (product != null)
                    {
                        Console.WriteLine($"Product: {product.Name}");
                        Console.WriteLine($"Price: {product.Price}");
                        Console.WriteLine($"Quantity: {item.Quantity}");
                        Console.WriteLine("-------------------------");
                    }
                    else
                    {
                        Console.WriteLine($"Product with ID {item.ProductId} not found.");
                    }
                }
            }
        }
        public List<OrderItem> GetOrderItems(int orderId)
        {
            List<OrderItem> items = new List<OrderItem>();
            try
            {
                using (SqlConnection conn = DBUtility.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM OrderItems WHERE OrderId = @OrderId", conn);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        items.Add(new OrderItem(
                            (int)reader["OrderItemId"],
                            (int)reader["OrderId"],
                            (int)reader["ProductId"],
                            (int)reader["Quantity"]
                        ));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error in GetOrderItems: {ex.Message}");
            }
            return items;
        }
       

    }
}
