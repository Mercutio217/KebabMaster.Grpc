using KebabMaster.Orders.Domain.Entities.Base;
using KebabMaster.Orders.Domain.Exceptions;

namespace KebabMaster.Orders.Domain.Entities;

public class Address : Entity
{
    public string StreetName { get; private set; }
    public int StreetNumber { get; private set; }
    public int? FlatNumber { get; private set; }

    private Address() { }
    private Address(string streetName, int streetNumber, int? flatNumber)
    {
        StreetName = streetName;
        StreetNumber = streetNumber;
        FlatNumber = flatNumber;
    }

    public static Address Create(string streetName, int? streetNumber, int? flatNumber)
    {
        if (string.IsNullOrWhiteSpace(streetName))
            throw new MissingMandatoryPropertyException<Address>(nameof(StreetName));
        if (streetName.Length > 50)
            throw new InvalidLenghtOfPropertyException(nameof(StreetName), streetName);

        if (!streetNumber.HasValue)
            throw new MissingMandatoryPropertyException<Address>(nameof(StreetNumber));
        if (streetNumber < 1)
            throw new InvalidQuantityOfProperty(nameof(StreetNumber), streetNumber.Value);

        return new Address(streetName, streetNumber.Value, flatNumber);
    }

}