
namespace EcommerceApp.entity
{
    [Serializable]
    internal class InvalidCartItemException : Exception
    {
        public InvalidCartItemException()
        {
        }

        public InvalidCartItemException(string? message) : base(message)
        {
        }

        public InvalidCartItemException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}