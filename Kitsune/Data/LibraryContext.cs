using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kitsune.Models;

namespace Kitsune.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext (DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Kitsune.Models.Product> Product { get; set; } = default!;

        public DbSet<Kitsune.Models.User> User { get; set; } = default!;

        public DbSet<Kitsune.Models.OrderItem> OrderItem { get; set; } = default!;

        public DbSet<Kitsune.Models.Order> Order { get; set; } = default!;

    }
}
