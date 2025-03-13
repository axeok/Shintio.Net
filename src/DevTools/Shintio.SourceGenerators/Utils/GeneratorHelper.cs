using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Shintio.SourceGenerators.Utils;

public static class GeneratorHelper
{
	public static IAssemblySymbol? GetAssembly(Compilation compilation, string name)
	{
		return compilation.References
			.Select(compilation.GetAssemblyOrModuleSymbol)
			.OfType<IAssemblySymbol>()
			.FirstOrDefault(s => s.Name == name);
	}

	public static IEnumerable<IAssemblySymbol> GetAssemblies(Compilation compilation, string[] names)
	{
		return compilation.References
			.Select(compilation.GetAssemblyOrModuleSymbol)
			.OfType<IAssemblySymbol>()
			.Where(s => names.Contains(s.Name));
	}
}