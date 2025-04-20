using System;

namespace EcommerceApp.exception
{
    public class InvalidCouponException : Exception
    {
        public InvalidCouponException(string message) : base(message) { }
    }
    public class InvalidUserException : Exception
    {
        public InvalidUserException() { }

        public InvalidUserException(string message)
            : base(message) { }

        public InvalidUserException(string message, Exception inner)
            : base(message, inner) { }
    }
}
