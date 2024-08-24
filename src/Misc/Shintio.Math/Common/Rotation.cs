using System.Collections.Generic;
using Shintio.Essentials.Common;
using Shintio.Math.Common.Enums;
using Shintio.Math.Utils;

namespace Shintio.Math.Common
{
	public class Rotation : ValueObject
	{
		public Rotation(
			Vector3 angles,
			AngleMeasurement measurement = AngleMeasurement.Degrees,
			RotationOrder order = RotationOrder.XYZ
		)
		{
			Angles = angles;
			Order = order;
			Measurement = measurement;
		}

		public Vector3 Angles { get; }
		public RotationOrder Order { get; }
		public AngleMeasurement Measurement { get; }
		
		public static Rotation Lerp(Rotation a, Rotation b, float t)
		{
			return LerpUnclamped(a, b, Mathf.Clamp01(t));
		}
		
		public static Rotation LerpUnclamped(Rotation a, Rotation b, float t)
		{
			return new Rotation(new Vector3(
				Mathf.LerpAngleUnclamped(a.Angles.X, b.Angles.X, t),
				Mathf.LerpAngleUnclamped(a.Angles.Y, b.Angles.Y, t),
				Mathf.LerpAngleUnclamped(a.Angles.Z, b.Angles.Z, t)
			), b.Measurement, b.Order);
		}
		
		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Angles;
			yield return Order;
			yield return Measurement;
		}
	}
}