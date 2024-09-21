using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shintio.Database.Extensions;

public static class ModelConfigurationBuilderExtensions
{
	public static void AddDefaultConverter<TProperty, TConverter>(this ModelConfigurationBuilder configurationBuilder)
		where TConverter : ValueConverter
	{
		configurationBuilder
			.Properties<TProperty>()
			.HaveConversion<TConverter>();
	}
}