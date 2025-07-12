using CodeCraft.NET.Generator.Helpers;
using CodeCraft.NET.Generator.Models;
using System.Reflection;

namespace CodeCraft.NET.Generator
{
	public static class EntityAnalyzer
	{
		public static List<EntityMetadata> AnalyzeDomainEntities()
		{
			var result = new List<EntityMetadata>();

			var domainAssembly = LoadTypesFromDomainAssembly();
			foreach (var type in domainAssembly)
			{
				var props = type
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(p => p.DeclaringType == type
						|| p.DeclaringType?.Name == "BaseDomainModel")
					.ToArray();

				var entity = new EntityMetadata
				{
					Name = type.Name,
					NamePlural = ConfigHelper.PluralizeName(type.Name),
					Properties = props
						.Select(p => new PropertyMetadata
						{
							Name = p.Name,
							Type = GetFriendlyTypeName(p.PropertyType),
							IsNavigation = IsNavigationProperty(p.PropertyType)
						}).ToList(),
					Usings = GetRequiredUsings(props)
				};
				result.Add(entity);
			}

			return result;
		}

		private static bool IsNavigationProperty(Type type)
		{
			if (TypeAliases.ContainsKey(type))
				return false;

			if (type.Namespace != null && type.Namespace.StartsWith("System"))
				return false;

			if (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
				return true;

			return true;
		}

		private static string GetFriendlyTypeName(Type type)
		{
			if (TypeAliases.TryGetValue(type, out var alias))
				return alias;

			if (type.IsGenericType)
			{
				if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					return GetFriendlyTypeName(type.GetGenericArguments()[0]) + "?";
				}

				string baseName = type.Name.Substring(0, type.Name.IndexOf('`'));
				string genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetFriendlyTypeName));
				return $"{baseName}<{genericArgs}>";
			}

			return type.Name;
		}

		private static readonly Dictionary<Type, string> TypeAliases = new()
		{
			{ typeof(string), "string" },
			{ typeof(int), "int" },
			{ typeof(bool), "bool" },
			{ typeof(double), "double" },
			{ typeof(decimal), "decimal" },
			{ typeof(object), "object" },
			{ typeof(void), "void" },
			{ typeof(byte), "byte" },
			{ typeof(char), "char" },
			{ typeof(long), "long" },
			{ typeof(short), "short" },
			{ typeof(uint), "uint" },
			{ typeof(ulong), "ulong" },
			{ typeof(ushort), "ushort" },
			{ typeof(sbyte), "sbyte" },
			{ typeof(float), "float" },
			{ typeof(DateTime), "DateTime" },
			{ typeof(DateOnly), "DateOnly" }
		};

		private static List<Type> LoadTypesFromDomainAssembly()
		{
			var config = CodeCraftConfig.Instance;
			var domainProjectName = config.ProjectNames.Domain;

			var domainAssembly = AppDomain.CurrentDomain.GetAssemblies()
				.FirstOrDefault(a => a.GetName().Name == domainProjectName);

			if (domainAssembly == null)
				domainAssembly = Assembly.Load(domainProjectName);

			return domainAssembly.GetTypes()
				.Where(t => t.IsClass && t.IsPublic && !t.IsAbstract)
				.ToList();
		}

		private static HashSet<string> GetRequiredUsings(PropertyInfo[] props)
		{
			var usings = new HashSet<string>();

			foreach (var prop in props)
			{
				if (prop.PropertyType.IsGenericType)
				{
					usings.Add(prop.PropertyType.GetGenericTypeDefinition().Namespace!);
					foreach (var arg in prop.PropertyType.GetGenericArguments())
					{
						if (!string.IsNullOrWhiteSpace(arg.Namespace))
							usings.Add(arg.Namespace!);
					}
				}
				else if (!string.IsNullOrWhiteSpace(prop.PropertyType.Namespace))
				{
					usings.Add(prop.PropertyType.Namespace!);
				}
			}

			return usings;
		}
	}
}