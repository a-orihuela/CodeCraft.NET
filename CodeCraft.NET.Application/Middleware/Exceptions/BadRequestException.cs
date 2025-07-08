namespace CodeCraft.NET.Application.Middleware.Exceptions
{
	public class BadRequestException : ApplicationException
	{
		public BadRequestException(string message) : base(message)
		{
		}
	}
}
