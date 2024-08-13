using Shintio.CodeGenerator.Enums;
using Shintio.Essentials.Common;

namespace Shintio.CodeGenerator.Models;

public class FileResult : ValueObject
{
	public FileResult(string name, ProjectInfo project, CodeLanguage codeLanguage, string content)
	{
		Name = name;
		Project = project;
		CodeLanguage = codeLanguage;
		Content = content;
	}

	public string Name { get; }
	public ProjectInfo Project { get; }
	public CodeLanguage CodeLanguage { get; }
	public string Content { get; }

	protected override IEnumerable<object?> GetEqualityComponents()
	{
		yield return Name;
		yield return CodeLanguage;
		yield return Project;
		yield return Content;
	}
}