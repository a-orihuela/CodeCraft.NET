using MediatR;

namespace CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Register
{
	public partial class RegisterUserCommand : IRequest<RegisterUserResponse>
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public int? OrganizationId { get; set; }
	}
}
