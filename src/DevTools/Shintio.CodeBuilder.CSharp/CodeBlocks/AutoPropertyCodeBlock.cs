using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Interfaces;
using TypeInfo = Shintio.CodeBuilder.CSharp.Components.TypeInfo;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class AutoPropertyCodeBlock : PropertyCodeBlock
{
	public AutoPropertyCodeBlock(
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		bool hasSetter,
		AccessModifier? setterAccessModifier = null
	)
		: base(
			accessModifier,
			type,
			name,
			new AutoPropertyGetterCodeBlock(),
			hasSetter ? new AutoPropertySetterCodeBlock() : null,
			setterAccessModifier
		)
	{
	}

	public AutoPropertyCodeBlock(
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		bool hasSetter,
		object? defaultValue,
		AccessModifier? setterAccessModifier = null
	) : this(accessModifier, type, name, hasSetter, setterAccessModifier)
	{
		DefaultValue = new LiteralCodeBlock(defaultValue);
	}

	protected override int BodyIndent => 0;
	protected override string BodyPrefix => string.Empty;

	public ICodeBlock? DefaultValue { get; set; }

	protected override string WrapBody(string body)
	{
		return $" {body} ";
	}

	protected override string GetPostfix()
	{
		return DefaultValue == null ? "" : $" = {DefaultValue.GetCode(0)};";
	}
}