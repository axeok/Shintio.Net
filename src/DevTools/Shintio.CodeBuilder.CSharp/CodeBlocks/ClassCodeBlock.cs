using System;
using System.Collections.Generic;
using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Extensions;
using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class ClassCodeBlock : CodeBlockBase
{
	public ClassCodeBlock(string name)
	{
		Name = name;
	}

	public AccessModifier AccessModifier { get; set; } = AccessModifier.Internal;
	public string Name { get; set; }

	public bool IsPartial { get; set; } = false;
	public bool IsStatic { get; set; } = false;
	public bool IsAbstract { get; set; } = false;
	public CommentCodeBlock? Comment { get; set; } = null;

	public List<ConstructorCodeBlock> Constructors { get; } = [];
	public List<FieldCodeBlock> Fields { get; } = [];
	public List<PropertyCodeBlock> Properties { get; } = [];
	public List<MethodCodeBlock> Methods { get; } = [];

	public List<ICodeBlock> AdditionalBlocks { get; } = [];

	public ClassCodeBlock Partial()
	{
		IsPartial = true;
		return this;
	}

	public ClassCodeBlock Static()
	{
		IsStatic = true;
		return this;
	}

	public ClassCodeBlock Abstract()
	{
		IsAbstract = true;
		return this;
	}

	public ClassCodeBlock SetComment(CommentCodeBlock comment)
	{
		Comment = comment;
		return this;
	}

	public ConstructorCodeBlock AddConstructor(ConstructorCodeBlock constructor)
	{
		Constructors.Add(constructor);

		return constructor;
	}

	public ClassCodeBlock AddField(FieldCodeBlock field)
	{
		Fields.Add(field);

		return this;
	}

	public ClassCodeBlock AddProperty(PropertyCodeBlock property)
	{
		Properties.Add(property);

		return this;
	}

	public MethodCodeBlock AddMethod(MethodCodeBlock method)
	{
		Methods.Add(method);

		return method;
	}

	public ClassCodeBlock AddAdditionalBlock(ICodeBlock codeBlock)
	{
		AdditionalBlocks.Add(codeBlock);

		return this;
	}

	protected override string BuildInternal()
	{
		var @abstract = IsAbstract ? "abstract " : string.Empty;
		var @static = IsStatic ? "static " : string.Empty;
		var @partial = IsPartial ? "partial " : string.Empty;

		var body = GetBody();

		return $$"""
			{{Comment.GetCode()}}public {{@abstract}}{{@static}}{{@partial}}class {{Name}}
			{{{(string.IsNullOrWhiteSpace(body) ? string.Empty : Environment.NewLine + body)}}
			}
			""";
	}

	private string GetBody()
	{
		var result = new List<string>();

		result.AddRange(GetCodeForBlocks(Fields));
		result.AddRange(GetCodeForBlocks(Constructors));
		result.AddRange(GetCodeForBlocks(Properties));
		result.AddRange(GetCodeForBlocks(Methods));
		result.AddRange(GetCodeForBlocks(AdditionalBlocks, true));

		return string.Join(Environment.NewLine, result);
	}

	private IEnumerable<string> GetCodeForBlocks<T>(List<T> blocks, bool ignoreNewLine = false) where T : ICodeBlock
	{
		foreach (var block in blocks)
		{
			yield return block.GetCode(1);
		}

		if (!ignoreNewLine && blocks.Count > 0)
		{
			yield return string.Empty;
		}
	}
}