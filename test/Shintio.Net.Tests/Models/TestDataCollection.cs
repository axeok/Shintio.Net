using Shintio.Essentials.Common;
using Shintio.Essentials.Converters;
using Shintio.Json.Attributes;

namespace Shintio.Net.Tests.Models;

[JsonConverter(typeof(DataCollectionJsonConverter<TestDataCollection>))]
public class TestDataCollection : DataCollection
{
	public static readonly TestDataCollection Value1 = new TestDataCollection(0);
	public static readonly TestDataCollection Value2 = new TestDataCollection(5);
	public static readonly TestDataCollection Value3 = new TestDataCollection(999);

	private TestDataCollection(int x)
	{
		X = x;
	}

	public int X { get; }
}