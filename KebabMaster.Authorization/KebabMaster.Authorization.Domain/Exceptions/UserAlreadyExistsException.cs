namespace KebabMaster.Authorization.Domain.Exceptions;

public class UserAlreadyExistsException : ApplicationValidationException
{
    public UserAlreadyExistsException(string email, string usernName) 
        : base($"There was attempt to create new accoutn for name{ usernName}, email {email}, but it already exists.")
    {
    }

    public override string GetValidationErrorMessage() => "User with these credentials already exisis!";
}