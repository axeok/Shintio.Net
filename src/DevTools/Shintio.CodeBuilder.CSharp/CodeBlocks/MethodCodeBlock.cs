using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Extensions;
using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class MethodCodeBlock : CodeBlockBase
{
	public MethodCodeBlock(
		ClassCodeBlock forClass,
		AccessModifier accessModifier,
		TypeInfo returnType,
		string name,
		IEnumerable<ParameterCodeBlock> parameters
	)
	{
		Class = forClass;
		AccessModifier = accessModifier;
		ReturnType = returnType;
		Name = name;
		Parameters = parameters.ToList();
	}

	public AccessModifier AccessModifier { get; set; }
	public TypeInfo ReturnType { get; set; }
	public string Name { get; set; }
	public ClassCodeBlock Class { get; set; }
	public List<ParameterCodeBlock> Parameters { get; set; }

	public bool IsPartial { get; set; } = false;
	public bool IsStatic { get; set; } = false;
	public bool IsAbstract { get; set; } = false;
	public bool IsVirtual { get; set; } = false;
	public bool IsOverride { get; set; } = false;

	public ICodeBlock? Body { get; set; }

	public MethodCodeBlock Partial()
	{
		IsPartial = true;
		return this;
	}

	public MethodCodeBlock Static()
	{
		IsStatic = true;
		return this;
	}

	public MethodCodeBlock Abstract()
	{
		IsAbstract = true;
		return this;
	}

	public MethodCodeBlock Virtual()
	{
		IsVirtual = true;
		return this;
	}

	public MethodCodeBlock Override()
	{
		IsOverride = true;
		return this;
	}

	public ClassCodeBlock WithBody(ICodeBlock body)
	{
		Body = body;
		return Class;
	}

	protected override string GetCodeInternal()
	{
		var @abstract = IsAbstract ? "abstract " : string.Empty;
		var @static = IsStatic ? "static " : string.Empty;
		var @partial = IsPartial ? "partial " : string.Empty;
		var @virtual = IsVirtual ? "virtual " : string.Empty;
		var @override = IsOverride ? "override " : string.Empty;

		var body = Body?.GetCode(1);

		return $$"""
			{{AccessModifier}} {{@abstract}}{{@static}}{{@virtual}}{{@override}}{{@partial}}{{ReturnType}} {{Name}}({{Parameters.Join()}})
			{{{(string.IsNullOrWhiteSpace(body) ? string.Empty : Environment.NewLine + body)}}
			}
			""";
	}
}