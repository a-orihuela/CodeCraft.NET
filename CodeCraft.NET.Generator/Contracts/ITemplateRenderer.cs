namespace CodeCraft.NET.Generator.Contracts
{
	public interface ITemplateRenderer
	{
		/// <summary>
		/// Renders a template with the provided model and writes the output to the specified path.
		/// </summary>
		/// <param name="templatePath">Absolute path to the template file.</param>
		/// <param name="outputPath">Absolute path where the rendered file will be written.</param>
		/// <param name="model">Model object passed to the template.</param>
		void Render(string templatePath, string outputPath, object model);
	}
}
