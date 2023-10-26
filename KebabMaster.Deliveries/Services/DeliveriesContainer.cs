namespace KebabMaster.Deliveries.Services;

public static class DeliveriesContainer
{
    public static List<Delivery> List { get; set; } = new();
    public static Guid TrackingId = Guid.Empty;
    public static void ResetId() => TrackingId = Guid.NewGuid();
}