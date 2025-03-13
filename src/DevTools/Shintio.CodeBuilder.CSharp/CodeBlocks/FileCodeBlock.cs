namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class FileCodeBlock : CodeBlockBase
{
	public FileCodeBlock(ClassCodeBlock @class, NamespaceCodeBlock @namespace)
	{
		Class = @class;
		Namespace = @namespace;
		Namespace.Classes.Add(@class);
	}

	public ClassCodeBlock Class { get; set; }
	public NamespaceCodeBlock Namespace { get; set; }

	protected override string GetCodeInternal()
	{
		return Namespace.GetCode(0);
	}
}