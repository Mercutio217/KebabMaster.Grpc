namespace KebabMaster.Orders.Domain.Exceptions;

public class InvalidEmailFormatException : ApplicationValidationException
{
    private readonly string _invalidEmailValue;
    
    public InvalidEmailFormatException(string invalidEmail) 
        : base($"User supplied invalid email value of {invalidEmail}")
    {
        _invalidEmailValue = invalidEmail;
    }

    public override string GetValidationErrorMessage()
    {
        return $"The email value of {_invalidEmailValue} is invalid, please supply email in correct format!";
    }
}