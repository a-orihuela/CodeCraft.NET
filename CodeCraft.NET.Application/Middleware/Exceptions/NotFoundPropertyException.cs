namespace CodeCraft.NET.Application.Middleware.Exceptions
{
	public class NotFoundPropertyException : ApplicationException
	{
		public NotFoundPropertyException(string name, object key) : base($"Property \"{name}\" ({key}) not found")
		{
		}
	}
}
