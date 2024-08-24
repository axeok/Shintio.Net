using System;
using System.Collections.Generic;
using System.Globalization;
using Shintio.Essentials.Common;
using Shintio.Json.Attributes;
using Shintio.Math.Utils;

namespace Shintio.Math.Common
{
	public class Vector2 : ValueObject
	{
		public static Vector2 Zero = new Vector2(0, 0);
		public static Vector2 One = new Vector2(1, 1);
		public static Vector2 Forward = new Vector2(1, 0);
		public static Vector2 Back = new Vector2(-1, 0);
		public static Vector2 Right = new Vector2(0, 1);
		public static Vector2 Left = new Vector2(0, -1);

		[JsonConstructor]
		public Vector2(float x, float y) => (X, Y) = (x, y);

		public Vector2() => (X, Y) = (0, 0);
		public Vector2(Vector2 vector) => (X, Y) = (vector.X, vector.Y);

		public float X { get; private set; } = 0;
		public float Y { get; private set; } = 0;
		
		public static Vector2 Abs(Vector2 value) => new Vector2(Mathf.Abs(value.X), Mathf.Abs(value.Y));

		public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
		{
			t = Mathf.Clamp01(t);

			return new Vector2(
				a.X + (b.X - a.X) * t,
				a.Y + (b.Y - a.Y) * t
			);
		}

		public float GetMagnitude()
		{
			return Mathf.Hypot(X, Y);
		}

		public static float Distance(Vector2 a, Vector2 b)
		{
			return (a - b).GetMagnitude();
		}
		
		public static Vector2 RotateAround(Vector2 point, Vector2 center, Angle angle)
		{
			var radians = angle.Radians;
			var cos = Mathf.Cos(radians);
			var sin = Mathf.Sin(radians);

			return new Vector2(
				(point.X - center.X) * cos - (point.Y - center.Y) * sin + center.X,
				(point.X - center.X) * sin + (point.Y - center.Y) * cos + center.Y
			);
		}

		public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);

		public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);

		public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);

		public static Vector2 operator *(Vector2 a, float d) => new Vector2(a.X * d, a.Y * d);

		public static Vector2 operator /(Vector2 a, float d) => new Vector2(a.X / d, a.Y / d);
		
		public static bool operator >(Vector2 a, Vector2 b) => a.X > b.X && a.Y > b.Y;
		public static bool operator <(Vector2 a, Vector2 b) => a.X < b.X && a.Y < b.Y;

		public override string ToString()
		{
			var format = CultureInfo.InvariantCulture.NumberFormat;

			return $"({X.ToString(format)}, {Y.ToString(format)})";
		}

		public void Deconstruct(out float x, out float y)
		{
			x = X;
			y = Y;
		}

		public float DistanceTo(Vector2 to)
		{
			return Distance(this, to);
		}
		
		public Vector2 RotateAround(Vector2 center, Angle angle)
		{
			return RotateAround(this, center, angle);
		}

		public float LengthSquared() => (X * X + Y * Y);

		public float Length() => MathF.Sqrt(LengthSquared());

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return X;
			yield return Y;
		}

		public static float Dot(Vector2 lhs, Vector2 rhs)
		{
			return lhs.X * rhs.X + lhs.Y * rhs.Y;
		}
	}
}