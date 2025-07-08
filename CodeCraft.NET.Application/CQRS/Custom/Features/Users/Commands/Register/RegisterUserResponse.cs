namespace CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Register
{
	public partial class RegisterUserResponse
	{
		public int UserId { get; set; }
		public string Email { get; set; } = string.Empty;
	}
}
