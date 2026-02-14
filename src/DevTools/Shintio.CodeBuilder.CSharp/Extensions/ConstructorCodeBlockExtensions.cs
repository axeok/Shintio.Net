using Shintio.CodeBuilder.CSharp.CodeBlocks;

namespace Shintio.CodeBuilder.CSharp.Extensions;

public static class ConstructorCodeBlockExtensions
{
	public static ClassCodeBlock WithBody(this ConstructorCodeBlock constructor, string body)
	{
		return constructor.WithBody(new RawCodeBlock(body));
	}
}