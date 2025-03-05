using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class AutoPropertyGetterCodeBlock : CodeBlockBase, IPropertyGetter
{
	protected override string BuildInternal()
	{
		return "get;";
	}
}