
namespace EcommerceApp.entity
{
    [Serializable]
    internal class InvalidCartDataException : Exception
    {
        public InvalidCartDataException()
        {
        }

        public InvalidCartDataException(string? message) : base(message)
        {
        }

        public InvalidCartDataException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}