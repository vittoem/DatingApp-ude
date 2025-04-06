using API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddApplicationServices(config);
builder.Services.AddIdentityServices(config);

var app = builder.Build();

// Middlewares

app.UseCors(x => x.AllowAnyHeader()
	.AllowAnyMethod()
	.WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
