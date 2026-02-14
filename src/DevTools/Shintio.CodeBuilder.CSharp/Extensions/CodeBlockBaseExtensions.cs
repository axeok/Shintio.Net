using System;
using Shintio.CodeBuilder.CSharp.CodeBlocks;

namespace Shintio.CodeBuilder.CSharp.Extensions;

public static class CodeBlockBaseExtensions
{
	public static T PostProcessing<T>(
		this T codeBlock,
		string? prefix = null,
		string? postfix = null,
		Func<string, string>? postProcessor = null
	) where T : CodeBlockBase
	{
		codeBlock.Prefix = prefix;
		codeBlock.Postfix = postfix;
		codeBlock.PostProcessor = postProcessor;

		return codeBlock;
	}
}