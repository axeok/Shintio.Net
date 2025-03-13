using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Shintio.SourceGenerators.Extensions;

public static class IAssemblySymbolExtensions
{
	public static IEnumerable<INamedTypeSymbol> GetAllTypes(this IAssemblySymbol? assembly)
	{
		return assembly?.GlobalNamespace.GetAllTypes() ?? [];
	}

	public static IEnumerable<INamedTypeSymbol> GetAllTypes(this IEnumerable<IAssemblySymbol> assemblies)
	{
		return assemblies.SelectMany(a => a.GlobalNamespace.GetAllTypes());
	}
}