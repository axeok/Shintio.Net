using System;
using System.Linq;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class CommentCodeBlock : CodeBlockBase
{
	public CommentCodeBlock(string text)
	{
		Value = string.Join(
			Environment.NewLine,
			text.Split([Environment.NewLine], StringSplitOptions.None)
				.Select(l => $"// {l}")
		);
	}

	public string Value { get; set; }

	protected override string GetCodeInternal()
	{
		return Value;
	}

}