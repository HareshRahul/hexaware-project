using System;
using EcommerceApp.exception;

namespace EcommerceApp.entity
{
    public enum DiscountType { Flat, Percent }

    public class Coupon
    {
        public string CouponCode { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Coupon() { }

        public Coupon(string couponCode, DiscountType discountType, decimal discountValue, DateTime expiryDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(couponCode))
                    throw new ArgumentException("Coupon code cannot be empty.");

                if (discountValue <= 0)
                    throw new ArgumentException("Discount value must be greater than zero.");

                if (expiryDate <= DateTime.Now)
                    throw new ArgumentException("Coupon expiry date must be in the future.");

                CouponCode = couponCode;
                DiscountType = discountType;
                DiscountValue = discountValue;
                ExpiryDate = expiryDate;
            }
            catch (Exception ex)
            {
                throw new InvalidCouponException("Failed to create Coupon: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"Coupon: {CouponCode}, Type: {DiscountType}, Value: ₹{DiscountValue}, Expiry: {ExpiryDate.ToShortDateString()}";
        }
    }
}
