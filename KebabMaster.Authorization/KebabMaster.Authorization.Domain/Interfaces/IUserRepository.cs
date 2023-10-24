using KebabMaster.Authorization.Domain.Entities;
using KebabMaster.Authorization.Domain.Filter;

namespace KebabMaster.Authorization.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task<IEnumerable<User>> GetUserByFilter(UserFilter filter);
    Task<User> GetUserByEmail(string email);
    Task DeleteUser(string email);
    Task<Role?> GetRoleByName(string name);
}