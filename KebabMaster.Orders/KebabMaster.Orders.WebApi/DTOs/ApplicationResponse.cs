namespace KebabMaster.Orders.DTOs;

public class ApplicationResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Count { get; set; }

    public ApplicationResponse()
    {
    }
    public ApplicationResponse(IEnumerable<T> items)
    {
        Items = items;
        Count = items.Count();
    }
}