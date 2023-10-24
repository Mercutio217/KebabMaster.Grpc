using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Interfaces;
using KebabMaster.Orders.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace KebabMaster.Orders.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly ApplicationDbContext _context;

    public MenuRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<MenuItem> GetMenuItemById(Guid id)
    {
        return _context.MenuItems.FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<IEnumerable<MenuItem>> GetMenuItems()
    {
        return _context.MenuItems;
    }
    
}