namespace CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Login
{
	public partial class LoginUserResponse
	{
		public int UserId { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
	}
}
