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

		[JsonIgnore] public int Id { get; set; }
		public string Name { get; set; }
	}
}