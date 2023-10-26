using Grpc.Core;
using Grpc.Net.Client;
using KebabMaster.Deliveries;
using KebabMaster.Grpc.Interfaces;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KebabMaster.Grpc;

public class MessageService : IMessageService
{
    private readonly IApplicationLogger _logger;
    private readonly IHttpContextAccessor _accessor;

    public MessageService(IApplicationLogger logger, IHttpContextAccessor accessor)
    {
        _logger = logger;
        _accessor = accessor;
    }

    public async Task<bool> SendMessage(Order order)
    {
        DeliveryRequest request = new ()
        {
            FlatNumber = order.Address.FlatNumber,
            StreetName = order.Address.StreetName,
            StreetNumber = order.Address.StreetNumber
        };

        try
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7256");
            var client = new Deliverer.DelivererClient(channel);
            var headers = new Metadata();
            var token = _accessor.HttpContext.Request.Headers["Authorization"];
            //headers.Add("Authorization", token);

            DeliveryResponse? reply = await client.CreateDeliveryAsync(request, headers);
            
            return reply.IsSuccess;

        } catch (Exception ex)
        {
            _logger.LogException(ex);

            return false;
        }

    }
}