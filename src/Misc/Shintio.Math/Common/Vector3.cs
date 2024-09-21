using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Shintio.Essentials.Common;
using Shintio.Json.Attributes;
using Shintio.Math.Common.Enums;
using Shintio.Math.Utils;

namespace Shintio.Math.Common
{
	public class Vector3 : ValueObject
	{
		private const float SquareRootOfTwo = 1.41421356237309504880168872420969807856967187537694807317667973799f;
		private const float KEpsilonNormalSqrt = 1e-15F;

		public static readonly Vector3 Zero = new Vector3(0, 0, 0);
		public static readonly Vector3 One = new Vector3(1, 1, 1);
		public static readonly Vector3 Up = new Vector3(0, 0, 1);
		public static readonly Vector3 Down = new Vector3(0, 0, -1);
		public static readonly Vector3 Forward = new Vector3(1, 0, 0);
		public static readonly Vector3 Back = new Vector3(-1, 0, 0);
		public static readonly Vector3 Right = new Vector3(0, 1, 0);
		public static readonly Vector3 Left = new Vector3(0, -1, 0);

		public Vector3(float? x, float? y, float? z) => (X, Y, Z) = (x ?? 0, y ?? 0, z ?? 0);
		
		[JsonConstructor]
		public Vector3(float x, float y, float z) => (X, Y, Z) = (x, y, z);

		public Vector3(double? x, double? y, double? z) =>
			(X, Y, Z) = ((float)(x ?? 0), (float)(y ?? 0), (float)(z ?? 0));

		public Vector3(float x, float y) => (X, Y, Z) = (x, y, 0);
		public Vector3() => (X, Y, Z) = (0, 0, 0);
		public Vector3(Vector3 vector) => (X, Y, Z) = (vector.X, vector.Y, vector.Z);
		public Vector3(Vector2 vector, float z = 0) => (X, Y, Z) = (vector.X, vector.Y, z);

		public float X { get; private set; } = 0;
		public float Y { get; private set; } = 0;
		public float Z { get; private set; } = 0;

		public Vector3 GetNormalized()
		{
			var length = Length();
			return new Vector3(X / length, Y / length, Z / length);
		}

		public Transform AsTransform()
		{
			return new Transform(this);
		}

		public static Vector3 Abs(Vector3 value) =>
			new Vector3(Mathf.Abs(value.X), Mathf.Abs(value.Y), Mathf.Abs(value.Z));
		
		public static void Initialize()
		{
			// Inits static properties.
		}

		public static float Angle(Vector3 from, Vector3 to)
		{
			// sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
			var denominator = (float)System.Math.Sqrt(MagnitudeSquared(from) * MagnitudeSquared(to));
			if (denominator < KEpsilonNormalSqrt)
			{
				return 0F;
			}

			var dot = Mathf.Clamp(Dot(from, to) / denominator, -1F, 1F);
			return ((float)System.Math.Acos(dot)) * Mathf.Rad2Deg;
		}

		public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
		{
			return new Vector3(
				lhs.Y * rhs.Z - lhs.Z * rhs.Y,
				lhs.Z * rhs.X - lhs.X * rhs.Z,
				lhs.X * rhs.Y - lhs.Y * rhs.X
			);
		}

		public static float Dot(Vector3 lhs, Vector3 rhs)
		{
			return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
		}

		public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
		{
			return LerpUnclamped(a, b, Mathf.Clamp01(t));
		}

		public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
		{
			return new Vector3(
				a.X + (b.X - a.X) * t,
				a.Y + (b.Y - a.Y) * t,
				a.Z + (b.Z - a.Z) * t
			);
		}

		public static Vector3 LerpAngle(Vector3 a, Vector3 b, float t)
		{
			return LerpAngleUnclamped(a, b, Mathf.Clamp01(t));
		}

		public static Vector3 LerpAngleUnclamped(Vector3 a, Vector3 b, float t)
		{
			return new Vector3(
				Mathf.LerpAngleUnclamped(a.X, b.X, t),
				Mathf.LerpAngleUnclamped(a.Y, b.Y, t),
				Mathf.LerpAngleUnclamped(a.Z, b.Z, t)
			);
		}

		public static float Magnitude(Vector3 vector)
		{
			return Mathf.Hypot(vector.X, vector.Y, vector.Z);
		}

		public static float MagnitudeSquared(Vector3 vector)
		{
			return Mathf.HypotSquared(vector.X, vector.Y, vector.Z);
		}

		public static float Magnitude2D(Vector3 vector)
		{
			return Mathf.Hypot(vector.X, vector.Y);
		}

