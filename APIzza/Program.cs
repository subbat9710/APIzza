using APIzza.BusinessLogic;
using APIzza.DAO;
using APIzza.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Set up configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get the connection string
string connectionString = builder.Configuration.GetConnectionString("Project");

// configure jwt authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSecret"]);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.Sub] = "sub";
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        NameClaimType = "name"
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Dependency Injection configuration
builder.Services.AddSingleton<ITokenGenerator>(tk => new JwtGenerator(builder.Configuration["JwtSecret"]));
builder.Services.AddSingleton<IPasswordHasher>(ph => new PasswordHasher());
builder.Services.AddTransient<IUserDao>(m => new UserSqlDao(connectionString));
builder.Services.AddTransient<IAddressDAO>(m => new AddressSqlDAO(connectionString));
builder.Services.AddTransient<IPendingOrders>(m => new PendingOrdersSqlDao(connectionString));
builder.Services.AddTransient<ICreateSpecialtyPizaa>(m => new CreateSpecialtyPizzaSqlDao(connectionString));
builder.Services.AddTransient<ISpecialtyDAO>(m => new SpecialtySqlDAO(connectionString));
builder.Services.AddTransient<ICartDao>(m => new CartSqlDao(connectionString));
builder.Services.AddTransient<ISideDao>(m => new SideSqlDao(connectionString));
builder.Services.AddTransient<IOrderDAO>(m => new OrderSqlDAO(connectionString));
builder.Services.AddTransient<IBeverageDao>(m => new BeverageSqlDao(connectionString));
builder.Services.AddTransient<IReviewDao>(m => new ReviewSqlDao(connectionString));
builder.Services.AddTransient<IAddToCart>(m => new AddToCartSqlDao(connectionString));
builder.Services.AddScoped<ICartService, CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
