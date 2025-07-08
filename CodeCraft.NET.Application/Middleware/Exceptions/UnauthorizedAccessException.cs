
namespace CodeCraft.NET.Application.Middleware.Exceptions
{
	public class UnauthorizedAccessException : ApplicationException
	{
		public UnauthorizedAccessException(string message) : base(message)
		{
		}
	}
}
