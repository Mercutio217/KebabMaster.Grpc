using KebabMaster.Orders.Domain.Entities.Base;
using KebabMaster.Orders.Domain.Exceptions;

namespace KebabMaster.Orders.Domain.Entities;

public class OrderItem : Entity
{
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }
    
    private OrderItem() { }

    private OrderItem(Guid menuItemId, int quantity)
    {
        MenuItemId = menuItemId;
        Quantity = quantity;
    }

    public static OrderItem Create(Guid menuItemId, int quantity)
    {
        if (menuItemId == default)
            throw new MissingMandatoryPropertyException<MenuItem>(nameof(MenuItemId));

        if (quantity > 50 || quantity < 1)
            throw new InvalidQuantityOfProperty(nameof(Quantity), quantity);
        
        return new OrderItem(menuItemId, quantity);
    }
}