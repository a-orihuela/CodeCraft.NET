using MediatR;

namespace CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Login
{
	public partial class LoginUserCommand : IRequest<LoginUserResponse>
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
