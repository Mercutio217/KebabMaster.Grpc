using Grpc.Core;

namespace KebabMaster.Deliveries.Services;

public class DelivererService : Deliverer.DelivererBase
{
    private ILogger<DelivererService> _logger;

    public DelivererService(ILogger<DelivererService> logger)
    {
        _logger = logger;
    }

    public override Task<DeliveryResponse> SendOrder(DeliveryRequest request, ServerCallContext context)
    {
        _logger.LogInformation(request.StreetName);
        Console.WriteLine(request.StreetName);
        
        return base.SendOrder(request, context);
    }
}