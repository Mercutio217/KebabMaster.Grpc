using AutoMapper;
using KebabMaster.Grpc.Interfaces;
using KebabMaster.Orders.Domain;
using KebabMaster.Orders.Domain.DTOs;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Exceptions;
using KebabMaster.Orders.Domain.Filters;
using KebabMaster.Orders.Domain.Interfaces;
using KebabMaster.Orders.DTOs;
using KebabMaster.Orders.Interfaces;

namespace KebabMaster.Orders.Services;

public class OrderApiService : IOrderApiService
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly IApplicationLogger _logger;
    private readonly IMenuRepository _menuRepository;
    private readonly IMessageService _messageService;

    public OrderApiService(
        IOrderService orderService,
        IMapper mapper,
        IApplicationLogger logger, 
        IMenuRepository menuRepository,
        IMessageService messageService)
    {
        _orderService = orderService;
        _mapper = mapper;
        _logger = logger;
        _menuRepository = menuRepository;
        _messageService = messageService;
    }

    public async Task CreateOrder(OrderRequest request)
    {
        await Execute(async () =>
        {
            _logger.LogPostStart(request);

            Order order = _mapper.Map<Order>(request);

            await _orderService.CreateOrder(order);
            await _messageService.SendMessage(order);
            _logger.LogPostEnd(order);
        });
    }

    public async Task<ApplicationResponse<OrderResponse>> GetOrdersAsync(OrderFilter filter) =>
        await Execute(async () =>
        {
            _logger.LogGetStart(filter);
            IEnumerable<Order> result = await _orderService.GetOrdersAsync(filter);
            _logger.LogGetEnd(filter);
            return new ApplicationResponse<OrderResponse>(
                _mapper.Map<IEnumerable<OrderResponse>>(result));
        });

    public async Task<OrderResponse> GetOrderById(Guid id) =>
        await Execute(async () =>
        {
            _logger.LogGetStart(id);
            Order result = await Execute<Order>(() => _orderService.GetOrderByIdAsync(id));
            _logger.LogGetEnd(id);

            return _mapper.Map<OrderResponse>(result);
        });

    public async Task<IEnumerable<MenuItem>> GetMenuItems()
    {
        return await Execute(() => _menuRepository.GetMenuItems());
    }

    private async Task Execute(Func<Task> function)
    {
        try
        {
            await function();
        }
        catch (ApplicationValidationException validationException)
        {
            _logger.LogValidationException(validationException);
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogException(exception);
            throw;
        }
    }

    private async Task<T> Execute<T>(Func<Task<T>> function)
    {
        try
        {
            return await function();
        }
        catch (ApplicationValidationException validationException)
        {
            _logger.LogValidationException(validationException);
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogException(exception);
            throw;
        }
    }
}
