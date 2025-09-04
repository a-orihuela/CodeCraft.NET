using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCraft.NET.Generator.Models
{
	public class SharedConfig
	{
		public Dictionary<string, string> Templates { get; set; } = new();
		public Dictionary<string, string> Files { get; set; } = new();
		public Dictionary<string, string> Folders { get; set; } = new();
		public Dictionary<string, string> ProjectNames { get; set; } = new();
		public MauiConfig MauiConfig { get; set; } = new();
	}
}
