namespace KebabMaster.Orders.Domain.Entities.Base;

public class Result<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Count { get; set; }
    public long TotalCount { get; set; }
}