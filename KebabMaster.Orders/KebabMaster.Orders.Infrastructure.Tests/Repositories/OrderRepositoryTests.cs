using KebabMaster.Orders.Domain;
using KebabMaster.Orders.Domain.DTOs;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Interfaces;
using KebabMaster.Orders.Infrastructure.Repositories;
using KebabMaster.Orders.Infrastructure.Tests.Mocks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KebabMaster.Orders.Infrastructure.Tests.Repositories;

public class OrderRepositoryTests
{
    [Fact]
    public async Task CreateOrder_ShouldAddOrderToContext()
    {
        // Arrange
        var context = new MockDbContext();
        var orderRepository = new OrderRepository(context);
        
        var order = Order.Create("test@example.com", Address.Create("Street", 1, null), new List<OrderItem>());

        // Act
        await orderRepository.CreateOrder(order);

        // Assert
        var result = await context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        Assert.NotNull(result);
        Assert.Equal(order.Email, result.Email);
        Assert.Equal(order.Address, result.Address);
        Assert.Equal(order.OrderItems, result.OrderItems);
    }

    [Fact]
    public async Task GetOrdersAsync_ShouldReturnFilteredOrders()
    {
        // Arrange
        var context = new MockDbContext();
        var orderRepository = new OrderRepository(context);
        
        var filter = new OrderFilter { Email = "test1@example.com" };
        var orders = new List<Order>
        {
            Order.Create("test1@example.com", Address.Create("Street1", 1, null), new List<OrderItem>()),
            Order.Create("test5@mple.com", Address.Create("Street2", 2, null), new List<OrderItem>())
        };
        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();

        // Act
        var result = await orderRepository.GetOrdersAsync(filter);

        // Assert
        Assert.Equal(1, result.Count());
        Assert.Equal("test1@example.com", result.First().Email);
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnMatchingOrder()
    {
        // Arrange
        var context = new MockDbContext();
        var orderRepository = new OrderRepository(context);
        
        var order = Order.Create("test@example.com", Address.Create("Street", 1, null), new List<OrderItem>());
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // Act
        var result = await orderRepository.GetOrderById(order.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(order.Id, result.Id);
    }

    [Fact]
    public async Task DeleteOrder_ShouldRemoveOrderFromContext()
    {
        // Arrange
        var context = new MockDbContext();
        var orderRepository = new OrderRepository(context);
        
        var order = Order.Create("test@example.com", Address.Create("Street", 1, null), new List<OrderItem>());
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // Act
        await orderRepository.DeleteOrder(order.Id);

        // Assert
        var result = await context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateOrder_ShouldUpdateOrderInContext()
    {
        // Arrange
        var context = new MockDbContext();
        var orderRepository = new OrderRepository(context);
        
        var order =
            Order.Create("test@example.com", Address.Create("Street1", 1, null), new List<OrderItem>());
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        var updatedOrder = 
            OrderUpdateModel.Create(order.Id, Address.Create("Street2", 2, null), new List<OrderItem>());

        // Act
        await orderRepository.UpdateOrder(updatedOrder);

        // Assert
        var result = await context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        Assert.NotNull(result);
        Assert.Equal(updatedOrder.Address.StreetName, result.Address.StreetName);
        Assert.Equal(updatedOrder.Address.StreetNumber, result.Address.StreetNumber);
        Assert.Equal(updatedOrder.OrderItems, result.OrderItems);
    }
}