using System;
using System.Collections.Generic;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class NamespaceCodeBlock : CodeBlockBase
{
	public NamespaceCodeBlock(string name)
	{
		Name = name;
	}

	public string Name { get; set; }

	public List<string> Usings { get; set; } = new();
	public List<ClassCodeBlock> Classes { get; set; } = new();

	protected override string BuildInternal()
	{
		var body = GetBody();

		return $$"""
			namespace {{Name}}
			{{{(string.IsNullOrWhiteSpace(body) ? string.Empty : Environment.NewLine + body)}}
			}
			""";
	}

	private string GetBody()
	{
		var result = new List<string>();

		foreach (var value in Usings)
		{
			result.Add($"\tusing {value};");
		}

		if (Usings.Count > 0 && Classes.Count > 0)
		{
			result.Add(string.Empty);
		}

		foreach (var value in Classes)
		{
			result.Add(value.GetCode(1));
		}

		return string.Join(Environment.NewLine, result);
	}
}