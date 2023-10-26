using Grpc.Net.Client;

namespace KebabMaster.Watcher;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = GrpcChannel.ForAddress("https://localhost:7256");
        int previousCount = default;

        var client = new Communication.Displayer.DisplayerClient(channel);

        using var request = client.Display(new Google.Protobuf.WellKnownTypes.Empty());

        while (await request.ResponseStream.MoveNext(CancellationToken.None))
        {
            Communication.DisplayResponse? current = request.ResponseStream.Current;
            if(current?.Payload.Count > 0 && current.Payload.Count > previousCount)
            {
                _logger.LogInformation($"AT {DateTime.UtcNow}, current deliveries are:");
                foreach (var item in current.Payload)
                    _logger.LogInformation($"{item.Email} {item.StreetName} {item.StreetNumber} {item.Time}");

                previousCount = current.Payload.Count;  
            }

            await Task.Delay(5000);
        }

    }
}
