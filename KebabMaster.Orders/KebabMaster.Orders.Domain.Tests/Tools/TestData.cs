using KebabMaster.Orders.Domain.Entities;

namespace KebabMaster.Orders.Domain.Tests.Tools;

public static class TestData
{
    public static Order Order => Order.Create("validEmail@email.com", Address, OrderItems);
    public static Address Address => Address.Create("Super street", 1, 1);

    public static IEnumerable<OrderItem> OrderItems => new List<OrderItem>()
    {
        OrderItem.Create(new Guid("a1676b07-e05e-4156-9b50-2f7dfd923773"), 1),
        OrderItem.Create(new Guid("59fc68ac-f156-434c-82f5-45e0a44df344"), 2),
        OrderItem.Create(new Guid("6833bf16-568c-46c4-9426-308bd686d8f9"), 1)
    };
}