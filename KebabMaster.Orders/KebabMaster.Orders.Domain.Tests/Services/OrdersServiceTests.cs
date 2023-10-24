using KebabMaster.Orders.Domain.DTOs;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Exceptions;
using KebabMaster.Orders.Domain.Interfaces;
using KebabMaster.Orders.Domain.Services;
using KebabMaster.Orders.Domain.Tests.Tools;
using Moq;
using Xunit;

namespace KebabMaster.Orders.Domain.Tests.Services;

public class OrdersServiceTests
{
        [Fact]
        public async Task CreateOrder_ShouldCallRepositoryCreateOrder()
        {
            // Arrange
            var repositoryMock = new Mock<IOrderRepository>();
            var ordersService = new OrderService(repositoryMock.Object, GetMenuRepository());
            
            Order order = TestData.Order;
    
            // Act
            await ordersService.CreateOrder(order);
    
            // Assert
            repositoryMock.Verify(r => r.CreateOrder(order), Times.Once);
        }


        [Fact]
        public async Task GetOrdersAsync_ShouldCallRepositoryGetOrdersAsync()
        {
            // Arrange
            var repositoryMock = new Mock<IOrderRepository>();
            var ordersService = new OrderService(repositoryMock.Object, GetMenuRepository());

            var filter = new OrderFilter();
    
            // Act
            await ordersService.GetOrdersAsync(filter);
    
            // Assert
            repositoryMock.Verify(r => r.GetOrdersAsync(filter), Times.Once);
        }
    
        [Fact]
        public async Task GetOrderByIdAsync_ShouldCallRepositoryGetOrderById()
        {
            var repositoryMock = new Mock<IOrderRepository>();
            var ordersService = new OrderService(repositoryMock.Object, GetMenuRepository());

            // Arrange
            var id = Guid.NewGuid();
    
            // Act
            await ordersService.GetOrderByIdAsync(id);
    
            // Assert
            repositoryMock.Verify(r => r.GetOrderById(id), Times.Once);
        }
    
        [Fact]
        public async Task DeleteOrder_ShouldCallRepositoryDeleteOrder()
        {
            // Arrange
            var repositoryMock = new Mock<IOrderRepository>();
            var ordersService = new OrderService(repositoryMock.Object, GetMenuRepository());

            var id = Guid.NewGuid();
    
            // Act
            await ordersService.DeleteOrder(id);
    
            // Assert
            repositoryMock.Verify(r => r.DeleteOrder(id), Times.Once);
        }
    
        [Fact]
        public async Task UpdateOrder_ShouldCallRepositoryUpdateOrder()
        {
            // Arrange
            var repositoryMock = new Mock<IOrderRepository>();
            var ordersService = new OrderService(repositoryMock.Object, GetMenuRepository());

            var order = OrderUpdateModel.Create(Guid.NewGuid(), TestData.Address, TestData.OrderItems);
    
            // Act
            await ordersService.UpdateOrder(order);
    
            // Assert
            repositoryMock.Verify(r => r.UpdateOrder(order), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_WithMissingMenuItemShouldThrowExceptition()
        {
            // Arrange
            var repositoryMock = new Mock<IOrderRepository>();
            var ordersService = new OrderService(repositoryMock.Object, new Mock<IMenuRepository>().Object);

            var order = Order.Create("test@mail.com", TestData.Address, TestData.OrderItems);

            // Act & Assert
            await Assert.ThrowsAsync<MissingItemException>(async () => await ordersService.CreateOrder(order));
        }

        private IMenuRepository GetMenuRepository()
        {
            var mock = new Mock<IMenuRepository>();
            mock.Setup(m => m.GetMenuItemById(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new MenuItem("Kebab", 20)));

            return mock.Object;
        }

}