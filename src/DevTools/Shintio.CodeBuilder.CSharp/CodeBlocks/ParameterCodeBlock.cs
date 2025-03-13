using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class ParameterCodeBlock : CodeBlockBase
{
	public ParameterCodeBlock(string name)
	{
		Type = TypeInfo.Object;
		Name = name;
		DefaultValue = null;
	}
	
	public ParameterCodeBlock(TypeInfo type, string name)
	{
		Type = type;
		Name = name;
		DefaultValue = null;
	}

	public ParameterCodeBlock(TypeInfo type, string name, object? defaultValue)
	{
		Type = type;
		Name = name;
		DefaultValue = new LiteralCodeBlock(defaultValue);
	}

	// public ParameterCodeBlock(TypeInfo type, string name, ICodeBlock defaultValue)
	// {
	// 	Type = type;
	// 	Name = name;
	// 	DefaultValue = defaultValue;
	// }

	public TypeInfo Type { get; set; }
	public string Name { get; set; }
	public ICodeBlock? DefaultValue { get; set; }

	protected override string GetCodeInternal()
	{
		var defaultValue = DefaultValue == null ? "" : $" = {DefaultValue.GetCode(0)}";

		return $"{Type} {Name}{defaultValue}";
	}
}