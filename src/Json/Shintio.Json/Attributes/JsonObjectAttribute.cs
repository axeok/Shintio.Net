using System;
using Shintio.Json.Enums;

namespace Shintio.Json.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class JsonObjectAttribute : Attribute
	{
		public JsonObjectAttribute(MemberSerialization memberSerialization)
		{
			MemberSerialization = memberSerialization;
		}
		
		public MemberSerialization MemberSerialization { get; }
	}
}