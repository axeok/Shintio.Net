using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.CodeBuilder.CSharp.CodeBlocks;

namespace Shintio.CodeBuilder.CSharp.Factories;

public class FileCodeBlockFactory
{
	public FileCodeBlock Create(
		IEnumerable<string> usings,
		string @namespace,
		string @class,
		Action<ClassCodeBlock> builder
	)
	{
		var result = new FileCodeBlock(
			new ClassCodeBlock(@class).Partial(),
			new NamespaceCodeBlock(@namespace) { Usings = usings.ToList() }
		);

		builder.Invoke(result.Class);

		return result;
	}

	public FileCodeBlock Create(
		IEnumerable<string> usings,
		string fullName,
		Action<ClassCodeBlock> builder
	)
	{
		var split = fullName.Split('.');

		return Create(usings, string.Join(".", split.Take(split.Length - 1)), split.Last(), builder);
	}
}