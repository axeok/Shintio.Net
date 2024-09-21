using System.Collections.ObjectModel;
using Shintio.Json.Interfaces;
using Shintio.Json.Newtonsoft.Common;
using Shintio.Json.Nodes;
using Shintio.Json.System.Common;
using Shintio.Math.Common;
using Shintio.Net.Tests.Models;

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

	[Fact]
	public void SerializeHasDiscriminatorClass()
	{
		Assert.Equal(
			"{\"X\":1,\"Discriminator\":\"TestChild1\"}",
			_json.Serialize(new TestChild1())
		);
		Assert.Equal(
			"{\"Y\":2,\"Discriminator\":\"TestChild2\"}",
			_json.Serialize(new TestChild2())
		);
	}

	[Fact]
	public void DeserializeHasDiscriminatorClass()
	{
		var json1 = "{\"X\":1,\"Discriminator\":\"TestChild1\"}";
		var json2 = "{\"Y\":2,\"Discriminator\":\"TestChild2\"}";

		var test1 = _json.Deserialize<TestParent>(json1) as TestChild1;
		var test2 = _json.Deserialize<TestParent>(json2) as TestChild2;

		Assert.IsAssignableFrom<TestParent>(test1);
		Assert.IsAssignableFrom<TestParent>(test2);
		Assert.IsType<TestChild1>(test1);
		Assert.IsType<TestChild2>(test2);
		Assert.Equal(1, test1.X);
		Assert.Equal("TestChild1", test1.Discriminator);
		Assert.Equal(2, test2.Y);
		Assert.Equal("TestChild2", test2.Discriminator);
	}

	[Fact]
	public void SerializeHasDiscriminatorInterface()
	{
		Assert.Equal(
			"{\"X\":1,\"Discriminator\":\"TestChild1\"}",
			_json.Serialize(new TestChild1())
		);
		Assert.Equal(
			"{\"Y\":2,\"Discriminator\":\"TestChild2\"}",
			_json.Serialize(new TestChild2())
		);
	}

	[Fact]
	public void DeserializeHasDiscriminatorInterface()
	{
		var json1 = "{\"X\":1,\"Discriminator\":\"TestChild1\"}";
		var json2 = "{\"Y\":2,\"Discriminator\":\"TestChild2\"}";

		var test1 = _json.Deserialize<ITestChild>(json1) as TestChild1;
		var test2 = _json.Deserialize<ITestChild>(json2) as TestChild2;

		Assert.IsAssignableFrom<ITestChild>(test1);
		Assert.IsAssignableFrom<ITestChild>(test2);
		Assert.IsType<TestChild1>(test1);
		Assert.IsType<TestChild2>(test2);
		Assert.Equal(1, test1.X);
		Assert.Equal("TestChild1", test1.Discriminator);
		Assert.Equal(2, test2.Y);
		Assert.Equal("TestChild2", test2.Discriminator);
	}

	[Fact]
	public void SerializeWithIgnore()
	{
		Assert.Equal(
			"{\"Row\":\"qwe\"}",
			_json.Serialize(new TestDataWithIgnore { Row = "qwe" })
		);
	}

	[Fact]
	public void DeserializeWithIgnore()
	{
		var result = _json.Deserialize<TestDataWithIgnore>("{\"Number\":5,\"Row\":\"qwe\"}");

		Assert.NotNull(result);
		Assert.Equal(3, result.Number);
		Assert.Equal("qwe", result.Row);
	}

	[Fact]
	public void SerializeReadOnlyCollection()
	{
		Assert.Equal(
			"[0,5,999]",
			_json.Serialize(new ReadOnlyCollection<int>([0, 5, 999]))
		);
	}

	[Fact]
	public void DeserializeReadOnlyCollection()
	{
		var result = _json.Deserialize<ReadOnlyCollection<int>>("[0,5,999]");

		Assert.NotNull(result);
		Assert.Equal(0, result[0]);
		Assert.Equal(5, result[1]);
		Assert.Equal(999, result[2]);
	}

	[Fact]
	public void SerializeReadOnlyDictionary()
	{
		Assert.Equal(
			"{\"q\":0,\"w\":5,\"e\":999}",
			_json.Serialize(new ReadOnlyDictionary<string, int>(new Dictionary<string, int>()
			{
				["q"] = 0,
				["w"] = 5,
				["e"] = 999,
			}))
		);
	}

	[Fact]
	public void DeserializeReadOnlyDictionary()
	{
		var result = _json.Deserialize<ReadOnlyDictionary<string, int>>("{\"q\":0,\"w\":5,\"e\":999}");

		Assert.NotNull(result);
		Assert.Equal(0, result["q"]);
		Assert.Equal(5, result["w"]);
		Assert.Equal(999, result["e"]);
	}

	[Fact]
	public void SerializeVector3()
	{
		if (_json is NewtonsoftJson)
		{
			Assert.Equal(
				"{\"X\":1.0,\"Y\":5.0,\"Z\":999.0}",
				_json.Serialize(new Vector3(1, 5, 999))
			);
		}
		else if (_json is SystemJson)
		{
			Assert.Equal(
				"{\"X\":1,\"Y\":5,\"Z\":999}",
				_json.Serialize(new Vector3(1, 5, 999))
			);
		}
	}

	[Fact]
	public void DeserializeVector3()
	{
		var result = _json.Deserialize<Vector3>("{\"X\":1.0,\"Y\":5.0,\"Z\":999.0}");

		Assert.NotNull(result);
		Assert.Equal(1, result.X);
		Assert.Equal(5, result.Y);
		Assert.Equal(999, result.Z);
	}

	[Fact]
	public void SerializeDataCollection()
	{
		Assert.Equal(
			"\"Value1\"",
			_json.Serialize(TestDataCollection.Value1)
		);
	}

	[Fact]
	public void DeserializeDataCollection()
	{
		var result = _json.Deserialize<TestDataCollection>("\"Value1\"");

		Assert.NotNull(result);
		Assert.Equal(TestDataCollection.Value1, result);
	}
}