using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.CodeBuilder.CSharp.CodeBlocks;
using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Interfaces;
using Shintio.CodeBuilder.CSharp.Utils;

namespace Shintio.CodeBuilder.CSharp.Extensions;

public static class ClassCodeBlockExtensions
{
	#region Properties

	public static ClassCodeBlock AddProperty(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		IPropertyGetter? getter = null,
		IPropertySetter? setter = null,
		AccessModifier? setterAccessModifier = null
	) => codeBlock.AddProperty(CodeFactory.Property.Create(accessModifier, type, name, getter, setter,
		setterAccessModifier));

	public static ClassCodeBlock AddAutoProperty(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		bool hasSetter,
		AccessModifier? setterAccessModifier = null
	) => codeBlock.AddProperty(CodeFactory.Property.CreateAuto(accessModifier, type, name, hasSetter,
		setterAccessModifier));

	public static ClassCodeBlock AddBackingProperty(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		string fieldName,
		bool hasSetter,
		AccessModifier? setterAccessModifier = null
	) => codeBlock.AddProperty(CodeFactory.Property.CreateBacking(accessModifier, type, name, fieldName, hasSetter,
		setterAccessModifier));

	public static ClassCodeBlock AddBackingPropertyWithField(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		string fieldName,
		bool hasSetter,
		AccessModifier? fieldModifier = null,
		AccessModifier? setterAccessModifier = null
	) => codeBlock
		.AddField(CodeFactory.Field.Create(fieldModifier ?? AccessModifier.Private, type, fieldName))
		.AddProperty(CodeFactory.Property.CreateBacking(accessModifier, type, name, fieldName, hasSetter,
			setterAccessModifier));

	public static ClassCodeBlock AddBackingPropertyWithField(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		bool hasSetter,
		AccessModifier? fieldModifier = null,
		AccessModifier? setterAccessModifier = null
	)
	{
		var fieldName = $"_{name[0].ToString().ToLowerInvariant()}{name.Substring(1)}";

		return codeBlock
			.AddField(CodeFactory.Field.Create(fieldModifier ?? AccessModifier.Private, type, fieldName))
			.AddProperty(CodeFactory.Property.CreateBacking(accessModifier, type, name, fieldName, hasSetter,
				setterAccessModifier));
	}

	#endregion

	#region Constructors And Methods

	public static ConstructorCodeBlock AddConstructor(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		IEnumerable<ParameterCodeBlock> parameters
	) =>
		codeBlock.AddConstructor(CodeFactory.Constructor.Create(codeBlock, accessModifier, parameters));

	public static ConstructorCodeBlock AddConstructor(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		params ParameterCodeBlock[] parameters
	) =>
		codeBlock.AddConstructor(CodeFactory.Constructor.Create(codeBlock, accessModifier, parameters));

	public static MethodCodeBlock AddMethod(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		TypeInfo returnType,
		string name,
		IEnumerable<ParameterCodeBlock> parameters
	) =>
		codeBlock.AddMethod(CodeFactory.Method.Create(codeBlock, accessModifier, returnType, name, parameters));

	public static MethodCodeBlock AddMethod(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		TypeInfo returnType,
		string name,
		params ParameterCodeBlock[] parameters
	) =>
		codeBlock.AddMethod(CodeFactory.Method.Create(codeBlock, accessModifier, returnType, name, parameters));

	#endregion

	#region Other

	public static ClassCodeBlock AddField(
		this ClassCodeBlock codeBlock,
		AccessModifier accessModifier,
		TypeInfo type,
		string name
	) => codeBlock.AddField(CodeFactory.Field.Create(accessModifier, type, name));

	public static ClassCodeBlock AddRaw(this ClassCodeBlock codeBlock, string code) =>
		codeBlock.AddAdditionalBlock(new RawCodeBlock(code));

	public static ClassCodeBlock SetComment(this ClassCodeBlock codeBlock, string text) =>
		codeBlock.SetComment(new CommentCodeBlock(text));

	#endregion

	#region Multiple

	public static ClassCodeBlock AddFields<T>(
		this ClassCodeBlock codeBlock,
		IEnumerable<T> source,
		Func<T, FieldCodeBlock> factory
	)
	{
		foreach (var item in source.Select(factory))
		{
			codeBlock.AddField(item);
		}

		return codeBlock;
	}

	public static ClassCodeBlock AddProperties<T>(
		this ClassCodeBlock codeBlock,
		IEnumerable<T> source,
		Func<T, PropertyCodeBlock> factory
	)
	{
		foreach (var item in source.Select(factory))
		{
			codeBlock.AddProperty(item);
		}

		return codeBlock;
	}

	public static ClassCodeBlock AddConstructors<T>(
		this ClassCodeBlock codeBlock,
		IEnumerable<T> source,
		Func<T, ConstructorCodeBlock> factory
	)
	{
		foreach (var item in source.Select(factory))
		{
			codeBlock.AddConstructor(item);
		}

		return codeBlock;
	}

	public static ClassCodeBlock AddMethods<T>(
		this ClassCodeBlock codeBlock,
		IEnumerable<T> source,
		Func<T, MethodCodeBlock> factory
	)
	{
		foreach (var item in source.Select(factory))
		{
			codeBlock.AddMethod(item);
		}

		return codeBlock;
	}

	public static ClassCodeBlock AddMultiple<T>(
		this ClassCodeBlock codeBlock,
		IEnumerable<T> source,
		Func<T, IEnumerable<ICodeBlock>> factory
	)
	{
		foreach (var item in source.SelectMany(factory))
		{
			switch (item)
			{
				case FieldCodeBlock field:
					codeBlock.AddField(field);
					break;
				case PropertyCodeBlock property:
					codeBlock.AddProperty(property);
					break;
				case ConstructorCodeBlock constructor:
					codeBlock.AddConstructor(constructor);
					break;
				case MethodCodeBlock method:
					codeBlock.AddMethod(method);
					break;
				default:
					codeBlock.AddAdditionalBlock(item);
					break;
			}
		}

		return codeBlock;
	}

	#endregion
}