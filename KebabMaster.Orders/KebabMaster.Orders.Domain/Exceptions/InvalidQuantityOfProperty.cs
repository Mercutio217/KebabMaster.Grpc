namespace KebabMaster.Orders.Domain.Exceptions;

public class InvalidQuantityOfProperty : ApplicationValidationException
{
    private readonly string _propertyName;
    private readonly double _propertyValue;

    public InvalidQuantityOfProperty(string propertyName, double value) : base(GetMessage(propertyName, value))
    {
        _propertyName = propertyName;
        _propertyValue = value;
    }

    public override string GetValidationErrorMessage()
    {
        return GetMessage(_propertyName, _propertyValue);
    }

    private static string GetMessage(string propertyName, double value) =>
        $"Property {propertyName} value of {value} is invalid, the max quantity is 50 and min is 1";
}