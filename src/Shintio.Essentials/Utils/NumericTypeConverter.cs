using System;

namespace Shintio.Essentials.Utils
{
	public static class NumericTypeConverter
	{
		public static bool TryConvert(ref object arg, Type expectedType, bool allowOverflow = true)
		{
			var originalType = arg.GetType();

			// Early return if types match
			if (originalType == expectedType)
			{
				return true;
			}

			if (originalType.IsEnum)
			{
				var underlyingType = Enum.GetUnderlyingType(originalType);
				var underlyingValue = Convert.ChangeType(arg, underlyingType);
				var result = TryConvert(ref underlyingValue, expectedType, allowOverflow);
				if (result)
				{
					arg = underlyingValue;
				}

				return result;
			}

			if (expectedType.IsEnum)
			{
				var underlyingType = Enum.GetUnderlyingType(expectedType);
				return TryConvert(ref arg, underlyingType, allowOverflow);
			}
			
			if (!originalType.IsPrimitive || !expectedType.IsPrimitive)
			{
				return false;
			}

			try
			{
				return Type.GetTypeCode(expectedType) switch
				{
					TypeCode.Int32 => TryConvertToInt(ref arg, allowOverflow),
					TypeCode.UInt32 => TryConvertToUint(ref arg, allowOverflow),
					TypeCode.Int64 => TryConvertToLong(ref arg, allowOverflow),
					TypeCode.Single => TryConvertToFloat(ref arg, allowOverflow),
					TypeCode.Double => TryConvertToDouble(ref arg, allowOverflow),
					TypeCode.Byte => TryConvertToByte(ref arg, allowOverflow),
					TypeCode.Boolean => TryConvertToBool(ref arg, allowOverflow),
				};
			}
			catch
			{
				return false;
			}
		}

		private static bool TryConvertToInt(ref object arg, bool allowOverflow)
		{
			switch (arg)
			{
				case long longValue
					when allowOverflow || (longValue >= int.MinValue && longValue <= int.MaxValue):
					arg = (int)longValue;
					return true;
				case uint uintValue when allowOverflow || uintValue <= int.MaxValue:
					arg = (int)uintValue;
					return true;
				case byte byteValue:
					arg = (int)byteValue;
					return true;
				case bool boolValue:
					arg = boolValue ? 1 : 0;
					return true;
				default:
					return false;
			}
		}

		private static bool TryConvertToUint(ref object arg, bool allowOverflow)
		{
			switch (arg)
			{
				case int intValue when allowOverflow || intValue >= 0:
					arg = (uint)intValue;
					return true;
				case long longValue when allowOverflow || (longValue >= 0 && longValue <= uint.MaxValue):
					arg = (uint)longValue;
					return true;
				case byte byteValue:
					arg = (uint)byteValue;
					return true;
				default:
					return false;
			}
		}

		private static bool TryConvertToLong(ref object arg, bool allowOverflow)
		{
			switch (arg)
			{
				case int intValue:
					arg = (long)intValue;
					return true;
				case uint uintValue:
					arg = (long)uintValue;
					return true;
				case byte byteValue:
					arg = (long)byteValue;
					return true;
				default:
					return false;
			}
		}

		private static bool TryConvertToFloat(ref object arg, bool allowOverflow)
		{
			switch (arg)
			{
				case double doubleValue:
					arg = (float)doubleValue;
					return true;
				case int intValue:
					arg = (float)intValue;
					return true;
				default:
					return false;
			}
		}

		private static bool TryConvertToDouble(ref object arg, bool allowOverflow)
		{
			switch (arg)
			{
				case float floatValue:
					arg = (double)floatValue;
					return true;
				case int intValue:
					arg = (double)intValue;
					return true;
				default:
					return false;
			}
		}

		private static bool TryConvertToByte(ref object arg, bool allowOverflow)
		{
			switch (arg)
			{
				case int intValue
					when allowOverflow || (intValue >= byte.MinValue && intValue <= byte.MaxValue):
					arg = (byte)intValue;
					return true;
				case uint uintValue when allowOverflow || uintValue <= byte.MaxValue:
					arg = (byte)uintValue;
					return true;
				default:
					return false;
			}
		}

		private static bool TryConvertToBool(ref object arg, bool allowOverflow)
		{
			switch (arg)
			{
				case int intValue:
					arg = intValue != 0;
					return true;
				case uint uintValue:
					arg = uintValue != 0;
					return true;
				default:
					return false;
			}
		}
	}
}