using System.Collections.Generic;
using Shintio.CodeBuilder.CSharp.CodeBlocks;
using Shintio.CodeBuilder.CSharp.Components;

namespace Shintio.CodeBuilder.CSharp.Factories;

public class MethodCodeBlockFactory
{
	public MethodCodeBlock Create(
		ClassCodeBlock forClass,
		AccessModifier accessModifier,
		TypeInfo returnType,
		string name,
		IEnumerable<ParameterCodeBlock> parameters
	)
	{
		return new MethodCodeBlock(forClass, accessModifier, returnType, name, parameters);
	}

	public MethodCodeBlock Create(
		ClassCodeBlock forClass,
		AccessModifier accessModifier,
		TypeInfo returnType,
		string name,
		params ParameterCodeBlock[] parameters
	)
	{
		return new MethodCodeBlock(forClass, accessModifier, returnType, name, parameters);
	}
}