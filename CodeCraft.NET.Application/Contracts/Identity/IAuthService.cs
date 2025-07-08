using CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Login;
using CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Register;

namespace CodeCraft.NET.Application.Contracts.Identity
{
	public interface IAuthService
	{
		Task<LoginUserResponse> LoginAsync(string email, string password);
		Task<(RegisterUserResponse Response, string ConfirmationLink)> RegisterAsync(string email, string password, string fullName);
		Task<bool> ConfirmEmailAsync(string userId, string token);
	}
}
