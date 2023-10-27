using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Filters;
using KebabMaster.Orders.DTOs;
using KebabMaster.Orders.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KebabMaster.Orders.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ApplicationBaseController
{
    private IOrderApiService _orderApiService;
    
    public OrdersController(
        IOrderApiService orderApiService)
    {
        _orderApiService = orderApiService;
    }
    
    /// <summary>
    /// Getting already created orders by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    //[Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<ApplicationResponse<OrderResponse>>> Get([FromQuery] OrderFilter filter)
    {
        return await Execute(() => _orderApiService.GetOrdersAsync(filter));
    }
    /// <summary>
    /// Getting already created orders by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    //[Authorize(Roles = "Admin")]
    [HttpGet("{id}")] 
    public async Task<ActionResult<OrderResponse>> Get(Guid id) => 
        await Execute(() => _orderApiService.GetOrderById(id));
    /// <summary>
    /// Creating new order
    /// </summary>
    /// <param name="orderRequest"></param>
    /// <returns></returns>
    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> Post(OrderRequest orderRequest) => 
        await Execute(() => _orderApiService.CreateOrder(orderRequest), NoContent());

    [HttpGet("/menu")]
    public async Task<IEnumerable<MenuItem>> GetItems() => await _orderApiService.GetMenuItems();
}