using KebabMaster.Orders.Domain.Entities;

namespace KebabMaster.Orders.Domain.Interfaces;

public interface IMenuRepository
{
    public Task<MenuItem> GetMenuItemById(Guid id);
    Task<IEnumerable<MenuItem>> GetMenuItems();
}