using KebabMaster.Authorization.Domain.Exceptions;

namespace KebabMaster.Authorization.Domain.Interfaces;

public interface IApplicationLogger
{
    void LogRegistrationStart(object request);
    void LogRegistrationEnd(object request);
    void LogLoginStart(object request);
    void LogLoginEnd(object request);
    void LogGetStart(object request);
    void LogGetEnd(object request);
    void LogDeleteStart(object request);
    void LogDeleteEnd(object request);
    void LogException(Exception exception);
    void LogValidationException(ApplicationValidationException applicationValidationException);

}