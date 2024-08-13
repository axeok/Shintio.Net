using System;
using Shintio.Json.Interfaces;

namespace Shintio.Json.Common
{
	public abstract class JsonConverter<T>
	{
#if NETCOREAPP3_0_OR_GREATER
        public abstract void Write(IJsonWriter writer, T? value);
#else
		public abstract void Write(IJsonWriter writer, T value);
#endif

		public abstract T Read(IJsonReader reader, Type type);
	}
}