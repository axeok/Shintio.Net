using System.Reflection;
using Shintio.CodeBuilder.CSharp.CodeBlocks;
using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Interfaces;
using TypeInfo = Shintio.CodeBuilder.CSharp.Components.TypeInfo;

namespace Shintio.CodeBuilder.CSharp.Factories;

public class PropertyCodeBlockFactory
{
	#region Raw

	public PropertyCodeBlock Create(
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		IPropertyGetter? getter = null,
		IPropertySetter? setter = null,
		AccessModifier? setterAccessModifier = null
	)
	{
		return new PropertyCodeBlock(accessModifier, type, name, getter, setter, setterAccessModifier);
	}

	public PropertyCodeBlock Create(FieldInfo field)
	{
		return new PropertyCodeBlock(AccessModifier.From(field), field, field.Name);
	}

	public PropertyCodeBlock Create(PropertyInfo property)
	{
		return new PropertyCodeBlock(
			AccessModifier.From(property),
			property,
			property.Name,
			setterAccessModifier: property.SetMethod != null
				? AccessModifier.From(property.SetMethod)
				: null
		);
	}

	#endregion

	#region Auto

	public AutoPropertyCodeBlock CreateAuto(
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		bool hasSetter,
		AccessModifier? setterAccessModifier = null
	)
	{
		return new AutoPropertyCodeBlock(accessModifier, type, name, hasSetter, setterAccessModifier);
	}

	public AutoPropertyCodeBlock CreateAuto(
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		bool hasSetter,
		object? defaultValue,
		AccessModifier? setterAccessModifier = null
	)
	{
		return new AutoPropertyCodeBlock(accessModifier, type, name, hasSetter, defaultValue, setterAccessModifier);
	}

	public AutoPropertyCodeBlock CreateAuto(FieldInfo field)
	{
		return new AutoPropertyCodeBlock(AccessModifier.From(field), field, field.Name, true)
		{
			AccessModifier = AccessModifier.From(field),
		};
	}

	public AutoPropertyCodeBlock CreateAuto(PropertyInfo property)
	{
		return new AutoPropertyCodeBlock(
			AccessModifier.From(property),
			property,
			property.Name,
			property.CanWrite,
			setterAccessModifier: property.SetMethod != null
				? AccessModifier.From(property.SetMethod)
				: null
		);
	}

	#endregion

	#region Backing

	public BackingPropertyCodeBlock CreateBacking(
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		string fieldName,
		bool hasSetter,
		AccessModifier? setterAccessModifier = null
	)
	{
		return new BackingPropertyCodeBlock(accessModifier, type, name, fieldName, hasSetter, setterAccessModifier);
	}

	#endregion
}