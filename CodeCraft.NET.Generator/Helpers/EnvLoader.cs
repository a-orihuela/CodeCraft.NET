namespace CodeCraft.NET.Generator.Helpers
{
	public static class EnvLoader
	{
		public static void LoadEnvFile(string envFilePath)
		{
			if (!File.Exists(envFilePath))
				return;

			foreach (var line in File.ReadAllLines(envFilePath))
			{
				var trimmed = line.Trim();

				// Omit empty lines and comments
				if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
					continue;

				var parts = trimmed.Split('=', 2);
				if (parts.Length != 2)
					continue;

				var key = parts[0].Trim();
				var value = parts[1].Trim().Trim('"'); // Remove surrounding quotes if any

				// Only set if not already defined
				if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(key)))
				{
					Environment.SetEnvironmentVariable(key, value);
				}
			}
		}
	}
}
