var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------- Conexion de base de datos ------------

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// ----------- Inyeccion de dependencias ------------

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

//Pujas
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<ICreateBidHandler, CreateBidHandler>();

// Ledger
builder.Services.AddScoped<ITransactionLedgerRepository, TransactionLedgerRepository>();
builder.Services.AddScoped<IGetTransactionHandler, GetTransactionHandler>();
builder.Services.AddScoped<IGetTransactionsHandler, GetTransactionsHandler>();

// Audit
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IGetAuditHandler, GetAuditHandler>();
builder.Services.AddScoped<IGetAuditsHandler, GetAuditsHandler>();

//Worker
builder.Services.AddHostedService<AuctionWorker>();
builder.Services.AddScoped<IAuctionClosingService, AuctionClosingService>();
builder.Services.AddScoped<IAuctionStartingService, AuctionStartingService>();

//SignalR
builder.Services.AddScoped<IAuctionNotifier, AuctionNotifierService>();
builder.Services.AddSignalR();

//Configuracion de cors
builder.Services.AddCors(opciones =>
    opciones.AddPolicy("frontend", politica => politica
        .WithOrigins(
            "http://127.0.0.1:5500",
            "http://127.0.0.1:5501"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
    ));

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

app.UseCors("frontend");
app.UseAuthorization();

// ----------- Uso de SignalR ------------
app.MapHub<AuctionHub>("/hubs/auction");
app.MapControllers();

// ----------- Seed de la base de datos ------------
using (var scope = app.Services.CreateScope())
{
    await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
}

app.Run();
