using KebabMaster.Orders.Domain;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KebabMaster.Orders.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    private readonly DatabaseOptions _databaseOptions;

    public ApplicationDbContext()
    {
    }
    public DbSet<Order> Orders { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Orders");
    }
}