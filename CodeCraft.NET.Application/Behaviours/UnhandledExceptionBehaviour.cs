using MediatR;
using Microsoft.Extensions.Logging;

namespace CodeCraft.NET.Application.Behaviours
{
	public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly ILogger<TRequest> Logger;
		private readonly string ERROR_HANDLE_MESSAGE = "Application Request: An exception occurred for the request {@Name} {@Request}";

		public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
		{
			Logger = logger;
		}

		public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			try
			{
				return next();
			}
			catch (Exception ex)
			{
				string requestName = typeof(TRequest).Name;
				Logger.LogError(ex, ERROR_HANDLE_MESSAGE, requestName, request);
				throw;
			}
		}
	}
}
