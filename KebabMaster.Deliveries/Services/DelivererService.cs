using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace KebabMaster.Deliveries.Services;

[Authorize]
public class DelivererService : Deliverer.DelivererBase
{
    private ILogger<DelivererService> _logger;

    public DelivererService(ILogger<DelivererService> logger)
    {
        _logger = logger;
    }

    public override async Task<DeliveryResponse> CreateDelivery(DeliveryRequest request, ServerCallContext context)
    {
        try
        {
            _logger.LogInformation($"Delivery Added! {request.Email} {request.StreetName} {request.StreetNumber}");
            
            DeliveriesContainer.List.Add(new Delivery()
            {
                Email = request.Email,
                StreetName = request.StreetName,
                StreetNumber = request.StreetNumber
            });
            DeliveriesContainer.ResetId();
            
            return new DeliveryResponse()
            {
                Message = "Completed",
                IsSuccess = true
            };
        }
        catch (Exception exception)
        {
            _logger.LogError($"Something fucky happened {exception.Message}");
            return new DeliveryResponse()
            {
                Message = "Not good",
                IsSuccess = false
            };
        }
    }
}