		public static float MagnitudeSquared2D(Vector3 vector)
		{
			return Mathf.HypotSquared(vector.X, vector.Y);
		}

		public static float Distance(Vector3 a, Vector3 b)
		{
			return Magnitude(a - b);
		}

		public static float DistanceSquared(Vector3 a, Vector3 b)
		{
			return MagnitudeSquared(a - b);
		}

		public static float Distance2D(Vector3 a, Vector3 b)
		{
			return Magnitude2D(a - b);
		}

		public static float DistanceToSquared2D(Vector3 a, Vector3 b)
		{
			return MagnitudeSquared2D(a - b);
		}

		public static Vector3 Project(Vector3 vector, Vector3 onNormal)
		{
			var sqrMag = Dot(onNormal, onNormal);
			if (sqrMag < Mathf.Epsilon)
			{
				return Zero;
			}

			var dot = Dot(vector, onNormal);
			return new Vector3(onNormal.X * dot / sqrMag,
				onNormal.Y * dot / sqrMag,
				onNormal.Z * dot / sqrMag);
		}

		public static Vector3 Average(IEnumerable<Vector3> vectors)
		{
			var x = 0f;
			var y = 0f;
			var z = 0f;
			var count = 0;

			foreach (var vector in vectors)
			{
				x += vector.X;
				y += vector.Y;
				z += vector.Z;
				count++;
			}

			return new Vector3(x / count, y / count, z / count);
		}

		public static Vector3 RotateAround(Vector3 point, Vector2 center, float angle)
		{
			var radians = angle * Mathf.Deg2Rad;
			var cos = System.Math.Cos(radians);
			var sin = System.Math.Sin(radians);

			return new Vector3(
				(point.X - center.X) * cos - (point.Y - center.Y) * sin + center.X,
				(point.X - center.X) * sin + (point.Y - center.Y) * cos + center.Y,
				point.Z
			);
		}

		public static Vector3 RotateAround(Vector3 point, Vector3 center, Rotation rotation)
		{
			var fromOrigin = point - center;
			
			var orderToMethods = new Dictionary<RotationOrder, List<Func<Vector3, Angle, Vector3>>>()
			{
				{ RotationOrder.XYZ, new List<Func<Vector3, Angle, Vector3>> { Mathf.RotateAroundX, Mathf.RotateAroundY, Mathf.RotateAroundZ } },
				{ RotationOrder.XZY, new List<Func<Vector3, Angle, Vector3>> { Mathf.RotateAroundX, Mathf.RotateAroundZ, Mathf.RotateAroundY } },
				{ RotationOrder.YXZ, new List<Func<Vector3, Angle, Vector3>> { Mathf.RotateAroundY, Mathf.RotateAroundX, Mathf.RotateAroundZ } },
				{ RotationOrder.YZX, new List<Func<Vector3, Angle, Vector3>> { Mathf.RotateAroundY, Mathf.RotateAroundZ, Mathf.RotateAroundX } },
				{ RotationOrder.ZXY, new List<Func<Vector3, Angle, Vector3>> { Mathf.RotateAroundZ, Mathf.RotateAroundX, Mathf.RotateAroundY } },
				{ RotationOrder.ZYX, new List<Func<Vector3, Angle, Vector3>> { Mathf.RotateAroundZ, Mathf.RotateAroundY, Mathf.RotateAroundX } },
			};
			var methodToVectorComponentGetter = new Dictionary<Func<Vector3, Angle, Vector3>, Func<Vector3, float>>
			{
				{ Mathf.RotateAroundX, vector3 => vector3.X },
				{ Mathf.RotateAroundY, vector3 => vector3.Y },
				{ Mathf.RotateAroundZ, vector3 => vector3.Z },
			};

			var order = Enum.IsDefined(typeof(RotationOrder), rotation.Order) ? rotation.Order : RotationOrder.XYZ;
			var methods = orderToMethods[order];

			var tempResult = fromOrigin;
			foreach (var method in methods)
			{
				var angleValue = methodToVectorComponentGetter[method](rotation.Angles);
				tempResult = method(tempResult, new Angle(angleValue, rotation.Measurement));
			}

			return tempResult + center;
		}

		public static bool IsPointWithin(IEnumerable<Vector3> vertices, Vector3 position, float height)
		{
			var arr = vertices.ToArray();

			var verticesCount = arr.Length;

			// TODO: разобраться для чего это.
			// var isAppropriateAngleSum = GetAngleSumBetweenPositionAndVertices(position, Vertices) >= UnknownConstant;

			for (var index = 0; index < verticesCount; index++)
			{
				var vertex = arr[index];
				var rightHeight = height == 0 || position.Z >= vertex.Z && position.Z <= (vertex.Z + height);
				if (!rightHeight)
				{
					return false;
				}
			}

			return IsPointInZone2D(position, arr);
		}

