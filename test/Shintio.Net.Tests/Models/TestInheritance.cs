using Shintio.Essentials.Converters;
using Shintio.Essentials.Interfaces;
using Shintio.Json.Attributes;

namespace Shintio.Net.Tests.Models;

[JsonConverter(typeof(HasDiscriminatorJsonConverter<ITestChild>), false)]
public interface ITestChild : IHasDiscriminator
{
}

[JsonConverter(typeof(HasDiscriminatorJsonConverter<TestParent>), false)]
public abstract class TestParent : IHasDiscriminator
{
	public TestParent()
	{
		Discriminator = GetType().Name;
	}

	public string Discriminator { get; }
}

public class TestChild1 : TestParent, ITestChild
{
	public int X { get; set; } = 1;
}

public class TestChild2 : TestParent, ITestChild
{
	public int Y { get; set; } = 2;
}