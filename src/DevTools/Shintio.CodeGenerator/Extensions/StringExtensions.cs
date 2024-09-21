using System.Text;

namespace Shintio.CodeGenerator.Extensions;

public static class StringExtensions
{
	public static string AddIndents(this string code, int count)
	{
		var indent = new string('\t', count);
		return string.Join(Environment.NewLine, code.Split(Environment.NewLine).Select(l => $"{indent}{l}"));
	}

	public static void AppendLines(this StringBuilder builder, IEnumerable<string> lines)
	{
		builder.Append(string.Join(Environment.NewLine, lines));
	}
}