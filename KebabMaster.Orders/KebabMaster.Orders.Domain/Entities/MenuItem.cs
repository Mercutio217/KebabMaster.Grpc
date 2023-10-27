using KebabMaster.Orders.Domain.Entities.Base;

namespace KebabMaster.Orders.Domain.Entities;

public class MenuItem : Entity
{
    public string Name { get; init; }
    public double Price { get; init; }

    private MenuItem() { }
    public MenuItem(Guid id, string name, double price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
    
    public MenuItem(string name, double price)
    {
        Name = name;
        Price = price;
    }
}