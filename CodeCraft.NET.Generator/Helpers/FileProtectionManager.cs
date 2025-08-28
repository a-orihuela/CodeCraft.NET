using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class FileProtectionManager
	{
		public static bool ShouldPreserveFile(string filePath)
		{
			var config = CodeCraftConfig.Instance.MauiConfig;
			
			// Archivos que nunca se regeneran
			var protectedPatterns = new[]
			{
				"**/Custom/**",
				"**/*.Custom.*",
				"**/*Override.*",
				"**/*.xaml" // Archivos XAML personalizables se preservan por defecto
			};
			
			// Archivos generados que siempre se sobrescriben
			var generatedPatterns = new[]
			{
				"**/*.Generated.*",
				"**/*Generated.*"
			};
			
			// Si es un archivo generado, no preservar
			if (generatedPatterns.Any(pattern => FilePathMatches(filePath, pattern)))
			{
				return false;
			}
			
			// Si coincide con patrones protegidos, preservar
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
			// Convertir patrón a regex simple
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