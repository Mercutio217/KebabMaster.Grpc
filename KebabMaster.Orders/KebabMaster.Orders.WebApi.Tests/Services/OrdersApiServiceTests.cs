using AutoMapper;
using KebabMaster.Orders.Domain;
using KebabMaster.Orders.Domain.DTOs;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Exceptions;
using KebabMaster.Orders.Domain.Interfaces;
using KebabMaster.Orders.DTOs;
using KebabMaster.Orders.Services;
using Moq;
using Xunit;

namespace KebabMaster.Orders.WebApi.Tests.Services;

public class OrdersApiServiceTests
{
    [Fact]
    public async Task CreateOrder_ShouldCallOrderServiceCreateOrder()
    {
        // Arrange
        var orderRequest = new OrderRequest();
        var mockOrderService = new Mock<IOrderService>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<IApplicationLogger>();
        var ordersApiService = new OrderApiService(mockOrderService.Object, mockMapper.Object, mockLogger.Object, new Mock<IMenuRepository>().Object);
        // Act
        await ordersApiService.CreateOrder(orderRequest);

        // Assert
        mockOrderService.Verify(service => service.CreateOrder(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task GetOrdersAsync_ShouldReturnListOfOrderResponses()
    {
        // Arrange
        var orderFilter = new OrderFilter();
        var orders = new List<Order>();
        var responses = new List<OrderResponse>();
        var orderResponses = new ApplicationResponse<OrderResponse>(responses);
        var mockOrderService = new Mock<IOrderService>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<IApplicationLogger>();
        var ordersApiService = new OrderApiService(mockOrderService.Object, mockMapper.Object, mockLogger.Object, new Mock<IMenuRepository>().Object);
        
        mockOrderService.Setup(service => service.GetOrdersAsync(orderFilter)).ReturnsAsync(orders);
        mockMapper.Setup(mapper => mapper.Map<IEnumerable<OrderResponse>>(orders)).Returns(responses);

        // Act
        var result = await ordersApiService.GetOrdersAsync(orderFilter);

        // Assert
        Assert.Equal(orderResponses.Count, result.Count);
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnOrderResponse()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = 
            Order.Create("testemail@mail.com", Address.Create("test", 1, 2), new List<OrderItem>());
        
        var orderResponse = new OrderResponse();
        var mockOrderService = new Mock<IOrderService>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<IApplicationLogger>();
        var ordersApiService = new OrderApiService(mockOrderService.Object, mockMapper.Object, mockLogger.Object, new Mock<IMenuRepository>().Object);
        
        mockOrderService.Setup(service => service.GetOrderByIdAsync(orderId)).ReturnsAsync(order);
        mockMapper.Setup(mapper => mapper.Map<OrderResponse>(order)).Returns(orderResponse);

        // Act
        var result = await ordersApiService.GetOrderById(orderId);

        // Assert
        Assert.Equal(orderResponse.Email, result.Email);
    }

    [Fact]
    public async Task DeleteOrder_ShouldCallOrderServiceDeleteOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var mockOrderService = new Mock<IOrderService>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<IApplicationLogger>();
        var ordersApiService = new OrderApiService(mockOrderService.Object, mockMapper.Object, mockLogger.Object, new Mock<IMenuRepository>().Object);
        
        // Act
        await ordersApiService.DeleteOrder(orderId);

        // Assert
        mockOrderService.Verify(service => service.DeleteOrder(orderId), Times.Once);
    }

    [Fact]
    public async Task UpdateOrder_ShouldCallOrderServiceUpdateOrder()
    {
        // Arrange
        var orderUpdateRequest = new OrderUpdateRequest();
        var orderUpdateModel = new OrderUpdateModel(Guid.NewGuid(), Address.Create("test", 1, 2), new List<OrderItem>());
        var mockOrderService = new Mock<IOrderService>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<IApplicationLogger>();
        var ordersApiService = new OrderApiService(mockOrderService.Object, mockMapper.Object, mockLogger.Object, new Mock<IMenuRepository>().Object);
        
        mockMapper.Setup(mapper => mapper.Map<OrderUpdateModel>(orderUpdateRequest)).Returns(orderUpdateModel);

        // Act
        await ordersApiService.UpdateOrder(orderUpdateRequest);

        // Assert
        mockOrderService.Verify(service => service.UpdateOrder(orderUpdateModel), Times.Once);
    }

    [Fact]
    public async Task GetOrder_ShouldLogValidationExceptionAfterThrowing()
    {
        var orderUpdateRequest = new OrderUpdateRequest();
        var mockOrderService = new Mock<IOrderService>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<IApplicationLogger>();
        var ordersApiService = new OrderApiService(mockOrderService.Object, mockMapper.Object, mockLogger.Object, new Mock<IMenuRepository>().Object);
        
        mockMapper.Setup(mapper => mapper.Map<OrderUpdateModel>(orderUpdateRequest)).Throws(new InvalidEmailFormatException("tests"));

        // Act
        try
        {
            await ordersApiService.UpdateOrder(orderUpdateRequest);
        }
        catch
        {
        }

        // Assert
        mockLogger.Verify(service => service.LogValidationException(It.IsAny<InvalidEmailFormatException>()), Times.Once);
    }
    
    [Fact]
    public async Task GetOrder_ShouldLogExceptionAfterThrowing()
    {
        var orderUpdateRequest = new OrderUpdateRequest();
        var mockOrderService = new Mock<IOrderService>();
        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<IApplicationLogger>();
        var ordersApiService = new OrderApiService(mockOrderService.Object, mockMapper.Object, mockLogger.Object, new Mock<IMenuRepository>().Object);
        
        mockMapper.Setup(mapper => mapper.Map<OrderUpdateModel>(orderUpdateRequest)).Throws(new Exception("tests"));

        // Act
        try
        {
            await ordersApiService.UpdateOrder(orderUpdateRequest);
        }
        catch
        {
        }


        // Assert
        mockLogger.Verify(service => service.LogException(It.IsAny<Exception>()), Times.Once);
    }
}