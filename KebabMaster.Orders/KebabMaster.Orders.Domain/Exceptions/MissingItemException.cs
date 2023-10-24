namespace KebabMaster.Orders.Domain.Exceptions;

public class MissingItemException : ApplicationValidationException
{
    private readonly Guid _missingId;
    
    public MissingItemException(Guid id) : base($"Requested item with id {id} is missing from db")
    {
        _missingId = id;
    }
    
    public override string GetValidationErrorMessage() => $"One of the items ({_missingId}) is missing!";
}