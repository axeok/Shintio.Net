using System.IO;

namespace Shintio.SourceGenerators.Common;

public class ProjectMetadata(string projectDirectory, string rootNamespace)
{
	public string ProjectDirectory { get; } = projectDirectory;
	public string ProjectName { get; } = GetProjectName(projectDirectory);
	public string RootNamespace { get; } = rootNamespace;

	public static string GetProjectName(string directory)
	{
		return Path.GetFileName(Path.GetDirectoryName(directory))!;
	}
}