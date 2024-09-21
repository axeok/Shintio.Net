using Shintio.Json.Attributes;

namespace Shintio.Net.Tests.Models;

public class TestData
{
	public TestData(int number, string row)
	{
		Number = number;
		Row = row;
	}

	public int Number { get; set; }
	public string Row { get; set; }
}

public class TestDataWithIgnore
{
	public TestDataWithIgnore()
	{
	}

	[JsonIgnore] public int Number { get; set; } = 3;
	public string Row { get; set; } = "";
}