using System;

namespace EcommerceApp.exception
{
    // Custom Exception for Product Not Found
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message) { }
    }

    // Custom Exception for Order Not Found
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(string message) : base(message) { }
    }

    // Custom Exception for Database Connection Issues
    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException(string message) : base(message) { }
    }

}
