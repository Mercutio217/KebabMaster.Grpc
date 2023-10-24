namespace KebabMaster.Authorization.Domain.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string userName) 
        : base(CreateMessage(userName))
    {
    }

    private static string CreateMessage(string email) => $"Invalid user credentials for email {email}.";
}