using KebabMaster.Orders.Domain.Exceptions;
using Xunit;

namespace KebabMaster.Orders.Domain.Tests.Entity;

public class AddressEntityTests
{
    [Fact]
    public void Create_ValidAddress_ReturnsAddressInstance()
    {
        // Arrange
        string streetName = "Main Street";
        int streetNumber = 123;
        int? flatNumber = 456;

        // Act
        Address address = Address.Create(streetName, streetNumber, flatNumber);

        // Assert
        Assert.NotNull(address);
        Assert.Equal(streetName, address.StreetName);
        Assert.Equal(streetNumber, address.StreetNumber);
        Assert.Equal(flatNumber, address.FlatNumber);
    }

    [Fact]
    public void Create_MissingStreetName_ThrowsMissingMandatoryPropertyException()
    {
        Assert.Throws<MissingMandatoryPropertyException<Address>>(() => Address.Create(string.Empty, 123, 456));
    }

    [Fact]
    public void Create_MissingStreetNumber_ThrowsMissingMandatoryPropertyException()
    {
        Assert.Throws<MissingMandatoryPropertyException<Address>>(() => Address.Create("Main Street", null, 456));
    }
}