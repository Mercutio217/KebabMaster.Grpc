using System.Net.Mail;
using KebabMaster.Orders.Domain.Exceptions;

namespace KebabMaster.Orders.Domain.Tests.Tools;

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