using Microsoft.Extensions.Logging;
using WaterShop.Exceptions;

namespace WaterShop.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
	private readonly ILogger<ExceptionMiddleware> _logger;

	public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
		_logger = logger;
	}
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			_logger.LogInformation($"{DateTime.UtcNow}: Method: {context.Request.Method}, Path: {context.Request.Path}");
			await next(context);
		}
		catch (ValidationDataException ex )
		{
			_logger.LogError(ConfigErrorMessage(context.Request.Method, context.Request.Path, ex.Message));
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			await context.Response.WriteAsync(ex.Message);
		}
		catch (ArgumentException ex)
		{
			_logger.LogError(ConfigErrorMessage(context.Request.Method, context.Request.Path, ex.Message));
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			await context.Response.WriteAsync(ex.Message);
		}
		catch (NotFoundException ex)
		{
			_logger.LogError(ConfigErrorMessage(context.Request.Method, context.Request.Path, ex.Message));
			context.Response.StatusCode = StatusCodes.Status404NotFound;
			await context.Response.WriteAsync(ex.Message);
		}
	}

	private string ConfigErrorMessage(string method, string path, string message)
	{
		return $"Error while handling request{path}. METHOD: {method} Message: {message}";
	}
}
