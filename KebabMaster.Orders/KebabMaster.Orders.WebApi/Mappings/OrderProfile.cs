using AutoMapper;
using KebabMaster.Orders.Domain;
using KebabMaster.Orders.Domain.DTOs;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Filters;
using KebabMaster.Orders.Domain.Interfaces;
using KebabMaster.Orders.DTOs;
using KebabMaster.Orders.Services;

namespace KebabMaster.Orders.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderRequest, Order>()
            .ConvertUsing(request =>
                Order.Create(request.Email,
                    Address.Create(request.Address.StreetName, request.Address.StreetNumber,
                        request.Address.FlatNumber),
                    request.OrderItems.Select(item => OrderItem.Create(item.MenuItemId, item.Quantity)).ToList()));

        CreateMap<Order, OrderResponse>()
            .ConvertUsing(order => new OrderResponse()
            {
                Email = order.Email,
                Id = order.Id,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto()
                {
                    Quantity = item.Quantity,
                    MenuItemId = item.MenuItemId
                }).ToList()
            });
        
         CreateMap<OrderFilterRequest, OrderFilter>();
    }
}
