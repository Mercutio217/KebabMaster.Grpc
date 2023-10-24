using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Exceptions;
using KebabMaster.Orders.Domain.Filters;
using KebabMaster.Orders.Domain.Interfaces;

namespace KebabMaster.Orders.Domain.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMenuRepository _menuRepository;
    
    public OrderService(IOrderRepository repository, IMenuRepository menuRepository)
    {
        _repository = repository;
        _menuRepository = menuRepository;
    }

    public async Task CreateOrder(Order order)
    {
        await ValidateOrderItems(order.OrderItems);
        
        await _repository.CreateOrder(order);
    }

    private async Task ValidateOrderItems(IEnumerable<OrderItem> orderOrderItems)
    {
        MenuItem menuItem;
        
        foreach (var item in orderOrderItems)
        {
            menuItem = await _menuRepository.GetMenuItemById(item.MenuItemId);
            if (menuItem is null)
                throw new MissingItemException(item.MenuItemId);
        }
    }

    public Task<IEnumerable<Order>> GetOrdersAsync(OrderFilter filter)
    {
        return _repository.GetOrdersAsync(filter);
    }

    public Task<Order> GetOrderByIdAsync(Guid id)
    {
        return _repository.GetOrderById(id);
    }
}