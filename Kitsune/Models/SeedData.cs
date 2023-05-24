using Microsoft.EntityFrameworkCore;
using Kitsune.Data;
using Kitsune.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.SqlTypes;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new LibraryContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<LibraryContext>>()))
        {
            if (!context.User.Any())
            {
                // add user if no user
                context.User.AddRange(
                    new User
                    {

                        Name = "Makai",
                        Password = "password",
                        Balance = (Decimal)0.00
                    }
                );
            }
            context.SaveChanges();
            if (!context.Order.Any())
            {

                context.Order.AddRange(
                    new Order
                    {
                        UserId =context.User.First<User>().Id,
                        Status = "completed",
                        CreatedAt = DateTime.Parse("12:00"),
                        FinishedAt = DateTime.Parse("13:00")
                    }
                );
            }
            context.SaveChanges();
            if (!context.Product.Any())
            {

                context.Product.AddRange(
                    new Product { Name = "Scufita Rosie", Price = 20, Quantity = 2, Type = "Kids" }
                );
            }
            context.SaveChanges();
            if (!context.OrderItem.Any())
            {

                context.OrderItem.AddRange(
                    new OrderItem { Quantity = 2, OrderId = context.Order.First<Order>().Id, ProductId=context.Product.First<Product>().Id }
                );
            }
            context.SaveChanges();

        }
    }
}