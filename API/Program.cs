using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register the DbContext with the DI container
// Use SQLite as the database provider
builder.Services.AddDbContext<DataContext>(options => {
		options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
	});

builder.Services.AddCors();

var app = builder.Build();

// Middlewares

app.UseCors(x => x.AllowAnyHeader()
	.AllowAnyMethod()
	.WithOrigins("http://localhost:4200", "https://localhost:4200"));

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
