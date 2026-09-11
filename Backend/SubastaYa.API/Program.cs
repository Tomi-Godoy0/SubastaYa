using Microsoft.EntityFrameworkCore;
using SubastaYa.API.Middleware;
using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Security;
using SubastaYa.Application.Interfaces.Service.Users;
using SubastaYa.Application.Interfaces.Service.Wallets;
using SubastaYa.Application.UseCases.Users.CreateUser;
using SubastaYa.Application.UseCases.Users.GetUser;
using SubastaYa.Application.UseCases.Users.UserAuthentication;
using SubastaYa.Application.UseCases.Wallets.BalanceWallet;
using SubastaYa.Application.UseCases.Wallets.DepositWallet;
using SubastaYa.Infrastructure.Persistence;
using SubastaYa.Infrastructure.Persistence.Repositories;
using SubastaYa.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------- Conexión de base de datos ------------

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// ----------- Inyección de dependencias ------------

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICreateUserHandler, CreateUserHandler>();
builder.Services.AddScoped<IGetUserHandler, GetUserHandler>();
builder.Services.AddScoped<IUserAuthenticationHandler, UserAuthenticationHandler>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

//Wallet
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IGetBalanceHandler, GetBalanceHandler>();
builder.Services.AddScoped<IDepositHandler, DepositHandler>();

var app = builder.Build();

// ----------- Uso de Middleware ------------
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
