using KebabMaster.Orders.Domain.Entities;

namespace KebabMaster.Grpc.Interfaces;

public interface IMessageService
{
    Task<bool> SendMessage(Order order);
}