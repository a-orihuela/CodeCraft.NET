using CodeCraft.NET.Generator.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace CodeCraft.NET.Generator.Helpers
{
    public static class NuGetPackageManager
    {
        /// <summary>
        /// Ensures the correct Entity Framework package is installed based on the database provider
        /// </summary>
        /// <param name="databaseProvider">The database provider (SQLite or SqlServer)</param>
        public static void EnsureEntityFrameworkPackage(string databaseProvider)
        {
            var config = CodeCraftConfig.Instance;
            var infrastructureProjectPath = Path.Combine(
                config.GetSolutionRoot(),
                config.ProjectNames.Infrastructure,
                $"{config.ProjectNames.Infrastructure}.csproj"
            );

            Console.WriteLine($"   Ensuring Entity Framework packages for {databaseProvider}...");

            if (!File.Exists(infrastructureProjectPath))
            {
                Console.WriteLine($"   Warning: Infrastructure project file not found at {infrastructureProjectPath}");
                return;
            }

            try
            {
                var projectDoc = XDocument.Load(infrastructureProjectPath);
                bool hasChanges = false;

                // Define required packages for each provider
                var requiredPackages = new Dictionary<string, string>();
                var optionalPackages = new List<string>();

                if (databaseProvider.Equals("SQLite", StringComparison.OrdinalIgnoreCase))
                {
                    requiredPackages["Microsoft.EntityFrameworkCore.Sqlite"] = "9.0.0";
                    requiredPackages["Microsoft.EntityFrameworkCore.SqlServer"] = "9.0.7"; // Keep both for flexibility
                }
                else
                {
                    requiredPackages["Microsoft.EntityFrameworkCore.SqlServer"] = "9.0.7";
                    requiredPackages["Microsoft.EntityFrameworkCore.Sqlite"] = "9.0.0"; // Keep both for flexibility
                }

                // Find or create PackageReference ItemGroup
                var itemGroups = projectDoc.Root?.Elements("ItemGroup").ToList() ?? new List<XElement>();
                var packageGroup = itemGroups.FirstOrDefault(ig => ig.Elements("PackageReference").Any());
                
                if (packageGroup == null)
                {
                    packageGroup = new XElement("ItemGroup");
                    projectDoc.Root?.Add(packageGroup);
                }

                // Check and add missing packages
                foreach (var (packageId, version) in requiredPackages)
                {
                    var existingPackage = packageGroup.Elements("PackageReference")
                        .FirstOrDefault(pr => pr.Attribute("Include")?.Value == packageId);

                    if (existingPackage == null)
                    {
                        var newPackage = new XElement("PackageReference",
                            new XAttribute("Include", packageId),
                            new XAttribute("Version", version));
                        packageGroup.Add(newPackage);
                        hasChanges = true;
                        Console.WriteLine($"   Added package: {packageId} v{version}");
                    }
                    else
                    {
                        // Update version if needed
                        var currentVersion = existingPackage.Attribute("Version")?.Value;
                        if (currentVersion != version)
                        {
                            existingPackage.SetAttributeValue("Version", version);
                            hasChanges = true;
                            Console.WriteLine($"   Updated package: {packageId} v{currentVersion} -> v{version}");
                        }
                    }
                }

                // Save changes if any
                if (hasChanges)
                {
                    projectDoc.Save(infrastructureProjectPath);
                    Console.WriteLine($"   Updated Infrastructure project file");
                }
                else
                {
                    Console.WriteLine($"   Entity Framework packages already up to date");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Warning: Could not update packages automatically: {ex.Message}");
                Console.WriteLine($"   Please ensure the following packages are installed in Infrastructure project:");
                
                if (databaseProvider.Equals("SQLite", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"   - Microsoft.EntityFrameworkCore.Sqlite (9.0.0)");
                }
                else
                {
                    Console.WriteLine($"   - Microsoft.EntityFrameworkCore.SqlServer (9.0.7)");
                }
            }
        }

        /// <summary>
        /// Restores NuGet packages for the solution
        /// </summary>
        public static void RestorePackages()
        {
            try
            {
                var config = CodeCraftConfig.Instance;
                var solutionPath = Path.Combine(config.GetSolutionRoot(), config.ProjectNames.SolutionFileName);

                if (File.Exists(solutionPath))
                {
                    Console.WriteLine("   Restoring NuGet packages...");
                    
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "restore",
                        WorkingDirectory = config.GetSolutionRoot(),
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using var process = new Process { StartInfo = startInfo };
                    process.Start();
                    
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        Console.WriteLine("   NuGet packages restored successfully");
                    }
                    else
                    {
                        Console.WriteLine("   Warning: NuGet restore completed with warnings");
                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            var errorLines = error.Split('\n')
                                .Where(line => !string.IsNullOrWhiteSpace(line))
                                .Take(3);
                            
                            foreach (var line in errorLines)
                            {
                                Console.WriteLine($"   {line.Trim()}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Warning: Could not restore packages automatically: {ex.Message}");
                Console.WriteLine("   Please run 'dotnet restore' manually");
            }
        }
    }
}