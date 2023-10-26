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
    DatabaseOptions settings = builder.Configuration.GetSection("Database").Get<DatabaseOptions>();
    var database = new ApplicationDbContext(Options.Create(settings));
    database.Database.EnsureCreated();

    if (!database.MenuItems.Any())
    {

        database.MenuItems.Add(new MenuItem("Chicken Kebab", 9.99));
        database.MenuItems.Add(new MenuItem("Beef Kebab", 10.99));
        database.MenuItems.Add(new MenuItem("Lamb Kebab", 12.99));
        database.MenuItems.Add(new MenuItem("Vegetable Kebab", 8.99));
        database.MenuItems.Add(new MenuItem("Mixed Kebab", 11.99));
    }

    database.SaveChanges();
}