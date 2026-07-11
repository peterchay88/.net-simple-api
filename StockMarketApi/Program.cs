using dotenv.net;
using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.Services.Clients;
using StockMarketApi.Services.Mappers;

// Load .env file
DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add controller services to the container
builder.Services.AddControllers(); 

// Add Database connection information
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:DefaultConnection");

// Add services via dependency injection
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<TickerMapper>();
builder.Services.AddScoped<StockApiClient>();

builder.Services.AddHttpClient<StockApiClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers(); 
app.Run();
