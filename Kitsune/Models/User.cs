using System.Data.SqlTypes;

namespace Kitsune.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Password { get; set; }

        public Decimal Balance { get; set; }
        public virtual ICollection<Order> Orders { get; set;}
    }
}
