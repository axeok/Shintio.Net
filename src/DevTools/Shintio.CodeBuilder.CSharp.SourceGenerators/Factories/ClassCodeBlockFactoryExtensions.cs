using Microsoft.CodeAnalysis;
using Shintio.CodeBuilder.CSharp.CodeBlocks;
using Shintio.CodeBuilder.CSharp.Factories;

namespace Shintio.CodeBuilder.CSharp.SourceGenerators.Factories;

public static class ClassCodeBlockFactoryExtensions
{
	public static ClassCodeBlock Create(this ClassCodeBlockFactory factory, INamedTypeSymbol symbol)
	{
		return factory.Create(symbol.Name).Partial();
	}
}