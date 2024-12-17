namespace Shintio.Essentials.Interfaces
{
	public interface IReversable<out T>
	{
		T GetReversed();
	}
}