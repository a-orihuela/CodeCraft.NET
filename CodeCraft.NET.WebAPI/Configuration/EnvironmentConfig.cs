using CodeCraft.NET.Application.DTOs.Identity;

namespace CodeCraft.NET.Server.Configuration
{
	public static class EnvironmentConfig
	{
		public static (
			string AppConnection, 
			string IdentityConnection, 
			JwtSettings JwtSettings,
			string AdminEmail, 
			string AdminUsername, 
			string AdminPassword, 
			string AdminRole) 
			Load(IConfiguration configuration)
		{
			var appConnection = configuration.GetConnectionString("Application");
			var identityConnection = configuration.GetConnectionString("Identity");
			var issuer = configuration["JwtSettings:Issuer"];
			var audience = configuration["JwtSettings:Audience"];
			var key = configuration["JwtSettings:Key"];
			var durationInMinutes = int.Parse(configuration["JwtSettings:DurationInMinutes"] ?? "60");
			var adminEmail = configuration["DefaultAdmin:Email"];
			var adminUsername = configuration["DefaultAdmin:Username"];
			var adminPassword = configuration["DefaultAdmin:Password"];
			var adminRole = configuration["DefaultAdmin:Role"];

			if (string.IsNullOrWhiteSpace(appConnection))
				throw new InvalidOperationException("Missing 'ConnectionStrings:Application'. Please set it via environment or appsettings.");
			if (string.IsNullOrWhiteSpace(identityConnection))
				throw new InvalidOperationException("Missing 'ConnectionStrings:Identity'. Please set it via environment or appsettings.");
			if(string.IsNullOrWhiteSpace(issuer))
				throw new InvalidOperationException("Missing 'JwtSettings:Issuer'. Please set it via environment or appsettings.");
			if (string.IsNullOrWhiteSpace(audience))
				throw new InvalidOperationException("Missing 'JwtSettings:Audience'. Please set it via environment or appsettings.");
			if (string.IsNullOrWhiteSpace(key))
				throw new InvalidOperationException("Missing 'JwtSettings:Key'. Please set it via environment or appsettings.");
			if (durationInMinutes <= 0)
				throw new InvalidOperationException("Invalid 'JwtSettings:DurationInMinutes'. It must be a positive integer.");
			if (string.IsNullOrWhiteSpace(adminEmail))
				throw new InvalidOperationException("Missing 'DefaultAdmin:Email'. Please set it via environment or appsettings.");
			if (string.IsNullOrWhiteSpace(adminUsername))
				throw new InvalidOperationException("Missing 'DefaultAdmin:Username'. Please set it via environment or appsettings.");
			if (string.IsNullOrWhiteSpace(adminPassword))
				throw new InvalidOperationException("Missing 'DefaultAdmin:Password'. Please set it via environment or appsettings.");
			if (string.IsNullOrWhiteSpace(adminRole))
				throw new InvalidOperationException("Missing 'DefaultAdmin:Role'. Please set it via environment or appsettings.");

			var jwtSettings = new JwtSettings
			{
				Issuer = issuer,
				Audience = audience,
				Key = key,
				DurationInMinutes = durationInMinutes
			};

			return (appConnection, identityConnection, jwtSettings, adminEmail, adminUsername, adminPassword, adminRole);
		}
	}
}
