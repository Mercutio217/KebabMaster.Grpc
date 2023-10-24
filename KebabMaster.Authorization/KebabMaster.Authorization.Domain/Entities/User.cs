using KebabMaster.Authorization.Domain.Exceptions;
using KebabMaster.Authorization.Domain.Tools;

namespace KebabMaster.Authorization.Domain.Entities;

public class User
{
    public string Email { get; private set; }
    public string UserName { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string PaswordHash { get; set; }
    public IEnumerable<Role> Roles { get; set; }
    
    public User() { }

    private User(string email, string userName, string name, string surname)
    {
        Email = email;
        UserName = userName;
        Name = name;
        Surname = surname;
    }

    public static User Create(string email, string username, string name, string surname)
    {
        EmailValidator.Validate(email);
        if (username is null)
            throw new MissingMandatoryPropertyException<User>(nameof(Name));

        if (username.Length > 50)
            throw new InvalidLenghtOfPropertyException(nameof(UserName), username);
        
        if (name is null)
            throw new MissingMandatoryPropertyException<User>(nameof(Name));
        
        if (name.Length > 50)
            throw new InvalidLenghtOfPropertyException(nameof(Name), username);
        
        if (surname is null)
            throw new MissingMandatoryPropertyException<User>(nameof(Name));
        
        if (surname.Length > 50)
            throw new InvalidLenghtOfPropertyException(nameof(Surname), username);
        
        return new User(email, username, name, surname);
    }
}
