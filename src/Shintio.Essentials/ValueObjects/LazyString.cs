using System.Collections.Generic;
using System.Linq;
using Shintio.Essentials.Common;
using Shintio.Json.Attributes;

namespace Shintio.Essentials.ValueObjects
{
	public class LazyString : ValueObject
	{
		[JsonConstructor]
		public LazyString(string format, params object[] args)
		{
			Format = format;
			Args = args;
		}

		public LazyString(string format, IEnumerable<object> args)
		{
			Format = format;
			Args = args.ToArray();
		}

		public string Format { get; }
		public object[] Args { get; }

		[JsonIgnore] public string Value => string.Format(Format, Args);

		public static implicit operator LazyString(string str) => new LazyString(str);
		public static implicit operator string(LazyString str) => str.Value;
		public override string ToString() => Value;

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Format;

			yield return Args.Length;
			foreach (var arg in Args)
			{
				yield return arg;
			}
		}
	}
}