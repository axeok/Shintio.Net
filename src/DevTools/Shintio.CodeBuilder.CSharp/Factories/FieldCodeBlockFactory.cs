using System.Reflection;
using Shintio.CodeBuilder.CSharp.CodeBlocks;
using Shintio.CodeBuilder.CSharp.Components;
using TypeInfo = Shintio.CodeBuilder.CSharp.Components.TypeInfo;

namespace Shintio.CodeBuilder.CSharp.Factories;

public class FieldCodeBlockFactory
{
	public FieldCodeBlock Create(AccessModifier accessModifier, TypeInfo type, string name)
	{
		return new FieldCodeBlock(accessModifier, type, name);
	}

	public FieldCodeBlock Create(FieldInfo field)
	{
		return new FieldCodeBlock(AccessModifier.From(field), field, field.Name);
	}

	public FieldCodeBlock Create(PropertyInfo property)
	{
		return new FieldCodeBlock(AccessModifier.From(property), property, property.Name);
	}
}