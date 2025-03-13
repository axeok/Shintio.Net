using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Shintio.CodeBuilder.CSharp.CodeBlocks;
using Shintio.CodeBuilder.CSharp.Factories;

namespace Shintio.CodeBuilder.CSharp.SourceGenerators.Factories;

public static class FileCodeBlockFactoryExtensions
{
	public static FileCodeBlock Create(
		this FileCodeBlockFactory factory,
		IEnumerable<string> usings,
		INamedTypeSymbol symbol,
		Action<ClassCodeBlock> builder
	)
	{
		return factory.Create(usings, symbol.ToString(), builder);
	}
}