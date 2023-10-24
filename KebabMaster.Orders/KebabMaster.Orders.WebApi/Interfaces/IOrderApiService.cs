using KebabMaster.Orders.Domain;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Filters;
using KebabMaster.Orders.Domain.Interfaces;
using KebabMaster.Orders.DTOs;

namespace KebabMaster.Orders.Interfaces;

public interface IOrderApiService
{
    public Task CreateOrder(OrderRequest order);
    public Task<ApplicationResponse<OrderResponse>> GetOrdersAsync(OrderFilter filter);
    public Task<OrderResponse> GetOrderById(Guid id);
    Task<IEnumerable<MenuItem>> GetMenuItems();
}