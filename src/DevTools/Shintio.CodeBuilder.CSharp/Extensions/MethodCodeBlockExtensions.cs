using Shintio.CodeBuilder.CSharp.CodeBlocks;

namespace Shintio.CodeBuilder.CSharp.Extensions;

public static class MethodCodeBlockExtensions
{
	public static ClassCodeBlock WithBody(this MethodCodeBlock method, string body)
	{
		return method.WithBody(new RawCodeBlock(body));
	}
}