using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace TaskManager.Middleware
{
	public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionMiddleware> _logger;

		public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception occurred. Request path: {Path}", httpContext.Request.Path);
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			var response = new ProblemDetails();

			switch (exception)
			{
				case UnauthorizedAccessException:
				case System.Security.Authentication.AuthenticationException:
				case Microsoft.IdentityModel.Tokens.SecurityTokenException:
					context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					response.Status = 401;
					response.Title = "Unauthorized";
					response.Detail = "You are not authorized to access this resource.";
					break;
				case ArgumentException:
				case InvalidOperationException:
					response.Status = (int)HttpStatusCode.BadRequest;
					response.Title = "Bad Request";
					response.Detail = exception.Message;
					break;
				case KeyNotFoundException:
					response.Status = (int)HttpStatusCode.NotFound;
					response.Title = "Not Found";
					response.Detail = "The requested resource was not found.";
					break;
				default:
					response.Status = (int)HttpStatusCode.InternalServerError;
					response.Title = "Server Error";
					response.Detail = "An unexpected error occurred. Please try again later.";
					break;
			}

			context.Response.StatusCode = response.Status.Value;

			var result = JsonSerializer.Serialize(response);
			return context.Response.WriteAsync(result);
		}
	}
}