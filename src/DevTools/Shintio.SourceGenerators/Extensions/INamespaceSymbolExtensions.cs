using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Shintio.SourceGenerators.Extensions;

public static class INamespaceSymbolExtensions
{
	public static IEnumerable<INamedTypeSymbol> GetAllTypes(this INamespaceSymbol namespaceSymbol)
	{
		foreach (var type in namespaceSymbol.GetTypeMembers())
		{
			yield return type;
		}

		foreach (var subNamespace in namespaceSymbol.GetNamespaceMembers())
		{
			foreach (var type in GetAllTypes(subNamespace))
			{
				yield return type;
			}
		}
	}
}