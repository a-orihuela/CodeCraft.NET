using CodeCraft.NET.Cross.Configuration;
using System.Diagnostics;

namespace CodeCraft.NET.Cross.Services
{
	public static class DockerManager
	{
		private static readonly DevEnvironmentConfig Config = DevEnvironmentConfig.Instance;

		public static async Task EnsureDatabaseIsRunningAsync()
		{
			Console.WriteLine("🐳 Checking Docker services...");

			if (!IsDockerRunning())
			{
				Console.WriteLine("❌ Docker is not running. Please start Docker Desktop.");
				return;
			}

			var dockerComposePath = GetDockerComposePath();
			if (!File.Exists(dockerComposePath))
			{
				Console.WriteLine($"❌ docker-compose.yml not found at: {dockerComposePath}");
				return;
			}

			try
			{
				if (!await IsPostgresHealthyAsync())
				{
					Console.WriteLine("🚀 Starting database services...");
					await StartServicesAsync();

					// Wait for services to be healthy with timeout
					await WaitForHealthyServicesAsync();
				}
				else
				{
					Console.WriteLine("✅ Database services are already running and healthy.");
				}

				PrintServiceInfo();
			}
			catch (TimeoutException ex)
			{
				Console.WriteLine($"⏰ Timeout waiting for services: {ex.Message}");
				Console.WriteLine("⚠️ Continuing without waiting for database...");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"❌ Error managing Docker services: {ex.Message}");
				Console.WriteLine("⚠️ Continuing without Docker services...");
			}
		}

		private static string GetDockerComposePath()
		{
			var solutionRoot = Config.GetSolutionRoot();
			return Path.Combine(solutionRoot, Config.DockerServices.ComposeFileName);
		}

