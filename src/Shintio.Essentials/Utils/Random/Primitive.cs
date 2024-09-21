using System.Text;

namespace Shintio.Essentials.Utils.Random
{
	public partial class Random
	{
		public double NextDouble()
		{
			return _random.NextDouble();
		}

		public float Float(float min, float max, bool includeMax = true)
		{
			// TODO: добавить поддержку includeMax
			return (float)_random.NextDouble() * (max - min) + min;
		}

		public float Float(float max, bool includeMax = true)
		{
			return Float(0f, max, includeMax);
		}

		public float Float((float min, float max) range, bool includeMax = true)
		{
			return Float(range.min, range.max, includeMax);
		}

		public int Int(int max, bool includeMax = true)
		{
			return Int(0, max, includeMax);
		}

		public int Int(int min, int max, bool includeMax = true)
		{
			var value = _random.Next(min, max + (includeMax ? 1 : 0));

			return value;
		}

		public uint UInt(uint min, uint max, bool includeMax = true)
		{
			return (uint)Int((int)min, (int)max, includeMax);
		}

		public byte Byte()
		{
			return (byte)Int(0, 255);
		}

		public bool Bool(int chance = 50)
		{
			return Int(100) <= chance;
		}

		public bool Bool(float chance)
		{
			return Float(100) <= chance;
		}

		public float Sign(int chance = 50)
		{
			return Bool(chance) ? -1 : 1;
		}

		public string String(int length = 8, bool numbers = true, bool lowers = true, bool uppers = true)
		{
			const string numbersString = "0123456789";
			const string uppersString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string lowersString = "abcdefghijklmnopqrstuvwxyz";

			var chars = (numbers ? numbersString : "") + (uppers ? uppersString : "") + (lowers ? lowersString : "");

			var result = new StringBuilder();

			for (var i = 0; i < length; i++)
			{
				result.Append(Item(chars));
			}

			return result.ToString();
		}
	}
}