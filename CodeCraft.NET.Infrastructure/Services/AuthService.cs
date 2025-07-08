using Microsoft.AspNetCore.Identity;
using CodeCraft.NET.Application.Contracts.Persistence.Repositories;
using CodeCraft.NET.Domain.Model;
using MediatR;
using CodeCraft.NET.Application.Middleware.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Register;
using CodeCraft.NET.Application.Contracts.Identity;
using CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Login;
using CodeCraft.NET.Application.DTOs.Identity;

namespace CodeCraft.NET.Infrastructure.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUserRepository _userRepository;
		private readonly JwtSettings _jwtSettings;
		private readonly IMediator _mediator;

		public AuthService(
			UserManager<ApplicationUser> userManager,
			IUserRepository userRepository,
			JwtSettings jwtSettings,
			IMediator mediator)
		{
			_userManager = userManager;
			_userRepository = userRepository;
			_jwtSettings = jwtSettings;
			_mediator = mediator;
		}

		public async Task<LoginUserResponse> LoginAsync(string email, string password)
		{
			var appUser = await _userManager.FindByEmailAsync(email);

			if (appUser == null)
				throw new Application.Middleware.Exceptions.UnauthorizedAccessException("Invalid credentials.");

			if (!await _userManager.CheckPasswordAsync(appUser, password))
				throw new Application.Middleware.Exceptions.UnauthorizedAccessException("Invalid credentials.");

			var domainUser = await _userRepository.GetByIdentityIdAsync(appUser.Id);
			if (domainUser == null)
				throw new Application.Middleware.Exceptions.UnauthorizedAccessException("Domain user not found.");

			var token = GenerateJwtToken(appUser, domainUser.Id);

			return new LoginUserResponse
			{
				UserId = domainUser.Id,
				Email = domainUser.Email,
				Token = token
			};
		}

		public async Task<(RegisterUserResponse Response, string ConfirmationLink)> RegisterAsync(string email, string password, string fullName)
		{
			var existingUser = await _userManager.FindByEmailAsync(email);
			if (existingUser != null)
				throw new ConflictException("Email is already in use.");

			var appUser = new ApplicationUser
			{
				Email = email,
				UserName = email
			};

			var createResult = await _userManager.CreateAsync(appUser, password);

			if (!createResult.Succeeded)
				throw new ValidationException(createResult.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));

			var domainUser = new User
			{
				Email = email,
				FullName = fullName,
				IdentityId = appUser.Id,
			};

			await _userRepository.AddAsync(domainUser);

			var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
			var confirmationLink = $"https://localhost:5001/api/auth/confirmemail?userId={appUser.Id}&token={Uri.EscapeDataString(confirmationToken)}";

			var response = new RegisterUserResponse
			{
				UserId = domainUser.Id,
				Email = domainUser.Email
			};

			return (response, confirmationLink);
		}

		public async Task<bool> ConfirmEmailAsync(string userId, string token)
		{
			var appUser = await _userManager.FindByIdAsync(userId);

			if (appUser == null)
				throw new NotFoundException("User not found.", 0);

			var result = await _userManager.ConfirmEmailAsync(appUser, token);

			if (!result.Succeeded)
				throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Code, e.Description)));

			return true;
		}

		private string GenerateJwtToken(ApplicationUser user, int domainUserId)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim("domainUserId", domainUserId.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}