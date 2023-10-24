using KebabMaster.Authorization.Domain.Filter;
using KebabMaster.Authorization.DTOs;
using KebabMaster.Authorization.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KebabMaster.Authorization.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorizationController : ApplicationBaseController
{
    private readonly IUserManagementService _service;

    public AuthorizationController(IUserManagementService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenResponse>> Login(
        [FromBody] LoginModel model
        )
    {
        return await Execute(() =>  _service.Login(model));
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterModel model
    )
    {
        return await Execute(() => _service.CreateUser(model), Ok());
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("users")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> Get(
        [FromQuery] UserRequest model
    )
    {
        return await Execute(() => _service.GetByFilter(model));
    }
    
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("users/{email}")]
    public async Task<IActionResult> Delete(
        [FromRoute] string email
    )
    {
        return await Execute(() => _service.DeleteUser(email), Ok());
    }

}