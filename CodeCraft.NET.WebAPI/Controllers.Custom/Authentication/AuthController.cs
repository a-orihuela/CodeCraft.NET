using MediatR;
using Microsoft.AspNetCore.Mvc;
using CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Login;
using CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.Register;
using CodeCraft.NET.Application.CQRS.Custom.Features.Users.Commands.ConfirmEmail;

namespace CodeCraft.NET.WebAPI.Controllers.Custom.Authentication
{
	[ApiController]
	[Route("api/[controller]")]
	public partial class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("sign-in")]
		public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserCommand command)
		{
			try
			{
				var result = await _mediator.Send(command);
				return Ok(result);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Unauthorized(new { message = ex.Message });
			}
		}

		[HttpPost("register")]
		public async Task<ActionResult<RegisterUserResponse>> Register([FromBody] RegisterUserCommand command)
		{
			try
			{
				var result = await _mediator.Send(command);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("confirmemail")]
		public async Task<ActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
		{
			try
			{
				var success = await _mediator.Send(command);
				return Ok(new { success });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
