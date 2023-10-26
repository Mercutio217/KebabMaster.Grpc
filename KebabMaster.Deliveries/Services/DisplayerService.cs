using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using KebabMaster.Communication;

namespace KebabMaster.Deliveries.Services;

public class DisplayerService : Displayer.DisplayerBase
{
    public override async Task Display(Empty request, IServerStreamWriter<DisplayResponse> responseStream, ServerCallContext context)
    {
        Guid previousChange = Guid.NewGuid();
        
        while (!context.CancellationToken.IsCancellationRequested)
        {
            if (DeliveriesContainer.TrackingId != previousChange)
            {
                DisplayResponse response = new ();
                response.Payload.AddRange(DeliveriesContainer.List.Select(di => 
                    new UserResponse() { Email = di.Email, StreetName = di.StreetName, StreetNumber = di.StreetNumber, 
                        Time = DateTime.UtcNow.ToString() }));

                await responseStream.WriteAsync(response);

                await Task.Delay(5000);
            }
        }
    }
}