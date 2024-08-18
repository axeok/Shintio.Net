using Shintio.Json.Interfaces;
using Shintio.Json.Nodes;

namespace Shintio.Net.Tests.Json;

public abstract class JsonTestBase<TJson> where TJson : IJson, new()
{
	private readonly TJson _json;

	public JsonTestBase()
	{
		_json = new TJson();
	}

	[Fact]
	public void SerializeSimpleClass()
	{
		Assert.Equal(
			"{\"Number\":5,\"Row\":\"qwe\"}",
			_json.Serialize(new TestData(5, "qwe"))
		);
	}

	[Fact]
	public void DeserializeSimpleClass()
	{
		var result = _json.Deserialize<TestData>("{\"Number\":5,\"Row\":\"qwe\"}");

		Assert.NotNull(result);
		Assert.Equal(5, result.Number);
		Assert.Equal("qwe", result.Row);
	}

	[Fact]
	public void SerializeJsonArray()
	{
		var array = _json.CreateArray();

		array.Add(1);
		array.Add(5);
		array.Add(999);
		array.Add("test");

		Assert.Equal(
			"[1,5,999,\"test\"]",
			_json.Serialize(array)
		);
	}

	[Fact]
	public void DeserializeJsonArray()
	{
		var result = _json.Deserialize<IJsonArray>("[1,5,999,\"test\"]");

		Assert.NotNull(result);
		Assert.Equal(4, result.Count);
		Assert.Equal("1", result[0].ToString());
		Assert.Equal("5", result[1].ToString());
		Assert.Equal("999", result[2].ToString());
		Assert.Equal("test", result[3].ToString());
	}

	[Fact]
	public void SerializeJsonObject()
	{
		// TODO
	}

	[Fact]
	public void DeserializeJsonObject()
	{
		var result = _json.Deserialize<IJsonObject>("{\"test\":\"qwe\",\"asd\":5}");

		Assert.NotNull(result);
		Assert.Equal("qwe", result["test"].ToString());
		Assert.Equal("5", result["asd"].ToString());
	}

	[Fact]
	public void SerializeJsonValue()
	{
		var array = _json.CreateArray();

		array.Add(5);

		Assert.NotNull(array[0] as IJsonValue);
		Assert.Equal("5", _json.Serialize(array[0]));
	}

	[Fact]
	public void DeserializeJsonValueInt()
	{
		var result = _json.Deserialize<IJsonValue>("5");

		Assert.NotNull(result);
		Assert.Equal("5", result.ToString());
	}

	[Fact]
	public void DeserializeJsonValueString()
	{
		var result = _json.Deserialize<IJsonValue>("\"qwe\"");

		Assert.NotNull(result);
		Assert.Equal("qwe", result.ToString());
	}

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
}