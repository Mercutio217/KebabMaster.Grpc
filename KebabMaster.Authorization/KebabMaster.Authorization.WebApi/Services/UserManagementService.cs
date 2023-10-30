using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using KebabMaster.Authorization.Domain.Entities;
using KebabMaster.Authorization.Domain.Exceptions;
using KebabMaster.Authorization.Domain.Filter;
using KebabMaster.Authorization.Domain.Interfaces;
using KebabMaster.Authorization.DTOs;
using KebabMaster.Authorization.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace KebabMaster.Authorization.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _repository;
    private readonly IApplicationLogger _logger;
    private readonly IMapper _mapper;

    public UserManagementService(
        IConfiguration configuration,
        IUserRepository repository,
        IApplicationLogger logger,
        IMapper mapper)
    {
        _configuration = configuration;
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateUser(RegisterModel model)
    {
        await Execute(async () =>
        {
            _logger.LogRegistrationStart(model);
            
            User user = User.Create(model.Email, model.UserName, model.Name, model.Surname);

            if (await _repository.GetUserByEmail(user.Email) is not null ||
                (await _repository.GetUserByFilter(new () { UserName = user.UserName })).Any())
                throw new UserAlreadyExistsException(user.Email, user.Surname);
            
            string hash = HashString(model.Password);

            user.PaswordHash = hash;

            var role = await _repository.GetRoleByName("User");
            user.Roles = new List<Role>() { role };

            await _repository.CreateUser(user);
            
            _logger.LogRegistrationEnd(model);

        });
    }


    public async Task<TokenResponse> Login(LoginModel model)
    {
        return await Execute(async () =>
        {
            _logger.LogLoginStart(model.Email);
            
            var user = await _repository.GetUserByEmail(model.Email);
            if (user is null)
                throw new UnauthorizedException(model.Email);

            string hashString = HashString(model.Password);

            if (hashString != user.PaswordHash)
                throw new UnauthorizedException(model.Email);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = GetToken(claims);
            
            _logger.LogLoginEnd(model.Email);

            return new TokenResponse()
            {
                ExpiresAt = token.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };
        });
    }

    public async Task<IEnumerable<UserResponse>> GetByFilter(UserRequest request) =>
        await Execute(async () =>
        {
            _logger.LogGetStart(request);
            
            var result =  _mapper.Map<IEnumerable<UserResponse>>(
                await _repository.GetUserByFilter(_mapper.Map<UserFilter>(request)));
            
            _logger.LogGetEnd(request);

            return result;
        });

    public async Task DeleteUser(string email)
    {
        await Execute(async () =>
        {
            _logger.LogDeleteStart(email);
            
             await _repository.DeleteUser(email);
             
             _logger.LogDeleteEnd(email);
        });
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        SymmetricSecurityKey authSigningKey = 
            new (Encoding.UTF8.GetBytes(_configuration["TokenData:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["TokenData:ValidIssuer"],
            audience: _configuration["TokenData:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
    private  string HashString(string text)
    {
        if (String.IsNullOrEmpty(text))
            throw new MissingMandatoryPropertyException<User>("Password");

        using var sha = new System.Security.Cryptography.SHA256Managed();
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        byte[] hashBytes = sha.ComputeHash(textBytes);
        
        string hash = BitConverter
            .ToString(hashBytes)
            .Replace("-", String.Empty);

        return hash;
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
        catch (UnauthorizedException unauthorizedException)
        {
            _logger.LogException(unauthorizedException);
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
        catch (UnauthorizedException unauthorizedException)
        {
            _logger.LogException(unauthorizedException);
            throw;

        }
        catch (Exception exception)
        {
            _logger.LogException(exception);
            throw;
        }
    }
}