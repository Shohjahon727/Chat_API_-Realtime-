using FluentValidation;
using System.Net;

namespace Chat_API.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (ValidationException ex)
			{
				_logger.LogWarning("Validation failed: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));

				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				await context.Response.WriteAsJsonAsync(new
				{
					errors = ex.Errors.Select(e => e.ErrorMessage)
				});
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogWarning("Unauthorized access attempt: {Message}", ex.Message);

				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				await context.Response.WriteAsJsonAsync(new
				{
					error = ex.Message
				});
			}
			catch (InvalidOperationException ex)
			{
				_logger.LogWarning("Operation failed: {Message}", ex.Message);

				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				await context.Response.WriteAsJsonAsync(new
				{
					error = ex.Message
				});
			}
			catch (KeyNotFoundException ex)
			{
				_logger.LogWarning("Resource not found: {Message}", ex.Message);

				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				await context.Response.WriteAsJsonAsync(new
				{
					error = ex.Message
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				await context.Response.WriteAsJsonAsync(new
				{
					error = "An unexpected error occurred"
				});
			}
		}
	}
}
