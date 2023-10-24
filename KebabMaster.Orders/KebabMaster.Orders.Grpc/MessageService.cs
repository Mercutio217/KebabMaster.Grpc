using Grpc.Net.Client;
using KebabMaster.Deliveries;
using KebabMaster.Grpc.Interfaces;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Interfaces;

namespace KebabMaster.Grpc;

public class MessageService : IMessageService
{
    private readonly IApplicationLogger _logger;

    public MessageService(IApplicationLogger logger)
    {
        _logger = logger;
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
            DeliveryResponse? reply = await client.SendOrderAsync(request);

            return reply.IsSuccess;

        } catch (Exception ex)
        {
            _logger.LogException(ex);

            return false;
        }

    }
}