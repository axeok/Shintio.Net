using Shintio.CodeGenerator.Enums;
using Shintio.CodeGenerator.Interfaces;
using Shintio.CodeGenerator.Models;

namespace Shintio.CodeGenerator.Common;

public abstract class Template : ITemplate
{
	public abstract ProjectInfo ProjectInfo { get; }
	public abstract CodeLanguage CodeLanguage { get; }
	public abstract Task<IEnumerable<KeyValuePair<string, string>>> Run();
	
	public static HashSet<Type> NumericTypes = new()
	{
		typeof(decimal), typeof(byte), typeof(sbyte),
		typeof(short), typeof(ushort), typeof(int), 
		typeof(double), typeof(float), typeof(uint), typeof(long), typeof(ulong)
	};
}