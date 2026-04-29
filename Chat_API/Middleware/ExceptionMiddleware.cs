using FluentValidation;
using System.Net;

namespace Chat_API.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (ValidationException ex)
			{
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				await context.Response.WriteAsJsonAsync(new
				{
					errors = ex.Errors.Select(e => e.ErrorMessage)
				});
			}
			catch (Exception)
			{
				context.Response.StatusCode = 500;
				await context.Response.WriteAsync("Internal Server Error");
			}
		}
	}
}
