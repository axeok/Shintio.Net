using System.Collections.Generic;
using Shintio.CodeBuilder.CSharp.CodeBlocks;
using Shintio.CodeBuilder.CSharp.Components;

namespace Shintio.CodeBuilder.CSharp.Factories;

public class ConstructorCodeBlockFactory
{
	public ConstructorCodeBlock Create(
		ClassCodeBlock forClass,
		AccessModifier accessModifier,
		IEnumerable<ParameterCodeBlock> parameters
	)
	{
		return new ConstructorCodeBlock(forClass, accessModifier, parameters);
	}

	public ConstructorCodeBlock Create(
		ClassCodeBlock forClass,
		AccessModifier accessModifier,
		params ParameterCodeBlock[] parameters
	)
	{
		return new ConstructorCodeBlock(forClass, accessModifier, parameters);
	}
}