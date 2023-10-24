namespace KebabMaster.Authorization.Domain.Exceptions;

public class InvalidLenghtOfPropertyException : ApplicationValidationException
{
    private readonly string _propertyName;
    private readonly string _propertyValue;

    public InvalidLenghtOfPropertyException(string propertyName, string value) : base(GetMessage(propertyName, value))
    {
        _propertyName = propertyName;
        _propertyValue = value;
    }

    public override string GetValidationErrorMessage()
    {
        return GetMessage(_propertyName, _propertyValue);
    }

    private static string GetMessage(string propertyName, string value) =>
        $"Property {propertyName} value of {value} is too long, the max length is 50";
}