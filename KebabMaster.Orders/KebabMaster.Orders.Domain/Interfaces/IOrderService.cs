using KebabMaster.Orders.Domain.DTOs;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Entities.Base;
using KebabMaster.Orders.Domain.Filters;

namespace KebabMaster.Orders.Domain.Interfaces;

public interface IOrderService
{
    public Task CreateOrder(Order order);
    public Task<IEnumerable<Order>> GetOrdersAsync(OrderFilter filter);
    public Task<Order> GetOrderByIdAsync(Guid id);

}