using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Extensions;
using Shintio.CodeBuilder.CSharp.Interfaces;
using TypeInfo = Shintio.CodeBuilder.CSharp.Components.TypeInfo;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class FieldCodeBlock : CodeBlockBase
{
	public FieldCodeBlock(AccessModifier accessModifier, TypeInfo type, string name)
	{
		AccessModifier = accessModifier;
		Type = type;
		Name = name;
	}

	public FieldCodeBlock(AccessModifier accessModifier, TypeInfo type, string name, object? defaultValue)
	{
		AccessModifier = accessModifier;
		Type = type;
		Name = name;
		DefaultValue = new LiteralCodeBlock(defaultValue);
	}

	public AccessModifier AccessModifier { get; set; }
	public TypeInfo Type { get; set; }
	public string Name { get; set; }

	public bool IsStatic { get; set; } = false;
	public bool IsReadonly { get; set; } = false;

	public ICodeBlock? DefaultValue { get; set; }
	public CommentCodeBlock? Comment { get; set; } = null;

	public FieldCodeBlock Static()
	{
		IsStatic = true;
		return this;
	}

	public FieldCodeBlock Readonly()
	{
		IsReadonly = true;
		return this;
	}

	public FieldCodeBlock SetComment(CommentCodeBlock comment)
	{
		Comment = comment;
		return this;
	}

	protected override string GetCodeInternal()
	{
		var @static = IsStatic ? "static " : string.Empty;
		var @readonly = IsReadonly ? "readonly " : string.Empty;
		
		var defaultValue = DefaultValue == null ? "" : $" = {DefaultValue.GetCode(0)}";

		return $"{Comment.GetCode()}{AccessModifier} {@static}{@readonly}{Type} {Name}{defaultValue};";
	}
}