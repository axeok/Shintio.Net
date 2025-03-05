using System;
using Shintio.CodeBuilder.CSharp.CodeBlocks;

namespace Shintio.CodeBuilder.CSharp.Extensions;

public static class CommentCodeBlockExtensions
{
	public static string GetCode(this CommentCodeBlock? codeBlock)
	{
		return codeBlock == null ? "" : $"{codeBlock.GetCode(0)}{Environment.NewLine}";
	}
}