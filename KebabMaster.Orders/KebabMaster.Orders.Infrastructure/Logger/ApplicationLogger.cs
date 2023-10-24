using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Exceptions;
using KebabMaster.Orders.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace KebabMaster.Orders.Infrastructure.Logger;

public class ApplicationLogger : IApplicationLogger
{
    private readonly ILogger<Order> _logger;
    public ApplicationLogger(ILogger<Order> logger)
    {
        _logger = logger;
    }

    public void LogGetStart(object request)
    {
        _logger.LogInformation($"Start getting Orders by filter {JsonSerializer.Serialize(request)}");
    }

    public void LogGetEnd(object request)
    {
        _logger.LogInformation($"Finish getting Orders by filter {JsonSerializer.Serialize(request)}");
    }

    public void LogPostStart(object request)
    {
        _logger.LogInformation($"Start creating Orders with request {JsonSerializer.Serialize(request)}");
    }

    public void LogPostEnd(object request)
    {
        _logger.LogInformation($"Finish creating Orders with request {JsonSerializer.Serialize(request)}");
    }

    public void LogDeleteStart(object request)
    {
        _logger.LogInformation($"Start deleting Orders with request {JsonSerializer.Serialize(request)}");
    }

    public void LogDeleteEnd(object request)
    {
        _logger.LogInformation($"Finish deleting Orders with request {JsonSerializer.Serialize(request)}");
    }

    public void LogPutEnd(object request)
    {
        _logger.LogInformation($"Start updating Orders with request {JsonSerializer.Serialize(request)}");
    }

    public void LogPutStart(object request)
    {
        _logger.LogInformation($"Finish updating Orders with request {JsonSerializer.Serialize(request)}");
    }

    public void LogException(Exception exception)
    {
        _logger.LogError($"Error occured during processing entity, Exception: {exception}");
    }
    
    public void LogValidationException(ApplicationValidationException validationException)
    {
        _logger.LogError($"There was error validation exception: {validationException.GetValidationErrorMessage()}");
    }
}