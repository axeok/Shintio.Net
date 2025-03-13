using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class AutoPropertySetterCodeBlock : CodeBlockBase, IPropertySetter
{
	public string Modifier { get; set; } = "";

	protected override string GetCodeInternal()
	{
		return string.IsNullOrEmpty(Modifier) ? " set;" : $" {Modifier} set;";
	}

	public string GetCode(int indent, string modifier)
	{
		Modifier = modifier;

		return GetCode(indent);
	}
}