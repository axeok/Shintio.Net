using System;
using Shintio.Json.Interfaces;

namespace Shintio.Json.Common
{
	public abstract class JsonConverter<T> : IJsonConverter
	{
		public IJson Converter { get; set; } = null!;
		
		public virtual bool CanRead { get; } = true;
		public virtual bool CanWrite { get; } = true;

#if NETCOREAPP3_0_OR_GREATER
        public abstract void Write(IJsonWriter writer, T? value);
#else
		public abstract void Write(IJsonWriter writer, T value);
#endif

#if NETCOREAPP3_0_OR_GREATER
		public abstract T? Read(IJsonReader reader, Type type);
#else
		public abstract T Read(IJsonReader reader, Type type);
#endif
	}
}