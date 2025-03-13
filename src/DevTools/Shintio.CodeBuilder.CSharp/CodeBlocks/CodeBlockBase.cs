using System;
using System.Text;
using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public abstract class CodeBlockBase : ICodeBlock
{
	public string GetCode(int indent)
	{
		var result = GetCodeInternal();

		if (PostProcessor != null)
		{
			result = PostProcessor(result);
		}

		return AddIndents($"{Prefix}{result}{Postfix}", indent);
	}

	public string? Prefix { get; set; }
	public string? Postfix { get; set; }
	public Func<string, string>? PostProcessor { get; set; }

	protected abstract string GetCodeInternal();

	private static string AddIndents(string code, int count, bool skipFirstLine = false)
	{
		if (count <= 0)
		{
			return code;
		}

		var indent = new string('\t', count);

		var builder = new StringBuilder();

		var lines = code.Split([Environment.NewLine], StringSplitOptions.None);
		if (lines.Length > 0)
		{
			if (skipFirstLine)
			{
				builder.Append(lines[0]);
			}
			else
			{
				builder.Append(indent).Append(lines[0]);
			}
		}

		for (var i = 1; i < lines.Length; i++)
		{
			builder.AppendLine()
				.Append(indent)
				.Append(lines[i]);
		}

		return builder.ToString();
	}
}