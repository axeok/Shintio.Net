using Shintio.CodeBuilder.CSharp.Components;
using TypeInfo = Shintio.CodeBuilder.CSharp.Components.TypeInfo;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class BackingPropertyCodeBlock : PropertyCodeBlock
{
	public BackingPropertyCodeBlock(
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		string fieldName,
		bool hasSetter,
		AccessModifier? setterAccessModifier = null
	)
		: base(
			accessModifier,
			type,
			name,
			new BackingPropertyGetterCodeBlock(fieldName),
			hasSetter ? new BackingPropertySetterCodeBlock(fieldName) : null,
			setterAccessModifier
		)
	{
	}
}