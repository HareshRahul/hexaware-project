using System;

namespace EcommerceApp.exception
{
    public class InvalidCategoryException : Exception
    {
        public InvalidCategoryException(string message) : base(message) { }
    }

}
