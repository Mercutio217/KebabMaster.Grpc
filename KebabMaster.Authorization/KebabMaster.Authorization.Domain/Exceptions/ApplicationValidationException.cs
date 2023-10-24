namespace KebabMaster.Authorization.Domain.Exceptions;

public abstract class ApplicationValidationException : Exception
{
    protected ApplicationValidationException(string message) : base(message)
    {
    }
    public abstract string GetValidationErrorMessage();
}