using KebabMaster.Authorization.Domain.Entities;
using KebabMaster.Authorization.Domain.Exceptions;
using Xunit;

namespace KebabMaster.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Create_ValidUser_ReturnsUserInstance()
    {
        // Arrange
        string email = "test@example.com";
        string username = "testuser";
        string name = "John";
        string surname = "Doe";

        // Act
        User user = User.Create(email, username, name, surname);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(email, user.Email);
        Assert.Equal(username, user.UserName);
        Assert.Equal(name, user.Name);
        Assert.Equal(surname, user.Surname);
    }

    [Theory]
    [InlineData(null, "testuser", "John", "Doe", typeof(InvalidEmailFormatException))]
    [InlineData("test@example.com", "toolongusername1234567890123456789012345678901234567890", "John", "Doe", typeof(InvalidLenghtOfPropertyException))]
    [InlineData("test@example.com", "testuser", null, "Doe", typeof(MissingMandatoryPropertyException<User>))]
    [InlineData("test@example.com", "testuser", "John", null, typeof(MissingMandatoryPropertyException<User>))]
    [InlineData("test@example.com", "testuser", "John", "toolongsurname1234567890123456789012345678901234567890", typeof(InvalidLenghtOfPropertyException))]
    public void Create_InvalidUser_ThrowsException(string email, string username, string name, string surname, Type exceptionType)
    {
        // Arrange, Act & Assert
        Assert.Throws(exceptionType, () => User.Create(email, username, name, surname));
    }
}