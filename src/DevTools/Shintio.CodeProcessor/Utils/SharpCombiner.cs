using Shintio.CodeProcessor.Models;

namespace Shintio.CodeProcessor.Utils;

public class SharpCombiner
{
	public static string CombineCode(IEnumerable<string> codes)
	{
		return new Combiner().Combine(CombineOptions.None, codes.Select(x => new SharpFile("", x)).ToArray());
	}
}