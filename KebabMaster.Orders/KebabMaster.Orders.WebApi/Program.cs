using System.Text;
using KebabMaster.Grpc;
using KebabMaster.Grpc.Interfaces;
using KebabMaster.Orders.Domain;
using KebabMaster.Orders.Domain.Entities;
using KebabMaster.Orders.Domain.Interfaces;
using KebabMaster.Orders.Domain.Services;
using KebabMaster.Orders.Infrastructure.Database;
using KebabMaster.Orders.Infrastructure.Logger;
using KebabMaster.Orders.Infrastructure.Repositories;
using KebabMaster.Orders.Infrastructure.Settings;
using KebabMaster.Orders.Interfaces;
using KebabMaster.Orders.Mappings;
using KebabMaster.Orders.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IOrderApiService, OrderApiService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IMenuRepository, MenuRepository>();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<IApplicationLogger, ApplicationLogger>();

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddAutoMapper(typeof(OrderProfile));
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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
builder.Services.AddHttpContextAccessor();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    SetupDatabase();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SetupDatabase()
{
    var database = new ApplicationDbContext();
    database.Database.EnsureCreated();

    if (!database.MenuItems.Any())
    {

        database.MenuItems.Add(new MenuItem(new Guid("089FC182-B76C-4521-9CA8-9FCF5F2B3701"), "Chicken Kebab", 9.99));
        database.MenuItems.Add(new MenuItem(new Guid("EC4E787D-68F4-4DFC-A66B-E30635F5C14C"),"Beef Kebab", 10.99));
        database.MenuItems.Add(new MenuItem(new Guid("995F97AE-89A5-48F2-8F2F-1DC8DC64FAC2"),"Lamb Kebab", 12.99));
        database.MenuItems.Add(new MenuItem(new Guid("F50B470A-8F2D-47A0-8EDB-976F6D52A00D"),"Vegetable Kebab", 8.99));
        database.MenuItems.Add(new MenuItem(new Guid("DBE65F4B-BE65-4DDD-AC99-B29D6E9EE5F1"), "Mixed Kebab", 11.99));
    }

    database.SaveChanges();
}