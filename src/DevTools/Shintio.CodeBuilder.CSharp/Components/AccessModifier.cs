using System.Reflection;

namespace Shintio.CodeBuilder.CSharp.Components;

public class AccessModifier
{
	public static readonly AccessModifier Public = new("public");
	public static readonly AccessModifier Private = new("private");
	public static readonly AccessModifier Protected = new("protected");
	public static readonly AccessModifier Internal = new("internal");
	public static readonly AccessModifier ProtectedInternal = new("protected internal");
	public static readonly AccessModifier PrivateProtected = new("private protected");
	public static readonly AccessModifier File = new("file");

	private readonly string _value;

	private AccessModifier(string value)
	{
		_value = value;
	}

	public static implicit operator AccessModifier(string? value) => value switch
	{
		"public" => Public,
		"private" => Private,
		"protected" => Protected,
		"internal" => Internal,
		"protected internal" => ProtectedInternal,
		"private protected" => PrivateProtected,
		"file" => File,

		_ => Public,
	};

	public static implicit operator string(AccessModifier value) => value.ToString();

	public override string ToString() => _value;

	public static AccessModifier From(PropertyInfo propertyInfo)
	{
		return From(propertyInfo.GetMethod == null ? propertyInfo.SetMethod : propertyInfo.GetMethod);
	}

	public static AccessModifier From(MethodInfo methodInfo)
	{
		if (methodInfo.IsPrivate)
			return Private;
		if (methodInfo.IsFamily)
			return Protected;
		if (methodInfo.IsFamilyOrAssembly)
			return ProtectedInternal;
		if (methodInfo.IsAssembly)
			return Internal;
		if (methodInfo.IsPublic)
			return Public;

		return Public;
	}

	public static AccessModifier From(FieldInfo fieldInfo)
	{
		if (fieldInfo.IsPrivate)
			return Private;
		if (fieldInfo.IsFamily)
			return Protected;
		if (fieldInfo.IsFamilyOrAssembly)
			return ProtectedInternal;
		if (fieldInfo.IsAssembly)
			return Internal;
		if (fieldInfo.IsPublic)
			return Public;

		return Public;
	}
}