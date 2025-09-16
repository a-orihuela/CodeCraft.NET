using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace CodeCraft.NET.MAUI
{
	public partial class App : Microsoft.Maui.Controls.Application
	{
		public IServiceProvider? Services { get; private set; }

		public App()
		{
			InitializeComponent();
		}

		public App(IServiceProvider services) : this()
		{
			Services = services;
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			return new Window(new AppShell());
		}
	}
}