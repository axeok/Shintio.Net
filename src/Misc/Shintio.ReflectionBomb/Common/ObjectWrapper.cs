using System;

namespace Shintio.ReflectionBomb.Common
{
	public class ObjectWrapper
	{
		public ObjectWrapper(TypeWrapper type, object value)
		{
			Type = type;
			Value = value;
		}

		public ObjectWrapper(Type type, object value)
		{
			Type = new TypeWrapper(type);
			Value = value;
		}

		public ObjectWrapper(object value)
		{
			Type = new TypeWrapper(value.GetType());
			Value = value;
		}

		public TypeWrapper Type { get; }
		public object Value { get; }
	}
}