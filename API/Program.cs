using Api.Data;
using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddApplicationServices(config);
builder.Services.AddIdentityServices(config);

var app = builder.Build();

// Middlewares

// Exception middleware is on top of the pipeline to catch exceptions from all other middlewares.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader()
	.AllowAnyMethod()
	.WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapControllers();

// We need to create a scope to access the services in the DI container
// and perform the database migration and seeding.
// We use the service locator to get the required services. Must be done after middleware registration.
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try {
	var context = services.GetRequiredService<DataContext>();
	await context.Database.MigrateAsync();
	await Seed.SeedUserData(context);
}
catch (Exception ex) {
	var logger = services.GetRequiredService<ILogger<Program>>();
	logger.LogError(ex, "An error occurred during migration");
}

app.Run();
