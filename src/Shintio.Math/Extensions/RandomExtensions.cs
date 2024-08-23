using Shintio.Essentials.Common;
using Shintio.Essentials.Utils.Random;

namespace Shintio.Math.Extensions
{
	public static class RandomExtensions
	{
		public static T Item<T>(this Random random) where T : DataCollection
		{
			return random.Item(DataCollection.GetValues<T>());
		}
	}
}