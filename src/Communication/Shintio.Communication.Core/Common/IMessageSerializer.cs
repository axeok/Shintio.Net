namespace Shintio.Communication.Core.Common
{
	public interface IMessageSerializer
	{
		byte[] Serialize(object?[] arguments);
		object?[] Deserialize(byte[] bytes);
	}
}