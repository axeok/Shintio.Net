using System;
using Shintio.CodeBuilder.CSharp.Components;

namespace Shintio.CodeBuilder.CSharp.CodeBlocks;

public class LiteralCodeBlock : CodeBlockBase
{
	public LiteralCodeBlock(object? value)
	{
		Value = value;
	}

	public object? Value { get; set; }

	protected override string GetCodeInternal()
	{
		return FormatPropertyValue(Value);
	}

	public static string FormatPropertyValue(object? value)
	{
		if (value == null)
		{
			return "null";
		}

		var type = value.GetType();

		if (type == typeof(string))
		{
			return $"\"{value}\"";
		}

		if (type == typeof(float))
		{
			return $"{value.ToString().Replace(",", ".")}f";
		}

		if (type == typeof(bool))
		{
			return Convert.ToBoolean(value).ToString().ToLowerInvariant();
		}

		if (type.IsPrimitive)
		{
			return value.ToString()!;
		}

		if (type.IsEnum)
		{
			return $"{(TypeInfo)type}.{value}";
		}

		return "#UNKNOWN#";
	}
}