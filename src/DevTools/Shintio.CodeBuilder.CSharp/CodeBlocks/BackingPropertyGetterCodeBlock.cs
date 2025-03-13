using Shintio.CodeBuilder.CSharp.Interfaces;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class BackingPropertyGetterCodeBlock : CodeBlockBase, IPropertyGetter
{
	public BackingPropertyGetterCodeBlock(string fieldName)
	{
		FieldName = fieldName;
	}

	public string FieldName { get; set; }

	public static implicit operator BackingPropertyGetterCodeBlock(string fieldName)
	{
		return new BackingPropertyGetterCodeBlock(fieldName);
	}

	protected override string GetCodeInternal()
	{
		return $$"""
			get
			{
				return {{FieldName}};
			}
			""";
	}
}