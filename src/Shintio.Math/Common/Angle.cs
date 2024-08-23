using System.Collections.Generic;
using Shintio.Essentials.Common;
using Shintio.Json.Attributes;
using Shintio.Math.Common.Enums;
using Shintio.Math.Utils;

namespace Shintio.Math.Common
{
	public class Angle : ValueObject
	{
		public static readonly Angle Zero = new Angle(0);
		
		[JsonConstructor]
		public Angle(float degrees)
		{
			Degrees = degrees;
			Radians = degrees * Mathf.Deg2Rad;
		}

		private Angle(float degrees, float radians)
		{
			Degrees = degrees;
			Radians = radians;
		}
		
		public Angle(float value, AngleMeasurement measurement)
		{
			if (measurement == AngleMeasurement.Degrees)
			{
				Degrees = value;
				Radians = value * Mathf.Deg2Rad;
			}
			else if (measurement == AngleMeasurement.Radians)
			{
				Radians = value;
				Degrees = value * Mathf.Rad2Deg;
			}
		}

		public float Degrees { get; private set; }
		[JsonIgnore] public float Radians { get; private set; }

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Degrees;
		}

		public static Angle FromRadians(float radians)
		{
			return new Angle(radians * Mathf.Rad2Deg, radians);
		}
		
		public static Angle FromDegrees(float degrees)
		{
			return new Angle(degrees);
		}
	}
}