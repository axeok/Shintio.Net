using System;
using System.Text;
using Shintio.CodeBuilder.CSharp.Components;
using Shintio.CodeBuilder.CSharp.Extensions;
using Shintio.CodeBuilder.CSharp.Interfaces;
using TypeInfo = Shintio.CodeBuilder.CSharp.Components.TypeInfo;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class PropertyCodeBlock : CodeBlockBase
{
	public PropertyCodeBlock(
		AccessModifier accessModifier,
		TypeInfo type,
		string name,
		IPropertyGetter? getter = null,
		IPropertySetter? setter = null,
		AccessModifier? setterAccessModifier = null
	)
	{
		AccessModifier = accessModifier;
		Type = type;
		Name = name;

		Getter = getter;
		Setter = setter;
		SetterAccessModifier = setterAccessModifier ?? accessModifier;
	}

	public AccessModifier AccessModifier { get; set; }
	public AccessModifier SetterAccessModifier { get; set; }
	public TypeInfo Type { get; set; }
	public string Name { get; set; }

	public bool IsPartial { get; set; } = false;
	public bool IsStatic { get; set; } = false;
	public bool IsAbstract { get; set; } = false;
	public bool IsVirtual { get; set; } = false;
	public bool IsOverride { get; set; } = false;

	public CommentCodeBlock? Comment { get; set; } = null;

	public IPropertyGetter? Getter { get; set; }
	public IPropertySetter? Setter { get; set; }

	protected virtual int BodyIndent => 1;
	protected virtual string BodyPrefix => Environment.NewLine;

	public PropertyCodeBlock Partial()
	{
		IsPartial = true;
		return this;
	}

	public PropertyCodeBlock Static()
	{
		IsStatic = true;
		return this;
	}

	public PropertyCodeBlock Abstract()
	{
		IsAbstract = true;
		return this;
	}

	public PropertyCodeBlock Virtual()
	{
		IsVirtual = true;
		return this;
	}

	public PropertyCodeBlock Override()
	{
		IsOverride = true;
		return this;
	}

	public PropertyCodeBlock SetComment(CommentCodeBlock comment)
	{
		Comment = comment;
		return this;
	}

	protected override string BuildInternal()
	{
		var @abstract = IsAbstract ? "abstract " : string.Empty;
		var @static = IsStatic ? "static " : string.Empty;
		var @partial = IsPartial ? "partial " : string.Empty;
		var @virtual = IsVirtual ? "virtual " : string.Empty;
		var @override = IsOverride ? "override " : string.Empty;

		return $"{Comment.GetCode()}{AccessModifier} {@abstract}{@static}{@virtual}{@override}{@partial}{Type} {Name} {BodyPrefix}{{{GetBody()}}}{GetPostfix()}";
	}

	protected virtual string GetBody()
	{
		var body = new StringBuilder();

		if (Getter != null)
		{
			body.Append(Getter.GetCode(BodyIndent));
		}

		if (Setter != null)
		{
			body.Append(Setter.GetCode(BodyIndent, SetterAccessModifier != AccessModifier ? SetterAccessModifier : ""));
		}

		return WrapBody(body.ToString());
	}

	protected virtual string WrapBody(string body)
	{
		return $"{Environment.NewLine}{body}{Environment.NewLine}";
	}

	protected virtual string GetPostfix()
	{
		return string.Empty;
	}
}