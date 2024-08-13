namespace Shintio.Essentials.Utils.Random
{
	public partial class Random
	{
		public static double NextDouble()
		{
			return Instance._random.NextDouble();
		}

		public static float Float(float min, float max, bool includeMax = true)
		{
			return (float)Instance._random.NextDouble() * (max - min) + min;
		}

		public static float Float(float max, bool includeMax = true)
		{
			return Float(0f, max, includeMax);
		}

		public static float Float((float min, float max) range, bool includeMax = true)
		{
			return Float(range.min, range.max, includeMax);
		}

		public static int Int(int max, bool includeMax = true, System.Random? random = null)
		{
			return Int(0, max, includeMax, random);
		}

		public static int Int(int min, int max, bool includeMax = true, System.Random? random = null)
		{
			var value = (random ?? Instance._random).Next(min, max + (includeMax ? 1 : 0));

			return value;
		}

		public static uint UInt(uint min, uint max, bool includeMax = true)
		{
			return (uint)Int((int)min, (int)max, includeMax);
		}

		public static byte Byte()
		{
			return (byte)Int(0, 255);
		}

		public static bool Bool(int chance = 50)
		{
			return Int(100) <= chance;
		}

		public static float Negative(int chance = 50)
		{
			return Bool(chance) ? -1 : 1;
		}

		public static string String(int length = 8, bool numbers = true, bool lowers = true, bool uppers = true)
		{
			var chars = (numbers ? "0123456789" : "") + (uppers ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ" : "") +
			            (lowers ? "abcdefghijklmnopqrstuvwxyz" : "");

			var result = "";

			for (var i = 0; i < length; i++)
			{
				result += Item(chars);
			}

			return result;
		}
	}
}