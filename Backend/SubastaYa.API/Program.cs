using Microsoft.EntityFrameworkCore;
using SubastaYa.API.Middleware;
using SubastaYa.Application.Interfaces;
using SubastaYa.Application.Interfaces.Repositories;
using SubastaYa.Application.Interfaces.Security;
using SubastaYa.Application.Interfaces.Service.Auctions;
using SubastaYa.Application.Interfaces.Service.Categories;
using SubastaYa.Application.Interfaces.Service.Users;
using SubastaYa.Application.Interfaces.Service.Wallets;
using SubastaYa.Application.UseCases.Auctions.CreateAuction;
using SubastaYa.Application.UseCases.Auctions.GetAuction;
using SubastaYa.Application.UseCases.Auctions.GetAuctions;
using SubastaYa.Application.UseCases.Categories.CreateCategory;
using SubastaYa.Application.UseCases.Categories.GetCategories;
using SubastaYa.Application.UseCases.Users.CreateUser;
using SubastaYa.Application.UseCases.Users.GetUser;
using SubastaYa.Application.UseCases.Users.UserAuthentication;
using SubastaYa.Application.UseCases.Wallets.BalanceWallet;
using SubastaYa.Application.UseCases.Wallets.DepositWallet;
using SubastaYa.Infrastructure.Persistence;
using SubastaYa.Infrastructure.Persistence.Repositories;
using SubastaYa.Infrastructure.Security;
using SubastaYa.Application.Interfaces.Service.Audits;
using SubastaYa.Application.Interfaces.Service.LedgerTransactions;
using SubastaYa.Application.UseCases.Audits.GetAudits;
using SubastaYa.Application.UseCases.Audits.RegisterAudits;
using SubastaYa.Application.UseCases.LedgerTransactions.GetTransactions;
using SubastaYa.Application.UseCases.LedgerTransactions.RegisterTransaction;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------- Conexi�n de base de datos ------------

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// ----------- Inyecci�n de dependencias ------------

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

//Auction
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<ICreateAuctionHandler, CreateAuctionHandler>();
builder.Services.AddScoped<IGetAuctionHandler, GetAuctionHandler>();
builder.Services.AddScoped<IGetAuctionsHandler, GetAuctionsHandler>();

//Category
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICreateCategoryHandler, CreateCategoryHandler>();
builder.Services.AddScoped<IGetCategoriesHandler, GetCategoriesHandler>();



// Ledger
builder.Services.AddScoped<ITransactionLedgerRepository, TransactionLedgerRepository>();
builder.Services.AddScoped<IGetTransactionsHandler, GetTransactionsHandler>();
builder.Services.AddScoped<IRegisterTransactionHandler, RegisterTransactionHandler>();

// Audit
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IGetAuditHandler, GetAuditHandler>();
builder.Services.AddScoped<IRegisterAuditHandler, RegisterAuditHandler>();



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
