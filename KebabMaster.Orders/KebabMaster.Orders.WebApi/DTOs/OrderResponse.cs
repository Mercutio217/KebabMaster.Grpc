using KebabMaster.Orders.Services;

namespace KebabMaster.Orders.DTOs;

public class OrderResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
}