		public static bool IsPointInZone2D(Vector3 point, IReadOnlyList<Vector3> area)
		{
			var x = point.X;
			var y = point.Y;

			var inside = false;

			for (int i = 0, j = area.Count - 1; i < area.Count; j = i++)
			{
				float xi = area[i].X, yi = area[i].Y;
				float xj = area[j].X, yj = area[j].Y;

				var intersect = ((yi > y) != (yj > y)) &&
				                (x < (xj - xi) * (y - yi) / (yj - yi) + xi);

				if (intersect)
				{
					inside = !inside;
				}
			}

			return inside;
		}

		public static bool IsPointWithin(
			PointShape shape,
			Vector3 point,
			Vector3 center,
			float radius,
			float height,
			float heading = 0
		) => shape switch
		{
			PointShape.Cylinder => IsPointWithinCylinder(point, center, radius, height),
			PointShape.Cube => IsPointWithinCube(point, center, radius, height, heading),
			_ => IsPointWithinCylinder(point, center, radius, height),
		};

		public static bool IsPointWithinCylinder(Vector3 point, Vector3 center, float radius, float height)
		{
			if (point.Z < center.Z || point.Z > center.Z + height)
			{
				return false;
			}

			var deltaX = point.X - center.X;
			var deltaY = point.Y - center.Y;

			return (deltaX * deltaX) + (deltaY * deltaY) <= radius * radius;
		}

		public static bool IsPointWithinCube(
			Vector3 point,
			Vector3 center,
			float radius,
			float height,
			float heading = 0
		)
		{
			if (point.Z < center.Z || point.Z > center.Z + height)
			{
				return false;
			}

			var diagonalSize = radius * SquareRootOfTwo;
			var A = center + Mathf.GetPointOnCircle(diagonalSize, (heading + 45) * Mathf.Deg2Rad);
			var B = center + Mathf.GetPointOnCircle(diagonalSize, (heading + 135) * Mathf.Deg2Rad);
			var D = center + Mathf.GetPointOnCircle(diagonalSize, (heading + 315) * Mathf.Deg2Rad);

			var AM = point - A;
			var AB = B - A;
			var AD = D - A;

			var AMAB = Vector2.Dot(AM, AB);
			var AMAD = Vector2.Dot(AM, AD);

			return 0 < AMAB && AMAB < Vector2.Dot(AB, AB) &&
			       0 < AMAD && AMAD < Vector2.Dot(AD, AD);
		}

		public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

		public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

		public static Vector3 operator -(Vector3 a) => new Vector3(-a.X, -a.Y, -a.Z);

		public static Vector3 operator *(Vector3 a, float d) => new Vector3(a.X * d, a.Y * d, a.Z * d);

		public static Vector3 operator /(Vector3 a, float d) => new Vector3(a.X / d, a.Y / d, a.Z / d);

		public static implicit operator Vector2(Vector3 v) => new Vector2(v.X, v.Y);

		public override string ToString()
		{
			var format = CultureInfo.InvariantCulture.NumberFormat;

			return $"({X.ToString(format)}, {Y.ToString(format)}, {Z.ToString(format)})";
		}

		public void Deconstruct(out float x, out float y, out float z)
		{
			x = X;
			y = Y;
			z = Z;
		}

		public float DistanceTo(Vector3 to)
		{
			return Distance(this, to);
		}

		public float DistanceToSquared(Vector3 to)
		{
			return DistanceSquared(this, to);
		}

		public float DistanceTo2D(Vector3 to)
		{
			return Distance2D(this, to);
		}

		public float DistanceToSquared2D(Vector3 to)
		{
			return DistanceToSquared2D(this, to);
		}

		public float HeadingDifferenceTo(Vector3 to)
		{
			return Mathf.GetHeadingDifferenceTo(Z, to.Z);
		}

		public Vector3 RotateAround(Vector2 center, float angle)
		{
			return RotateAround(this, center, angle);
		}
		
		public Vector3 RotateAround(Vector3 center, Rotation rotation)
		{
			return RotateAround(this, center, rotation);
		}

		public float LengthSquared() => (X * X + Y * Y + Z * Z);

		public float Length() => MathF.Sqrt(LengthSquared());
		public float Magnitude() => Length();

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return X;
			yield return Y;
			yield return Z;
		}
	}
}