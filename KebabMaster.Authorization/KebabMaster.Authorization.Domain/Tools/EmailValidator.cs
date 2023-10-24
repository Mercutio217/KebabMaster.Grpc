using System.Net.Mail;
using KebabMaster.Authorization.Domain.Exceptions;

namespace KebabMaster.Authorization.Domain.Tools;

public static class EmailValidator
{
    public static void Validate(string email)
    {
        try
        {
            _ = new MailAddress(email);
        }
        catch
        {
            throw new InvalidEmailFormatException(email);
        }
    }
}