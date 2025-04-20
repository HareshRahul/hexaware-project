using System;
using EcommerceApp.exception;

namespace EcommerceApp.entity
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public OrderItem() { }

        public OrderItem(int orderItemId, int orderId, int productId, int quantity)
        {
            try
            {
                if (quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than 0.");

                OrderItemId = orderItemId;
                OrderId = orderId;
                ProductId = productId;
                Quantity = quantity;
            }
            catch (Exception ex)
            {
                throw new InvalidOrderItemException("Failed to create OrderItem: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"OrderItemId: {OrderItemId}, OrderId: {OrderId}, ProductId: {ProductId}, Quantity: {Quantity}";
        }
    }
}
