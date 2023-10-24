using System.Text.Json;
using KebabMaster.Authorization.Domain.Entities;
using KebabMaster.Authorization.Domain.Exceptions;
using KebabMaster.Authorization.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace KebabMaster.Authorization.Infrastructure.Logger;

public class ApplicationLogger : IApplicationLogger
{
    private readonly ILogger<User> _logger;

    public ApplicationLogger(ILogger<User> logger)
    {
        _logger = logger;
    }

    public void LogRegistrationStart(object request)
    {
        _logger.LogInformation($"Start registering user with {Serialize(request)}");
    }

    public void LogRegistrationEnd(object request)
    {
        _logger.LogInformation($"Finish registering user with {Serialize(request)}");
    }

    public void LogLoginStart(object request)
    {
        _logger.LogInformation($"Start loggging user with {Serialize(request)}");
    }

    public void LogLoginEnd(object request)
    {
        _logger.LogInformation($"Finish logging user with {Serialize(request)}");
    }
    
    public void LogGetStart(object request)
    {
        _logger.LogInformation($"Start getting users with {Serialize(request)}");
    }

    public void LogGetEnd(object request)
    {
        _logger.LogInformation($"Finish getting users with {Serialize(request)}");
    }

    public void LogDeleteStart(object request)
    {
        _logger.LogInformation($"Start deleting users with {Serialize(request)}");
    }

    public void LogDeleteEnd(object request)
    {
        _logger.LogInformation($"Finish deleting users with {Serialize(request)}");
    }

    public void LogException(Exception exception)
    {
        _logger.LogError($"Error occured during processing entity, Exception: {exception}");
    }
    
    public void LogValidationException(ApplicationValidationException validationException)
    {
        _logger.LogError($"There was error validation exception: {validationException.GetValidationErrorMessage()}");
    }

    private string Serialize(object data)
    {
        string result = JsonSerializer.Serialize(data);
        if (result.Contains("password"))
        {
            // var regex = new Regex("P")
        }

        return result;
    }
}