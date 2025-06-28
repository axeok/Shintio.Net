using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Shintio.SourceGenerators.Common;
using Shintio.SourceGenerators.Utils;

namespace Shintio.SourceGenerators.Extensions;

public static class IncrementalGeneratorInitializationContextExtensions
{
	private static readonly string[] ProjectDirectoryKeys =
	[
		"build_property.projectdir", "build_property.MSBuildProjectDirectory", "build_property.msbuildprojectdirectory"
	];

	private static readonly string[] RootNamespaceKeys =
	[
		"build_property.rootnamespace", "build_property.RootNamespace"
	];

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

	public static IncrementalValueProvider<string> GetProjectDirectoryProvider(
		this IncrementalGeneratorInitializationContext context
	)
	{
		return context.AnalyzerConfigOptionsProvider.Select((options, _) =>
		{
			var projectDirectory = string.Empty;

			foreach (var key in ProjectDirectoryKeys)
			{
				if (options.GlobalOptions.TryGetValue(key, out var value))
				{
					projectDirectory = value;
					break;
				}
			}

			return projectDirectory;
		});
	}

	public static IncrementalValueProvider<(string, string)> GetProjectMetadataValueProvider(
		this IncrementalGeneratorInitializationContext context
	)
	{
		return context.AnalyzerConfigOptionsProvider.Select((options, _) =>
		{
			var projectDirectory = string.Empty;
			var rootNamespace = string.Empty;

			foreach (var key in ProjectDirectoryKeys)
			{
				if (options.GlobalOptions.TryGetValue(key, out var value))
				{
					projectDirectory = value;
					break;
				}
			}

			foreach (var key in RootNamespaceKeys)
			{
				if (options.GlobalOptions.TryGetValue(key, out var value))
				{
					rootNamespace = value;
					break;
				}
			}

			return (projectDirectory, rootNamespace);
		});
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