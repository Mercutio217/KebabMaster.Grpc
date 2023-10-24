using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Exceptions;
using KebabMaster.Orders.Domain.Tests.Tools;
using Xunit;

namespace KebabMaster.Orders.Domain.Tests.Entity;

public class OrderTests
{
    [Fact]
    public void Create_ValidEmail_ReturnsOrderInstance()
    {
        // Arrange
        string email = "test@example.com";
        Address address = TestData.Address;
        IEnumerable<OrderItem> orderItems = TestData.OrderItems;

        // Act
        Order order = Order.Create(email, address, orderItems);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(email, order.Email);
        Assert.Equal(address, order.Address);
        Assert.Equal(orderItems, order.OrderItems);
    }

    [Fact]
    public void Create_InvalidEmail_ThrowsInvalidEmailFormatException()
    {
        // Arrange
        string email = "invalid_email";
        Address address = TestData.Address;
        IEnumerable<OrderItem> orderItems = TestData.OrderItems;

        // Act & Assert
        Assert.Throws<InvalidEmailFormatException>(() => Order.Create(email, address, orderItems));
    }
}