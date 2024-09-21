using System.Collections.Generic;
using Shintio.Essentials.Common;
using Shintio.Json.Attributes;
using Shintio.Localization.Enums;
using Shintio.Localization.Interfaces;

namespace Shintio.Localization.ValueObjects
{
	public class StringContainer : ValueObject, IHasTranslation
	{
		public static readonly StringContainer Empty = new StringContainer();
		
		public static implicit operator StringContainer(string value) => new StringContainer(value);
		
		public StringContainer() : this(string.Empty)
		{
		}
		
		[JsonConstructor]
		public StringContainer(string value)
		{
			Value = value;
		}
		
		public string Discriminator => GetType().Name;
		
		public string Value { get; }
		
		public string Get(II18N i18N, Language language)
		{
			return Value;
		}
		
		public string GetRaw()
		{
			return Value;
		}
		
		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Value;
		}
	}
}