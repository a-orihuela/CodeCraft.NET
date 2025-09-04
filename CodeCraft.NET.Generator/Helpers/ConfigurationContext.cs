using CodeCraft.NET.Generator.Models;

namespace CodeCraft.NET.Generator.Helpers
{
    /// <summary>
    /// Static context that provides access to the current configuration throughout the generator
    /// </summary>
    public static class ConfigurationContext
    {
        private static CodeCraftOptions? _options;
        private static ProfileConfig? _activeProfile;
        
        public static CodeCraftOptions Options => _options ?? throw new InvalidOperationException("Configuration not initialized. Call Initialize() first.");
        public static ProfileConfig ActiveProfile => _activeProfile ?? throw new InvalidOperationException("Active profile not initialized. Call Initialize() first.");
        
        public static void Initialize(CodeCraftOptions options, ProfileConfig activeProfile)
        {
            _options = options;
            _activeProfile = activeProfile;
        }
        
        public static string GetSolutionRoot()
        {
            // Start from the current directory (where the generator is executed)
            var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            
            // First, try to find the solution file from current directory
            while (currentDir != null)
            {
                if (currentDir.GetFiles("*.sln").Any())
                {
                    return currentDir.FullName;
                }
                currentDir = currentDir.Parent;
            }
            
            // Fallback: try from AppContext.BaseDirectory (bin/Debug/net9.0)
            currentDir = new DirectoryInfo(AppContext.BaseDirectory);
            
            // Navigate up from bin/Debug/net9.0 to solution root
            while (currentDir != null)
            {
                if (currentDir.GetFiles("*.sln").Any())
                {
                    return currentDir.FullName;
                }
                currentDir = currentDir.Parent;
            }
            
            // SAFE FALLBACK: Use current directory instead of hardcoded path
            var currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"Warning: Could not find .sln file. Using current directory: {currentDirectory}");
            Console.WriteLine("   Make sure you're running the generator from within the solution directory.");
            
            return currentDirectory;
        }
        
        public static string GetSolutionRelativePath(string projectName)
        {
            var solutionRoot = GetSolutionRoot();
            var result = Path.Combine(solutionRoot, projectName);
            
            // SAFETY CHECK: Ensure we're not accidentally working in another solution
            if (!Directory.Exists(solutionRoot))
            {
                throw new DirectoryNotFoundException($"Solution root directory does not exist: {solutionRoot}");
            }
            
            // SAFETY CHECK: Verify this looks like our solution
            var solutionFiles = Directory.GetFiles(solutionRoot, "*.sln");
            if (!solutionFiles.Any())
            {
                Console.WriteLine($"Warning: No .sln files found in {solutionRoot}");
                Console.WriteLine("   This might not be the correct solution directory.");
            }
            
            return result;
        }
        
        public static string PluralizeName(string name)
        {
            if (name.EndsWith('y') && name.Length > 1 && !"aeiou".Contains(name[^2]))
                return name[..^1] + "ies";
            if (name.EndsWith('s') || name.EndsWith('x') || name.EndsWith('z') || name.EndsWith("ch") || name.EndsWith("sh"))
                return name + "es";
            return name + "s";
        }
    }
}