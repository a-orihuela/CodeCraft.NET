namespace CodeCraft.NET.Application.Middleware.Exceptions
{
	public class ConflictException : ApplicationException
	{
		public ConflictException(string message)
			: base(message)
		{
		}
	}
}
