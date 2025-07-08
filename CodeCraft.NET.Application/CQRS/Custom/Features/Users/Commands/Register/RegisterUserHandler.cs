using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;
using CodeCraft.NET.Application.DTOs.Identity;
using CodeCraft.NET.Application.Middleware.Exceptions;
using CodeCraft.NET.Domain.Model;
using CodeCraft.NET.Application.Contracts.Persistence.Repositories;

namespace CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Register
{
	public partial class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
	{
		private readonly IUserRepository _userRepository;
		private readonly UserManager<ApplicationUser> _userManager;

		public RegisterUserHandler(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
		{
			_userRepository = userRepository;
			_userManager = userManager;
		}

		public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			var existingUser = await _userManager.FindByEmailAsync(request.Email);
			if (existingUser != null)
				throw new ConflictException("Email is already in use.");

			var appUser = new ApplicationUser
			{
				Email = request.Email,
				UserName = request.Email
			};

			var createResult = await _userManager.CreateAsync(appUser, request.Password);

			if (!createResult.Succeeded)
				throw new FluentValidation.ValidationException(createResult.Errors.Select(e => new ValidationFailure(e.Code, e.Description)));

			var domainUser = new User
			{
				Email = request.Email,
				FullName = request.FullName,
				IdentityId = appUser.Id,
				OrganizationId = request.OrganizationId,
			};

			await _userRepository.AddAsync(domainUser);

			return new RegisterUserResponse
			{
				UserId = domainUser.Id,
				Email = domainUser.Email
			};
		}
	}
}
