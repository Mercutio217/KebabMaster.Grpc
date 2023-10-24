using KebabMaster.Authorization.Domain.Entities;
using KebabMaster.Authorization.Domain.Exceptions;
using KebabMaster.Authorization.Domain.Filter;
using KebabMaster.Authorization.Domain.Interfaces;
using KebabMaster.Authorization.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace KebabMaster.Authorization.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task CreateUser(User user)
    {
        _context.Users.AddAsync(user);
        return _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetUserByFilter(UserFilter filter)
    {
        return await ParseFilter(_context.Users, filter).ToListAsync();
    }

    private IQueryable<User> ParseFilter(IQueryable<User> collection, UserFilter filter)
    {
        if (filter.Email is not null)
            collection = collection.Where(u => u.Email == filter.Email);
        if (filter.Name is not null)
            collection = collection.Where(u => u.Name == filter.Name);
        if (filter.Surname is not null)
            collection = collection.Where(u => u.Surname == filter.Surname);
        if (filter.UserName is not null)
            collection = collection.Where(u => u.UserName == filter.UserName);

        return collection;
    }
    public Task<User?> GetUserByEmail(string email)
    {
        return _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public Task<User?> GetUserByName(string name)
    {
        return _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(user => user.Name == name);
    }
    public async Task DeleteUser(string email)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

        if (user is null)
            throw new NotFoundException(email);

        _context.Users.Remove(user);

        _context.SaveChanges();
    }

    public Task<Role?> GetRoleByName(string name)
    {
        return _context.Roles.FirstOrDefaultAsync(role => role.Name == name);
    }
}