using System;
using System.Collections.Generic;
using Shintio.Essentials.Extensions;

namespace Shintio.Communication.Core.Common
{
	public class MessageSerializer
	{
		private const int IntSize = sizeof(int);

		public virtual byte[] Serialize(object?[] arguments)
		{
			var count = arguments.Length;

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
			var memory = new Memory<byte>(bytes);
			return Deserialize(memory);
		}

		public virtual object?[] Deserialize(Memory<byte> memory)
		{
			var result = new List<object?>();

			var count = BitConverter.ToInt32(memory.Span);
			memory = memory.Slice(IntSize);

			for (var i = 0; i < count; i++)
			{
				var size = ReadObject(ref memory, out var obj);
				result.Add(obj);
				memory = memory.Slice(size);
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
					buffer.AddRange(BitConverter.GetBytes(obj.ToIntPtr().ToInt64()));
					break;
				case TypeCode.DBNull:
				case TypeCode.Empty:
				default:
					break;
			}
		}

		protected virtual int ReadObject(ref Memory<byte> memory, out object? result)
		{
			var type = (TypeCode)BitConverter.ToInt32(memory.Span);
			memory = memory.Slice(IntSize);

			switch (type)
			{
				case TypeCode.Boolean:
					result = BitConverter.ToBoolean(memory.Span);
					return sizeof(bool);
				case TypeCode.Byte:
					result = memory.Span[0];
					return sizeof(byte);
				case TypeCode.SByte:
					result = (sbyte)memory.Span[0];
					return sizeof(sbyte);
				case TypeCode.Char:
					result = BitConverter.ToChar(memory.Span);
					return sizeof(char);
				case TypeCode.Int16:
					result = BitConverter.ToInt16(memory.Span);
					return sizeof(short);
				case TypeCode.UInt16:
					result = BitConverter.ToUInt16(memory.Span);
					return sizeof(ushort);
				case TypeCode.Int32:
					result = BitConverter.ToInt32(memory.Span);
					return IntSize;
				case TypeCode.UInt32:
					result = BitConverter.ToUInt32(memory.Span);
					return sizeof(uint);
				case TypeCode.Int64:
					result = BitConverter.ToInt64(memory.Span);
					return sizeof(long);
				case TypeCode.UInt64:
					result = BitConverter.ToUInt64(memory.Span);
					return sizeof(ulong);
				case TypeCode.Single:
					result = BitConverter.ToSingle(memory.Span);
					return sizeof(float);
				case TypeCode.Double:
					result = BitConverter.ToDouble(memory.Span);
					return sizeof(double);
				case TypeCode.Decimal:
					result = new decimal(new int[]
					{
						BitConverter.ToInt32(memory.Span),
						BitConverter.ToInt32(memory.Span.Slice(IntSize)),
						BitConverter.ToInt32(memory.Span.Slice(2 * IntSize)),
						BitConverter.ToInt32(memory.Span.Slice(3 * IntSize))
					});
					return 4 * IntSize;
				case TypeCode.DateTime:
					result = DateTime.FromBinary(BitConverter.ToInt64(memory.Span));
					return sizeof(long);
				case TypeCode.String:
					var length = BitConverter.ToInt32(memory.Span);
					result = System.Text.Encoding.UTF8.GetString(memory.Span.Slice(IntSize, length));
					return IntSize + length;
				case TypeCode.Object:
					var intPtr = new IntPtr(BitConverter.ToInt64(memory.Span));
					result = intPtr.ToObject();
					return sizeof(long);
				case TypeCode.DBNull:
				case TypeCode.Empty:
				default:
					result = null;
					return 0;
			}
		}
	}
}