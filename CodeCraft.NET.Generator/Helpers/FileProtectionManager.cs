using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class FileProtectionManager
	{
		public static bool ShouldPreserveFile(string filePath)
		{
			var config = CodeCraftConfig.Instance.MauiConfig;
			
			// Files that are never regenerated
			var protectedPatterns = new[]
			{
				"**/Custom/**",
				"**/*.Custom.*",
				"**/*Override.*",
				"**/*.xaml" // Customizable XAML files are preserved by default
			};
			
			// Generated files that are always overwritten
			var generatedPatterns = new[]
			{
				"**/*.Generated.*",
				"**/*Generated.*"
			};
			
			// If it's a generated file, don't preserve
			if (generatedPatterns.Any(pattern => FilePathMatches(filePath, pattern)))
			{
				return false;
			}
			
			// If it matches protected patterns, preserve
			return protectedPatterns.Any(pattern => FilePathMatches(filePath, pattern)) ||
				   config.GenerateOnlyIfNotExists.Any(pattern => FilePathMatches(filePath, pattern));
		}
		
		public static void BackupFileIfExists(string filePath)
		{
			if (File.Exists(filePath))
			{
				var backupPath = $"{filePath}.backup.{DateTime.Now:yyyyMMddHHmmss}";
				File.Copy(filePath, backupPath);
				Console.WriteLine($"   Backed up: {Path.GetFileName(backupPath)}");
			}
		}
		
		public static bool ShouldGenerateOnlyIfNotExists(string filePath)
		{
			var config = CodeCraftConfig.Instance.MauiConfig;
			return config.GenerateOnlyIfNotExists.Any(pattern => FilePathMatches(filePath, pattern));
		}
		
		private static bool FilePathMatches(string filePath, string pattern)
		{
			// Convert pattern to simple regex
			var regexPattern = pattern
				.Replace("**", ".*")
				.Replace("*", "[^/\\\\]*")
				.Replace("/", "[/\\\\]");
			
			return System.Text.RegularExpressions.Regex.IsMatch(
				filePath.Replace('\\', '/'), 
				regexPattern, 
				System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		}
	}
}