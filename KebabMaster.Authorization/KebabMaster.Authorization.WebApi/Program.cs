using System.Text;
using KebabMaster.Authorization.Domain.Entities;
using KebabMaster.Authorization.Domain.Interfaces;
using KebabMaster.Authorization.Infrastructure.Database;
using KebabMaster.Authorization.Infrastructure.Logger;
using KebabMaster.Authorization.Infrastructure.Repositories;
using KebabMaster.Authorization.Infrastructure.Settings;
using KebabMaster.Authorization.Interfaces;
using KebabMaster.Authorization.Mappings;
using KebabMaster.Authorization.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IApplicationLogger, ApplicationLogger>();
builder.Services.AddTransient<IUserManagementService, UserManagementService>();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection("Database"));
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddAutoMapper(conf => conf.AddProfile<UserProfile>());
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = builder.Configuration["TokenData:Issuer"],
        ValidIssuer = builder.Configuration["TokenData:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenData:Secret"]))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    SetDatabase();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SetDatabase()
{
    DatabaseOptions settings = builder.Configuration.GetSection("Database").Get<DatabaseOptions>();
    var database = new ApplicationDbContext(Options.Create(settings));
    database.Database.EnsureCreated();
    
    if (!database.Roles.Any())
    {
        database.Roles.Add(new Role("Admin"));
        database.Roles.Add(new Role("User"));
    
    }
    
    database.SaveChanges();
    
    if (!database.Users.Any())
    {
        var adminUser = 
            User.Create("testmail@mail.com","Gul" ,"Skrain", "Dukat");
    
        adminUser.PaswordHash = "799DBF90EE52688EB50516DE263415C05207AD00866860331795784F1EC950CF";
        adminUser.Roles = new List<Role>() { database.Roles.First(r => r.Name == "Admin") };
        database.Users.Add(adminUser);
    }

    database.SaveChanges();
}