using System.Globalization;

namespace Shintio.CodeGenerator.Utils
{
	public static class Formatter
	{
		public static string AsFloat(float value)
		{
			return $"{value.ToString(CultureInfo.InvariantCulture)}f";
		}
		
		public static string AsDouble(float value)
		{
			return $"{value.ToString(CultureInfo.InvariantCulture)}";
		}

		public static string AsFloat(string value)
		{
			return $"{value.Replace(",", ".")}f";
		}
		
		public static string AsTimeSpan(TimeSpan value)
		{
			var args = new List<string>
			{
				value is { Days: 0, Milliseconds: 0 } ? "" : $"{value.Days}",
				$"{value.Hours}",
				$"{value.Minutes}",
				$"{value.Seconds}",
				value.Milliseconds == 0 ? "" : $"{value.Milliseconds}",
			};

			return AsNewObject<TimeSpan>(args.ToArray());
		}
		
		public static string AsNewObject<T>(params string[] args)
		{
			return $"new {typeof(T).Name}({AsArgs(args)})";
		}
		
		public static string AsArgs(params string[] args)
		{
			return string.Join(", ", args.Where(a => a.Length != 0));
		}
	}
}