using CodeCraft.NET.Application.Contracts.Persistence.Repositories;
using CodeCraft.NET.Application.DTOs.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Login
{
	public partial class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
	{
		private readonly IUserRepository _userRepository;
		private readonly UserManager<ApplicationUser> _userManager;

		public LoginUserHandler(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
		{
			_userRepository = userRepository;
			_userManager = userManager;
		}

		public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			var appUser = await _userManager.FindByEmailAsync(request.Email);
			if (appUser == null)
				throw new UnauthorizedAccessException("Invalid credentials.");

			var passwordValid = await _userManager.CheckPasswordAsync(appUser, request.Password);
			if (!passwordValid)
				throw new UnauthorizedAccessException("Invalid credentials.");

			var domainUser = await _userRepository.GetByIdentityIdAsync(appUser.Id, cancellationToken);
			if (domainUser == null)
				throw new UnauthorizedAccessException("Domain user not found.");

			return new LoginUserResponse
			{
				UserId = domainUser.Id,
				Email = domainUser.Email
			};
		}
	}
}
