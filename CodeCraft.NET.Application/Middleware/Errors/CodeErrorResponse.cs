namespace CodeCraft.NET.Application.Middleware.Errors
{
	public class CodeErrorResponse
	{
		private readonly string ERROR_400 = "The Request sent has errors";
		private readonly string ERROR_401 = "You do not have authorization for this resource";
		private readonly string ERROR_404 = "The requested resource was not found";
		private readonly string ERROR_500 = "Errors occurred on the server";

		public int StatusCode { get; set; }
		public string? Message { get; set; }

		public CodeErrorResponse(int statusCode, string? message = null)
		{
			StatusCode = statusCode;
			Message = message ?? GetDefaultMessageStatusCode(statusCode);
		}

		private string GetDefaultMessageStatusCode(int statusCode)
		{
			return statusCode switch
			{
				400 => ERROR_400,
				401 => ERROR_401,
				404 => ERROR_404,
				500 => ERROR_500,
				_ => string.Empty
			};
		}
	}
}
