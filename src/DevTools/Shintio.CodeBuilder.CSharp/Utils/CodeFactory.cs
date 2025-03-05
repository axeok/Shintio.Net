using Shintio.CodeBuilder.CSharp.Factories;

namespace Shintio.CodeBuilder.CSharp.Utils;

public static class CodeFactory
{
	public static FieldCodeBlockFactory Field = new();
	public static PropertyCodeBlockFactory Property = new();
	public static ConstructorCodeBlockFactory Constructor = new();
	public static MethodCodeBlockFactory Method = new();
}