		private static bool IsDockerRunning()
		{
			try
			{
				var psi = new ProcessStartInfo
				{
					FileName = "docker",
					Arguments = "info",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				using var process = Process.Start(psi);
				var timeoutTask = Task.Delay(5000);
				var processTask = process.WaitForExitAsync();

				var completedTask = Task.WhenAny(processTask, timeoutTask).Result;

				if (completedTask == timeoutTask)
				{
					process.Kill();
					return false;
				}

				return process.ExitCode == 0;
			}
			catch
			{
				return false;
			}
		}

		private static async Task<bool> IsPostgresHealthyAsync()
		{
			try
			{
				var dockerComposePath = GetDockerComposePath();
				var psi = new ProcessStartInfo
				{
					FileName = "docker",
					Arguments = "compose ps --services --filter status=running",
					WorkingDirectory = Path.GetDirectoryName(dockerComposePath),
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				using var process = Process.Start(psi);

				var timeoutTask = Task.Delay(10000);
				var processTask = process.WaitForExitAsync();

				var completedTask = await Task.WhenAny(processTask, timeoutTask);

				if (completedTask == timeoutTask)
				{
					process.Kill();
					return false;
				}

				var output = await process.StandardOutput.ReadToEndAsync();
				return output.Contains("postgres");
			}
			catch
			{
				return false;
			}
		}

		private static async Task StartServicesAsync()
		{
			var dockerComposePath = GetDockerComposePath();
			var psi = new ProcessStartInfo
			{
				FileName = "docker",
				Arguments = "compose up -d --wait", // Agregar --wait para timeout automático
				WorkingDirectory = Path.GetDirectoryName(dockerComposePath),
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using var process = Process.Start(psi);

			// Agregar timeout de 2 minutos
			var timeoutTask = Task.Delay(TimeSpan.FromMinutes(2));
			var processTask = process.WaitForExitAsync();

			var completedTask = await Task.WhenAny(processTask, timeoutTask);

			if (completedTask == timeoutTask)
			{
				process.Kill();
				throw new TimeoutException("Docker compose startup timed out after 2 minutes");
			}

			var output = await process.StandardOutput.ReadToEndAsync();
			var error = await process.StandardError.ReadToEndAsync();

			if (process.ExitCode != 0)
			{
				Console.WriteLine($"❌ Error starting services: {error}");
				throw new Exception($"Docker compose failed: {error}");
			}
			else
			{
				Console.WriteLine("✅ Services started successfully.");
				if (!string.IsNullOrEmpty(output))
				{
					Console.WriteLine($"Docker output: {output}");
				}
			}
		}

		private static async Task WaitForHealthyServicesAsync()
		{
			Console.WriteLine("⏳ Waiting for services to be healthy...");
			var maxAttempts = 15; // Reducir intentos
			var attempt = 0;

			while (attempt < maxAttempts)
			{
				try
				{
					if (await IsPostgresHealthyAsync())
					{
						if (await CheckPostgresConnectionAsync())
						{
							Console.WriteLine("✅ Database is ready!");
							return;
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"⚠️ Health check attempt {attempt + 1} failed: {ex.Message}");
				}

				attempt++;
				Console.WriteLine($"⏳ Waiting... ({attempt}/{maxAttempts})");
				await Task.Delay(3000); // Incrementar delay
			}

			throw new TimeoutException("Services did not become healthy within the expected time");
		}

		private static async Task<bool> CheckPostgresConnectionAsync()
		{
			try
			{
				var psi = new ProcessStartInfo
				{
					FileName = "docker",
					Arguments = $"exec {Config.DockerServices.PostgreSql.ContainerName} pg_isready -U {Config.DockerServices.PostgreSql.Username} -d {Config.DockerServices.PostgreSql.Database}",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				using var process = Process.Start(psi);

				// Timeout de 10 segundos para pg_isready
				var timeoutTask = Task.Delay(10000);
				var processTask = process.WaitForExitAsync();

				var completedTask = await Task.WhenAny(processTask, timeoutTask);

				if (completedTask == timeoutTask)
				{
					process.Kill();
					return false;
				}

				return process.ExitCode == 0;
			}
			catch
			{
				return false;
			}
		}

		private static void PrintServiceInfo()
		{
			var pgConfig = Config.DockerServices.PostgreSql;
			var pgAdminConfig = Config.DockerServices.PgAdmin;

			Console.WriteLine("\n📊 Service Information:");
			Console.WriteLine($"🗄️  PostgreSQL: {pgConfig.Host}:{pgConfig.Port}");
			Console.WriteLine($"   Database: {pgConfig.Database}");
			Console.WriteLine($"   User: {pgConfig.Username}");
			Console.WriteLine($"   Password: {pgConfig.Password}");
			Console.WriteLine($"\n🌐 pgAdmin: http://{pgAdminConfig.Host}:{pgAdminConfig.Port}");
			Console.WriteLine($"   Email: {pgAdminConfig.Email}");
			Console.WriteLine($"   Password: {pgAdminConfig.Password}");
			Console.WriteLine();
		}

		public static async Task StopServicesAsync()
		{
			Console.WriteLine("🛑 Stopping Docker services...");

			try
			{
				var dockerComposePath = GetDockerComposePath();
				var psi = new ProcessStartInfo
				{
					FileName = "docker",
					Arguments = "compose down",
					WorkingDirectory = Path.GetDirectoryName(dockerComposePath),
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				using var process = Process.Start(psi);

				var timeoutTask = Task.Delay(30000); // 30 segundos timeout
				var processTask = process.WaitForExitAsync();

				var completedTask = await Task.WhenAny(processTask, timeoutTask);

				if (completedTask == timeoutTask)
				{
					process.Kill();
					Console.WriteLine("⚠️ Timeout stopping services");
					return;
				}

				if (process.ExitCode == 0)
				{
					Console.WriteLine("✅ Services stopped successfully.");
				}
				else
				{
					var error = await process.StandardError.ReadToEndAsync();
					Console.WriteLine($"⚠️ Error stopping services: {error}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"❌ Exception stopping services: {ex.Message}");
			}
		}

		public static async Task<bool> AreServicesRunningAsync()
		{
			try
			{
				return await IsPostgresHealthyAsync();
			}
			catch
			{
				return false;
			}
		}

		public static async Task RestartServicesAsync()
		{
			Console.WriteLine("🔄 Restarting Docker services...");

			await StopServicesAsync();
			await Task.Delay(2000); // Esperar 2 segundos

			try
			{
				await StartServicesAsync();
				await WaitForHealthyServicesAsync();
				Console.WriteLine("✅ Services restarted successfully.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"❌ Error restarting services: {ex.Message}");
				throw;
			}
		}

		public static void PrintDockerStatus()
		{
			Console.WriteLine("🔍 Docker Status Check:");
			Console.WriteLine($"   Docker Running: {(IsDockerRunning() ? "✅" : "❌")}");
			Console.WriteLine($"   Compose File: {(File.Exists(GetDockerComposePath()) ? "✅" : "❌")} {GetDockerComposePath()}");

			Task.Run(async () =>
			{
				var servicesHealthy = await AreServicesRunningAsync();
				Console.WriteLine($"   Services Healthy: {(servicesHealthy ? "✅" : "❌")}");
			}).Wait();
		}
	}
}