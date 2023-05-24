namespace Kitsune.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual User User { get; set; }

    }
}
