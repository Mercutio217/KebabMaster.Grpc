using KebabMaster.Orders.Infrastructure.Database;
using KebabMaster.Orders.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KebabMaster.Orders.Infrastructure.Tests.Mocks;

public class MockDbContext : ApplicationDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("OrdersDb");    
    }

    public MockDbContext() : base(Options.Create(new DatabaseOptions()))
    {
    }
}