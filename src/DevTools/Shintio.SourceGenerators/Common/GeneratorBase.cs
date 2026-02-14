using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Shintio.SourceGenerators.Extensions;

namespace Shintio.SourceGenerators.Common;

public abstract class GeneratorBase<T> : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var provider = GetValueProvider(context)
			.Combine(context.GetProjectMetadataValueProvider());

		context.RegisterSourceOutput(provider, (sourceProductionContext, providers) =>
		{
			var (data, (projectDirectory, rootNamespace)) = providers;

			var generation = new Generation<T>(
				data,
				sourceProductionContext,
				new ProjectMetadata(projectDirectory, rootNamespace)
			);

			if (!NeedToGenerate(generation))
			{
				return;
			}

			foreach (var pair in GenerateFiles(generation))
			{
				sourceProductionContext.AddSource($"{pair.Key}.g.cs", SourceText.From(pair.Value, Encoding.UTF8));
			}
		});
	}
	
	protected abstract IEnumerable<KeyValuePair<string, string>> GenerateFiles(Generation<T> generation);
	protected abstract IncrementalValueProvider<T> GetValueProvider(IncrementalGeneratorInitializationContext context);

	protected virtual bool NeedToGenerate(Generation<T> generation)
	{
		return true;
	}
}