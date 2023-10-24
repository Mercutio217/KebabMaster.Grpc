using KebabMaster.Authorization.Domain.Entities;
using KebabMaster.Authorization.Domain.Exceptions;
using KebabMaster.Authorization.Domain.Filter;
using KebabMaster.Authorization.Infrastructure.Database;
using KebabMaster.Authorization.Infrastructure.Repositories;
using KebabMaster.Authorization.Infrastructure.Tests.Mocks;
using Xunit;

namespace KebabMaster.Authorization.Infrastructure.Tests.Repositories;

public class UserRepositoryTests
{     
        [Fact]
        public async Task CreateUser_ValidUser_CreatesUserAndSavesChanges()
        {
            // Arrange
            var user = User.Create("test@example.com", "testuser", "John", "Doe");
            user.PaswordHash = "test";
            var context = CreateContext();
            var repository = new UserRepository(context);

            // Act
            await repository.CreateUser(user);

            // Assert
            Assert.Single(context.Users);
        }

        [Fact]
        public async Task GetUserByFilter_ValidFilter_ReturnsFilteredUsers()
        {
            // Arrange
            var users = new List<User>
            {
                User.Create("test1@example.com", "testuser1", "John", "Doe"),
                User.Create("test2@example.com", "testuser2", "Jane", "Smith"),
                User.Create("test3@example.com", "testuser3", "John", "Smith"),
                User.Create("test4@example.com", "testuser4", "Jane", "Doe")
            };
            users.ForEach(user =>             user.PaswordHash = "test");
            var filter = new UserFilter(){ Name = "John" };
            var context = CreateContext(users);
            var repository = new UserRepository(context);

            // Act
            var filteredUsers = await repository.GetUserByFilter(filter);

            // Assert
            Assert.Equal(2, filteredUsers.Count());
            Assert.All(filteredUsers, user => Assert.Equal("John", user.Name));
        }

        [Fact]
        public async Task GetUserByEmail_ExistingEmail_ReturnsUserWithRoles()
        {
            // Arrange
            var user = User.Create("test@example.com", "testuser", "John", "Doe");
            user.PaswordHash = "test";
            var context = CreateContext(new List<User> { user });
            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetUserByEmail("test@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("testuser", result.UserName);
            Assert.Equal("John", result.Name);
            Assert.Equal("Doe", result.Surname);
        }

        [Fact]
        public async Task GetUserByEmail_NonexistentEmail_ReturnsNull()
        {
            // Arrange
            var context = CreateContext();
            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetUserByEmail("nonexistent@example.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByName_ExistingName_ReturnsUserWithRoles()
        {
            // Arrange
            var user = User.Create("test@example.com", "testuser", "John", "Doe");
            user.PaswordHash = "test";

            var context = CreateContext(new List<User> { user });
            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetUserByName("John");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Email);
            Assert.Equal("testuser", result.UserName);
            Assert.Equal("John", result.Name);
            Assert.Equal("Doe", result.Surname);
            Assert.Empty(result.Roles); // Assuming roles are not loaded in the test data
        }

        [Fact]
        public async Task GetUserByName_NonexistentName_ReturnsNull()
        {
            // Arrange
            var context = CreateContext();
            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetUserByName("NonexistentName");

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task DeleteUser_NonexistentEmail_ThrowsNotFoundException()
        {
            // Arrange
            var context = CreateContext();
            var repository = new UserRepository(context);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => repository.DeleteUser("nonexistent@example.com"));
        }

        [Fact]
        public async Task GetRoleByName_ExistingName_ReturnsRole()
        {
            // Arrange
            var role = new Role("Admin");
            var context = CreateContext(roles: new List<Role> { role });
            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetRoleByName("Admin");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Admin", result.Name);
        }

        [Fact]
        public async Task GetRoleByName_NonexistentName_ReturnsNull()
        {
            // Arrange
            var context = CreateContext();
            var repository = new UserRepository(context);

            // Act
            var result = await repository.GetRoleByName("NonexistentRole");

            // Assert
            Assert.Null(result);
        }

        private static ApplicationDbContext CreateContext(
            List<User>? users = null, List<Role>? roles = null)
        {
            var context = new MockDbContext();

            if (users is not null)
            {
                users.ForEach(user => context.Add(user));
                context.SaveChanges();
            }
            
            if (roles is not null)
            {
                roles.ForEach(role => context.Add(role));

                context.SaveChanges();
            }

            return context;
        }
}