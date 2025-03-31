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

var app = builder.Build();

// Middlewares
// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
