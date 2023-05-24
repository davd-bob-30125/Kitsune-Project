namespace Kitsune.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set;
        }

    }
}
