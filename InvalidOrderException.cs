using System;

namespace EcommerceApp.exception
{
    public class InvalidOrderException : Exception
    {
        public InvalidOrderException(string message) : base(message) { }
    }
    public class InvalidOrderItemException : Exception
    {
        public InvalidOrderItemException(string message) : base(message) { }
    }
    public class InvalidProductException : Exception
    {
        public InvalidProductException(string message) : base(message) { }
    }
    public class InvalidReviewException : Exception
    {
        public InvalidReviewException(string message) : base(message) { }
    }
  
    public class InvalidWishlistItemException : Exception
    {
        public InvalidWishlistItemException(string message) : base(message)
        {
        }
    }
    public class DaoOperationException : Exception
    {
        public DaoOperationException(string message) : base(message)
        {
        }

        public DaoOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
       
    }

}
