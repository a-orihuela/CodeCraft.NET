using CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Login;
using FluentValidation;

namespace CodeCraft.NET.Application.CQRS.Custom.Features.Users.Validators
{
	public partial class LoginUserValidator : AbstractValidator<LoginUserCommand>
	{
		public LoginUserValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid email format.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(5).WithMessage("Password must be at least 6 characters long.");
		}
	}
}
