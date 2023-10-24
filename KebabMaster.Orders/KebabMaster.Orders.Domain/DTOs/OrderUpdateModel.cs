using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Exceptions;

namespace KebabMaster.Orders.Domain.DTOs;

public class OrderUpdateModel
{
    public Guid Id { get; private set; }
    public Address Address { get; private set; }
    public IEnumerable<OrderItem> OrderItems { get; private set; }

    public OrderUpdateModel(Guid id, Address address, IEnumerable<OrderItem> orderItems)
    {
        Id = id;
        Address = address;
        OrderItems = orderItems;
    }

    public static OrderUpdateModel Create(Guid? guid, Address address, IEnumerable<OrderItem> items)
    {
        if (!guid.HasValue)
            throw new MissingMandatoryPropertyException<OrderUpdateModel>(nameof(Id));

        return new(guid.Value, address, items);
    }
}