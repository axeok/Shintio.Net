using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Extensions;
using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class ConstructorCodeBlock : CodeBlockBase
{
	public ConstructorCodeBlock(
		ClassCodeBlock forClass,
		AccessModifier accessModifier,
		IEnumerable<ParameterCodeBlock> parameters
	)
	{
		Class = forClass;
		AccessModifier = accessModifier;
		Parameters = parameters.ToList();
	}

	public AccessModifier AccessModifier { get; set; }
	public ClassCodeBlock Class { get; set; }
	public List<ParameterCodeBlock> Parameters { get; set; }

	public ICodeBlock? Body { get; set; }
	public IEnumerable<string>? ThisParameters { get; set; }
	public IEnumerable<string>? BaseParameters { get; set; }

	public ClassCodeBlock WithBody(ICodeBlock body)
	{
		Body = body;
		return Class;
	}

	public ConstructorCodeBlock WithThis(IEnumerable<string> parameters)
	{
		ThisParameters = parameters;
		return this;
	}

	public ConstructorCodeBlock WithBase(IEnumerable<string> parameters)
	{
		BaseParameters = parameters;
		return this;
	}

	protected override string GetCodeInternal()
	{
		var body = Body?.GetCode(1);

		var thisConstructor = ThisParameters == null
			? ""
			: $" : this({string.Join(", ", ThisParameters)})";

		var baseConstructor = BaseParameters == null
			? ""
			: $" : base({string.Join(", ", BaseParameters)})";

		return $$"""
			{{AccessModifier}} {{Class.Name}}({{Parameters.Join()}}){{thisConstructor}}{{baseConstructor}}
			{{{(string.IsNullOrWhiteSpace(body) ? string.Empty : Environment.NewLine + body)}}
			}
			""";
	}
}