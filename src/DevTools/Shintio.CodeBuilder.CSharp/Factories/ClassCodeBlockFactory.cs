using Shintio.CodeBuilder.CSharp.CodeBlocks;

namespace Shintio.CodeBuilder.CSharp.Factories;

public class ClassCodeBlockFactory
{
	public ClassCodeBlock Create(string name)
	{
		return new ClassCodeBlock(name);
	}
}