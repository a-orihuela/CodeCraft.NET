using FluentValidation;
using MediatR;
using ValidationException = CodeCraft.NET.Application.Middleware.Exceptions.ValidationException;

namespace CodeCraft.NET.Application.Behaviours
{
	public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> Validators;

		public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => Validators = validators;

		public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (Validators.Any())
			{
				ValidationContext<TRequest> context = new(request);
				Task<FluentValidation.Results.ValidationResult[]> validationResults = Task.WhenAll(Validators.Select(v => v.ValidateAsync(context, cancellationToken)));
				List<FluentValidation.Results.ValidationFailure> failures = validationResults.Result.SelectMany(r => r.Errors).Where(f => f != null).ToList();
				if (failures.Count != 0)
				{
					throw new ValidationException(failures);
				}
			}
			return next();
		}
	}
}
