using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Shintio.SourceGenerators.Utils;

namespace Shintio.SourceGenerators.Extensions;

public static class IncrementalGeneratorInitializationContextExtensions
{
	#region ValueProviders

	public static IncrementalValueProvider<IAssemblySymbol?> GetAssemblyValueProvider(
		this IncrementalGeneratorInitializationContext context,
		string assemblyName
	)
	{
		return context.CompilationProvider.Select((compilation, _) =>
			GeneratorHelper.GetAssembly(compilation, assemblyName));
	}

	public static IncrementalValueProvider<IAssemblySymbol[]> GetAssembliesValueProvider(
		this IncrementalGeneratorInitializationContext context,
		string[] assembliesNames
	)
	{
		return context.CompilationProvider.Select((compilation, _) =>
			GeneratorHelper.GetAssemblies(compilation, assembliesNames).ToArray());
	}

	#endregion

	public static void RegisterAssembliesTypesProcessing(
		this IncrementalGeneratorInitializationContext context,
		string[] assembliesNames,
		Func<INamedTypeSymbol, bool> typeFilter,
		params (string, Func<INamedTypeSymbol[], string>)[] codeGenerators
	)
	{
		var provider = context.GetAssembliesValueProvider(assembliesNames);

		context.RegisterSourceOutput(provider, (sourceProductionContext, assemblies) =>
		{
			var types = assemblies.GetAllTypes()
				.Where(typeFilter)
				.ToArray();

			foreach (var (fileName, generator) in codeGenerators)
			{
				sourceProductionContext.AddSource($"{fileName}.g.cs",
					SourceText.From(generator.Invoke(types), Encoding.UTF8));
			}
		});
	}
}