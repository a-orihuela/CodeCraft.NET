namespace CodeCraft.NET.Application.Middleware.Exceptions
{
	public class NotFoundException : ApplicationException
	{
		public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) not found")
		{
		}
	}
}
