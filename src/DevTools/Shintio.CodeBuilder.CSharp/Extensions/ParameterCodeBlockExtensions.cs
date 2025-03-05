using System.Collections.Generic;
using System.Linq;
using Shintio.CodeBuilder.CSharp.CodeBlocks;

namespace Shintio.CodeBuilder.CSharp.Extensions;

public static class ParameterCodeBlockExtensions
{
	public static string Join(this IEnumerable<ParameterCodeBlock> parameters, string separator = ", ")
	{
		return string.Join(separator, parameters.Select(p => p.GetCode(0)));
	}

	public static string ToInvokeArgs(this IEnumerable<ParameterCodeBlock> parameters, string separator = ", ")
	{
		return string.Join(separator, parameters.GetNames());
	}

	public static IEnumerable<string> GetNames(this IEnumerable<ParameterCodeBlock> parameters)
	{
		return parameters.Select(p => p.Name);
	}
}