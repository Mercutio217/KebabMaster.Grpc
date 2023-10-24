namespace KebabMaster.Authorization.Domain.Exceptions;

public class NotFoundException : ApplicationValidationException
{
    public NotFoundException(string key) 
        : base($"Resource with id {key} was not found.")
    {
    }

    public override string GetValidationErrorMessage() => "Requested resource was not found!";
}