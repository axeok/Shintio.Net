using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Shintio.CodeBuilder.CSharp.Components;

public class TypeInfo
{
	private const string NullableAttributeName = "System.Runtime.CompilerServices.NullableAttribute";
	private const string NullableContextAttributeName = "System.Runtime.CompilerServices.NullableContextAttribute";

	#region BuiltInTypes
	
	public static readonly TypeInfo Int = new("int");
	public static readonly TypeInfo UInt = new("uint");
	public static readonly TypeInfo Long = new("long");
	public static readonly TypeInfo ULong = new("ulong");
	public static readonly TypeInfo Short = new("short");
	public static readonly TypeInfo UShort = new("ushort");

	public static readonly TypeInfo Float = new("float");
	public static readonly TypeInfo Double = new("double");
	public static readonly TypeInfo Decimal = new("decimal");

	public static readonly TypeInfo Byte = new("byte");
	public static readonly TypeInfo SByte = new("sbyte");
	public static readonly TypeInfo Bool = new("bool");

	public static readonly TypeInfo Char = new("char");
	public static readonly TypeInfo String = new("string");

	public static readonly TypeInfo Object = new("object");

	#endregion

	public TypeInfo(string fullName)
	{
		fullName = FetchNullable(fullName.Trim());
		fullName = FetchGeneric(fullName);

		var split = fullName.Split('.');
		if (split.Length > 1)
		{
			Namespace = string.Join(".", split.Take(split.Length - 1));
			Name = split.Last();
		}
		else if (split.Length == 1)
		{
			Name = fullName;
		}
		else
		{
			Name = string.Empty;
		}
	}

	public TypeInfo(string? @namespace, string name)
	{
		Namespace = @namespace;
		Name = name;
	}

	// public string? Assembly { get; set; }
	public string? Namespace { get; set; }
	public string Name { get; set; }
	public bool IsNullable { get; set; } = false;

	public bool IsPrimitive { get; set; } = false;
	public bool IsString { get; set; } = false;
	public bool IsBool { get; set; } = false;
	public bool IsFloat { get; set; } = false;
	public bool IsEnum { get; set; } = false;

	public List<TypeInfo> GenericArguments { get; } = new();

	#region Converters

	public static implicit operator TypeInfo(string fullName) => new TypeInfo(fullName);

	public static implicit operator TypeInfo(Type type)
	{
		return FromType(type, null);
	}

	public static implicit operator TypeInfo(FieldInfo field)
	{
		return FromType(field.FieldType, (field.DeclaringType, field.CustomAttributes));
	}

	public static implicit operator TypeInfo(PropertyInfo property)
	{
		return FromType(property.PropertyType, (property.DeclaringType, property.CustomAttributes));
	}

	public static implicit operator TypeInfo(ParameterInfo parameter)
	{
		return FromType(parameter.ParameterType, (parameter.Member, parameter.CustomAttributes));
	}

	#endregion

	public override string ToString()
	{
		var name = string.IsNullOrWhiteSpace(Namespace) ? Name : $"{Namespace}.{Name}";

		name = GenericArguments.Count <= 0 ? name : $"{name}<{string.Join(", ", GenericArguments)}>";

		return IsNullable ? $"{name}?" : name;
	}

	public TypeInfo Nullable()
	{
		IsNullable = true;
		return this;
	}

	public TypeInfo NotNullable()
	{
		IsNullable = false;
		return this;
	}

	private string FetchNullable(string name)
	{
		if (name.EndsWith("?"))
		{
			IsNullable = true;
			return name.Substring(0, name.Length - 1);
		}

		var match = Regex.Match(name, @"^(global::)?(System\.)?Nullable\s*<(.+)>$");
		if (match.Success)
		{
			IsNullable = true;
			return match.Groups[3].Value.Trim();
		}

		return name;
	}

	private string FetchGeneric(string name)
	{
		if (!name.EndsWith(">"))
		{
			return name;
		}

		var split = name.Split('<');
		if (split.Length <= 1)
		{
			return name;
		}

		var arguments = string.Join("<", split.Skip(1));
		var genericArguments = SplitGenericArguments(arguments.Substring(0, arguments.Length - 1));

		GenericArguments.AddRange(genericArguments
			.Select(t => new TypeInfo(t.Trim())));

		return split[0];
	}

	private static bool? HasCorrectNullableAttribute(IEnumerable<CustomAttributeData> customAttributes)
	{
		var nullable =
			customAttributes.FirstOrDefault(x => x.AttributeType.FullName == NullableAttributeName);
		if (nullable is { ConstructorArguments: { Count: 1 } })
		{
			var attributeArgument = nullable.ConstructorArguments[0];
			if (attributeArgument.ArgumentType == typeof(byte[]))
			{
				var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
				if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
				{
					return (byte)args[0].Value! == 2;
				}
			}
			else if (attributeArgument.ArgumentType == typeof(byte))
			{
				return (byte)attributeArgument.Value! == 2;
			}
		}

		return null;
	}

	private static bool? HasCorrectNullableContextAttribute(MemberInfo declaringType)
	{
		for (var type = declaringType; type != null; type = type.DeclaringType)
		{
			var context = type.CustomAttributes
				.FirstOrDefault(x => x.AttributeType.FullName == NullableContextAttributeName);
			if (context is { ConstructorArguments: { Count: 1 } } &&
			    context.ConstructorArguments[0].ArgumentType == typeof(byte))
			{
				return (byte)context.ConstructorArguments[0].Value! == 2;
			}
		}

		return null;
	}

	private static TypeInfo FromType(
		Type type,
		(MemberInfo declaringType, IEnumerable<CustomAttributeData> customAttributes)? memberInfo
	)
	{
		var isNullable = false;

		if (type.IsValueType)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = type.GetGenericArguments()[0];
				isNullable = true;
			}
		}
		else
		{
			if (memberInfo != null)
			{
				var (declaringType, customAttributes) = memberInfo.Value;

				isNullable = HasCorrectNullableAttribute(customAttributes) ??
				             HasCorrectNullableContextAttribute(declaringType) ?? false;
			}
		}

		var result = new TypeInfo(
			type.Namespace,
			type.IsGenericType && type.Name.Contains('`')
				? type.Name.Substring(0, type.Name.IndexOf('`'))
				: type.Name
		)
		{
			IsNullable = isNullable,
		};

		if (type.IsGenericType)
		{
			result.GenericArguments.AddRange(type.GenericTypeArguments.Select(t => (TypeInfo)t));
		}

		return result;
	}

	private static string[] SplitGenericArguments(string arguments)
	{
		var result = new List<string>();
		var builder = new StringBuilder();
		var angleLevel = 0;
		var parenLevel = 0;

		foreach (var c in arguments)
		{
			if (c == '<')
			{
				angleLevel++;
				builder.Append(c);
			}
			else if (c == '>')
			{
				angleLevel--;
				builder.Append(c);
			}
			else if (c == '(')
			{
				parenLevel++;
				builder.Append(c);
			}
			else if (c == ')')
			{
				parenLevel--;
				builder.Append(c);
			}
			else if (c == ',' && angleLevel == 0 && parenLevel == 0)
			{
				result.Add(builder.ToString().Trim());
				builder.Clear();
			}
			else
			{
				builder.Append(c);
			}
		}

		if (builder.Length > 0)
		{
			result.Add(builder.ToString().Trim());
		}

		return result.ToArray();
	}
}