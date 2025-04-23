using System.Net;
using System.Text.Json;
using DatingApp.API.Errors;

namespace API.Middleware;

	public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
	{
   // Next looks for an InvokeAsync method in the pipeline to call next middleware
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				// Calling the next middleware in the pipeline if no exception occurs.
				await next(context);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, ex.Message);

				// Creating the response object.
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				var response = env.IsDevelopment()
					? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
					: new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

				// Serializing the response object to JSON.
				var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
				var json = JsonSerializer.Serialize(response, options);

				// Writing the JSON response to the HTTP context.
				await context.Response.WriteAsync(json);
			}
		}
	}

	public static class ExceptionMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
		{
			return app.UseMiddleware<ExceptionMiddleware>();
		}
	}