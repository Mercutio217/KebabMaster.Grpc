using KebabMaster.Authorization.DTOs;

namespace KebabMaster.Authorization.Interfaces;

public interface IUserManagementService
{
    public Task CreateUser(RegisterModel model);
    public Task<TokenResponse> Login(LoginModel model);
    public Task<IEnumerable<UserResponse>> GetByFilter(UserRequest request);
    public Task DeleteUser(string email);

}