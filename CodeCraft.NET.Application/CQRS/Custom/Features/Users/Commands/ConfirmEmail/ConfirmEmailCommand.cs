using MediatR;

namespace CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.ConfirmEmail
{
	public partial class ConfirmEmailCommand : IRequest<bool>
	{
		public string UserId { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
	}
}
