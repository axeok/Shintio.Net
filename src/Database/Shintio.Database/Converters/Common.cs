using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Color = System.Drawing.Color;

namespace Shintio.Database.Converters;

public class ColorToInt32Converter : ValueConverter<Color, int>
{
	public ColorToInt32Converter()
		: base(v => v.ToArgb(),
			v => Color.FromArgb(v)
		)
	{
	}
}

public class DateTimeUtcConverter : ValueConverter<DateTime, DateTime>
{
	public DateTimeUtcConverter()
		: base(v => v,
			v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
		)
	{
	}
}

