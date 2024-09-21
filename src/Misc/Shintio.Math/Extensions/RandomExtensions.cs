using Shintio.Essentials.Common;
using Shintio.Essentials.Utils.Random;
using Shintio.Math.Common;
using Shintio.Math.Utils;

namespace Shintio.Math.Extensions
{
	public static class RandomExtensions
	{
		public static T Item<T>(this Random random) where T : DataCollection
		{
			return random.Item(DataCollection.GetValues<T>());
		}

		public static Vector2 PointWithinCircle(this Random random, Vector2 center, float radius)
		{
			var r = radius * Mathf.Sqrt((float)random.NextDouble());
			var theta = (float)random.NextDouble() * 2 * Mathf.PI;

			return new Vector2(
				center.X + r * Mathf.Cos(theta),
				center.Y + r * Mathf.Sin(theta)
			);
		}
	}
}