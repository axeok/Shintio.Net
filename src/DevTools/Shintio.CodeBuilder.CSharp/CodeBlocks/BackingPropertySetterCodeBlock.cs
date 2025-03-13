using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class BackingPropertySetterCodeBlock : CodeBlockBase, IPropertySetter
{
	public BackingPropertySetterCodeBlock(string fieldName)
	{
		FieldName = fieldName;
	}

	public string FieldName { get; set; }
	public string Modifier { get; set; } = "";

	public static implicit operator BackingPropertySetterCodeBlock(string fieldName)
	{
		return new BackingPropertySetterCodeBlock(fieldName);
	}

	protected override string GetCodeInternal()
	{
		return $$"""
			
			{{(string.IsNullOrEmpty(Modifier) ? "" : $"{Modifier} ")}}set
			{
				{{FieldName}} = value;
			}
			""";
	}

	public string GetCode(int indent, string modifier)
	{
		Modifier = modifier;

		return GetCode(indent);
	}
}