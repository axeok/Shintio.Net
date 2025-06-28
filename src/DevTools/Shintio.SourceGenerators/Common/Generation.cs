using Microsoft.CodeAnalysis;

namespace Shintio.SourceGenerators.Common;

public class Generation<T>(T data, SourceProductionContext context, ProjectMetadata metadata)
{
	public T Data { get; } = data;
	public SourceProductionContext Context { get; } = context;
	public ProjectMetadata Metadata { get; } = metadata;
}