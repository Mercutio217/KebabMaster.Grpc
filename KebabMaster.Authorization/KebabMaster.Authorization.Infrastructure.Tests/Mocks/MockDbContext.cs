using KebabMaster.Authorization.Infrastructure.Database;
using KebabMaster.Authorization.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace KebabMaster.Authorization.Infrastructure.Tests.Mocks;

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