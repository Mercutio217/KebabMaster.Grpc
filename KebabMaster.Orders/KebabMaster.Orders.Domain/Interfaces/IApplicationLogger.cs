using KebabMaster.Orders.Domain.Entities.Base;
using KebabMaster.Orders.Domain.Exceptions;

namespace KebabMaster.Orders.Domain.Interfaces;

public interface IApplicationLogger
{
    void LogGetStart(object request);
    void LogGetEnd(object request);
    void LogPostStart(object request);
    void LogPostEnd(object request);
    void LogException(Exception exception);
    void LogValidationException(ApplicationValidationException applicationValidationException);

}