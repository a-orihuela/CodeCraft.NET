using CodeCraft.NET.Application.DTOs.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CodeCraft.NET.Application.Middleware.Exceptions;

namespace CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.ConfirmEmail
{
	public partial class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, bool>
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public ConfirmEmailHandler(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.UserId);
			if (user == null)
				throw new NotFoundException("User not found.", 0);

			var result = await _userManager.ConfirmEmailAsync(user, request.Token);

			if (!result.Succeeded)
				throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));

			return true;
		}
	}
}
