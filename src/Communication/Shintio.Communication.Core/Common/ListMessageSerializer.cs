using System;
using System.Collections.Generic;
using System.Linq;

namespace Shintio.Communication.Core.Common
{
	public class ListMessageSerializer
	{
		private const int IntSize = sizeof(int);

		public virtual byte[] Serialize(List<object?> arguments)
		{
			var count = arguments.Count;

			var result = new List<byte>(count * IntSize);

			result.AddRange(BitConverter.GetBytes(count));

			for (var i = 0; i < count; i++)
			{
				WriteObject(result, arguments[i]);
			}

			return result.ToArray();
		}

		public virtual object?[] Deserialize(byte[] bytes)
		{
			return Deserialize(bytes.ToList());
		}

		public virtual object?[] Deserialize(List<byte> list)
		{
			var result = new List<object?>();

			var count = BitConverter.ToInt32(list.ToArray());
			list = list.Skip(IntSize).ToList();

			for (var i = 0; i < count; i++)
			{
				var size = ReadObject(list, out var obj);
				result.Add(obj);
				list = list.Skip(size).ToList();
			}

			return result.ToArray();
		}

		protected virtual void WriteObject(List<byte> buffer, object? obj)
		{
			if (obj == null)
			{
				buffer.AddRange(BitConverter.GetBytes((int)TypeCode.Empty));
				return;
			}

			var typeCode = Type.GetTypeCode(obj.GetType());
			buffer.AddRange(BitConverter.GetBytes((int)typeCode));

			switch (typeCode)
			{
				case TypeCode.Boolean:
					buffer.Add((bool)obj ? (byte)1 : (byte)0);
					break;
				case TypeCode.Byte:
					buffer.Add((byte)obj);
					break;
				case TypeCode.SByte:
					buffer.Add((byte)(sbyte)obj);
					break;
				case TypeCode.Char:
					buffer.AddRange(BitConverter.GetBytes((char)obj));
					break;
				case TypeCode.Int16:
					buffer.AddRange(BitConverter.GetBytes((short)obj));
					break;
				case TypeCode.UInt16:
					buffer.AddRange(BitConverter.GetBytes((ushort)obj));
					break;
				case TypeCode.Int32:
					buffer.AddRange(BitConverter.GetBytes((int)obj));
					break;
				case TypeCode.UInt32:
					buffer.AddRange(BitConverter.GetBytes((uint)obj));
					break;
				case TypeCode.Int64:
					buffer.AddRange(BitConverter.GetBytes((long)obj));
					break;
				case TypeCode.UInt64:
					buffer.AddRange(BitConverter.GetBytes((ulong)obj));
					break;
				case TypeCode.Single:
					buffer.AddRange(BitConverter.GetBytes((float)obj));
					break;
				case TypeCode.Double:
					buffer.AddRange(BitConverter.GetBytes((double)obj));
					break;
				case TypeCode.Decimal:
					var decimalBits = decimal.GetBits((decimal)obj);
					foreach (var bit in decimalBits)
					{
						buffer.AddRange(BitConverter.GetBytes(bit));
					}

					break;
				case TypeCode.DateTime:
					buffer.AddRange(BitConverter.GetBytes(((DateTime)obj).ToBinary()));
					break;
				case TypeCode.String:
					var stringBytes = System.Text.Encoding.UTF8.GetBytes((string)obj);
					buffer.AddRange(BitConverter.GetBytes(stringBytes.Length));
					buffer.AddRange(stringBytes);
					break;
				case TypeCode.Object:
				case TypeCode.DBNull:
				case TypeCode.Empty:
				default:
					break;
			}
		}

		protected virtual int ReadObject(List<byte> list, out object? result)
		{
			var type = (TypeCode)BitConverter.ToInt32(list.ToArray());
			list = list.Skip(IntSize).ToList();

			switch (type)
			{
				case TypeCode.Boolean:
					result = BitConverter.ToBoolean(list.ToArray());
					return sizeof(bool);
				case TypeCode.Byte:
					result = list[0];
					return sizeof(byte);
				case TypeCode.SByte:
					result = (sbyte)list[0];
					return sizeof(sbyte);
				case TypeCode.Char:
					result = BitConverter.ToChar(list.ToArray());
					return sizeof(char);
				case TypeCode.Int16:
					result = BitConverter.ToInt16(list.ToArray());
					return sizeof(short);
				case TypeCode.UInt16:
					result = BitConverter.ToUInt16(list.ToArray());
					return sizeof(ushort);
				case TypeCode.Int32:
					result = BitConverter.ToInt32(list.ToArray());
					return IntSize;
				case TypeCode.UInt32:
					result = BitConverter.ToUInt32(list.ToArray());
					return sizeof(uint);
				case TypeCode.Int64:
					result = BitConverter.ToInt64(list.ToArray());
					return sizeof(long);
				case TypeCode.UInt64:
					result = BitConverter.ToUInt64(list.ToArray());
					return sizeof(ulong);
				case TypeCode.Single:
					result = BitConverter.ToSingle(list.ToArray());
					return sizeof(float);
				case TypeCode.Double:
					result = BitConverter.ToDouble(list.ToArray());
					return sizeof(double);
				case TypeCode.Decimal:
					result = new decimal(new int[]
					{
						BitConverter.ToInt32(list.ToArray()),
						BitConverter.ToInt32(list.Skip(IntSize).ToArray()),
						BitConverter.ToInt32(list.Skip(2 * IntSize).ToArray()),
						BitConverter.ToInt32(list.Skip(3 * IntSize).ToArray()),
					});
					return 4 * IntSize;
				case TypeCode.DateTime:
					result = DateTime.FromBinary(BitConverter.ToInt64(list.ToArray()));
					return sizeof(long);
				case TypeCode.String:
					var length = BitConverter.ToInt32(list.ToArray());
					result = System.Text.Encoding.UTF8.GetString(list.Skip(IntSize).Take(length).ToArray());
					return IntSize + length;
				case TypeCode.Object:
				case TypeCode.DBNull:
				case TypeCode.Empty:
				default:
					result = null;
					return 0;
			}
		}
	}
}