using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class RawCodeBlock : CodeBlockBase, IPropertyGetter, IPropertySetter
{
	public RawCodeBlock(string value)
	{
		Value = value;
	}

	public string Value { get; set; }

	protected override string GetCodeInternal()
	{
		return Value;
	}

	public string GetCode(int indent, string modifier)
	{
		return GetCode(indent);
	}
}