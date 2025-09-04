using CodeCraft.NET.Generator.Helpers;

namespace CodeCraft.NET.Generator.Helpers
{
	public static class FileProtectionManager
	{
		public static bool ShouldProtectFile(string filePath)
		{
			var config = ConfigurationContext.Options;
			
			// Extract relative path from solution root
			var solutionRoot = ConfigurationContext.GetSolutionRoot();
			var relativePath = Path.GetRelativePath(solutionRoot, filePath);
			
			// Check against protection patterns
			var protectedPatterns = config.Shared.MauiConfig.GenerateOnlyIfNotExists;
			
			foreach (var pattern in protectedPatterns)
			{
				if (IsPatternMatch(relativePath, pattern))
				{
					return true;
				}
			}
			
			// Also protect files with custom suffix
			var customSuffix = config.Shared.MauiConfig.CustomFilesSuffix;
			if (!string.IsNullOrEmpty(customSuffix) && filePath.Contains(customSuffix))
			{
				return true;
			}
			
			return false;
		}
		
		public static bool ShouldGenerateFile(string outputPath)
		{
			// If file doesn't exist, always generate
			if (!File.Exists(outputPath))
			{
				return true;
			}
			
			// If file exists but is not protected, regenerate based on settings
			if (!ShouldProtectFile(outputPath))
			{
				return true; // Always regenerate non-protected files
			}
			
			// Protected file exists, don't regenerate
			return false;
		}
		
		private static bool IsPatternMatch(string path, string pattern)
		{
			// Simple pattern matching - could be enhanced with regex
			// For now, handle basic wildcards
			pattern = pattern.Replace("**", "*"); // Normalize double wildcards
			
			if (pattern.Contains('*'))
			{
				var parts = pattern.Split('*', StringSplitOptions.RemoveEmptyEntries);
				var currentIndex = 0;
				
				foreach (var part in parts)
				{
					var index = path.IndexOf(part, currentIndex, StringComparison.OrdinalIgnoreCase);
					if (index == -1)
					{
						return false;
					}
					currentIndex = index + part.Length;
				}
				
				return true;
			}
			else
			{
				return path.Equals(pattern, StringComparison.OrdinalIgnoreCase);
			}
		}
	}
}