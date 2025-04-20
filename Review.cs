namespace EcommerceApp.entity
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public override string ToString()
        {
            return $"Rating: {Rating} | Comment: {Comment} | Date: {ReviewDate.ToShortDateString()}";
        }
    }
}
