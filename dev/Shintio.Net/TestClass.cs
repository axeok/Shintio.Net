using Shintio.Essentials.Converters;
using Shintio.Essentials.Interfaces;
using Shintio.Json.Attributes;

namespace Shintio.Net
{
	// [JsonConverter(typeof(TestClassConverter))]
	public class TestClass
	{
		[JsonConstructor]
		public TestClass(int id, string name)
		{
			Console.WriteLine("Constructor 1");
			Id = id;
			Name = name;
		}

		public TestClass()
		{
			Console.WriteLine("Constructor 2");
			Id = 321;
			Name = "asd";
		}

		public int Id { get; set; }
		public string Name { get; set; }
	}

	[JsonConverter(typeof(HasDiscriminatorJsonConverter<A>), false)]
	public abstract class A : IHasDiscriminator
	{
		public A()
		{
			Discriminator = GetType().Name;
		}

		public string Discriminator { get; }
	}

	public class B : A
	{
		public int X { get; set; } = 5;
	}

	public class C : A
	{
		[JsonIgnore]public int Y { get; set; } = 10;
	}
}