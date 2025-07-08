using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CodeCraft.NET.Application.Middleware.Errors;
using CodeCraft.NET.Application.Middleware.Exceptions;
using System.Net;

namespace CodeCraft.NET.Application.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);

				string? details = null;
				int statusCode;
				switch (ex)
				{
					case NotFoundException notFoundException:
						statusCode = (int)HttpStatusCode.NotFound;
						break;
					case ValidationException validationException:
						statusCode = (int)HttpStatusCode.BadRequest;
						details = JsonConvert.SerializeObject(validationException.Errors);
						break;
					case BadRequestException badRequestException:
						statusCode = (int)HttpStatusCode.BadRequest;
						break;
					case Exceptions.UnauthorizedAccessException unauthorizedAccessException:
						statusCode = (int)HttpStatusCode.Unauthorized;
						break;
					default:
						statusCode = (int)HttpStatusCode.InternalServerError;
						details = ex.StackTrace;
						break;
				}

				string result = JsonConvert.SerializeObject(
					new CodeErrorException(statusCode, ex.Message, details));

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = statusCode;
				await context.Response.WriteAsync(result);
			}
		}
	}
}
