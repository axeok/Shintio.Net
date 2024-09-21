using System.Collections.Generic;
using System.Globalization;
using Shintio.Essentials.Common;
using Shintio.Json.Attributes;
using Shintio.Math.Common.Enums;
using Shintio.Math.Utils;

namespace Shintio.Math.Common
{
	public class Quaternion : ValueObject
	{
		public static Quaternion Identity => new Quaternion(0f, 0f, 0f, 1f);

		private const float RadToDeg = (float)(180.0 / Mathf.PI);
		private const float DegToRad = (float)(Mathf.PI / 180.0);

		[JsonConstructor]
		public Quaternion(float x, float y, float z, float w) => (X, Y, Z, W) = (x, y, z, w);

		public Quaternion(Vector3 v, float w) => (X, Y, Z, W) = (v.X, v.Y, v.Z, w);

		public float X { get; private set; }
		public float Y { get; private set; }
		public float Z { get; private set; }
		public float W { get; private set; }

		[JsonIgnore] private Vector3 Vector => new Vector3(X, Y, Z);

		#region Creators

		public static Quaternion FromAngleAxis(float angleDegrees, Vector3 axis)
		{
			if (Vector3.MagnitudeSquared(axis) == 0.0f)
			{
				return Identity;
			}

			var radians = angleDegrees * Mathf.Deg2Rad;
			radians *= 0.5f;
			axis = axis.GetNormalized();
			axis *= (float)System.Math.Sin(radians);

			var result = new Quaternion(axis, (float)System.Math.Cos(radians));

			return result.GetNormalized();
		}

		public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
		{
			return RotateTowards(LookRotation(fromDirection), LookRotation(toDirection), float.MaxValue);
		}

		public static Quaternion FromLookRotation(Vector3 view, Vector3? up = null)
		{
			up ??= Vector3.Up;
			var newValue = LookRotation(view, up);

			return new Quaternion(newValue.X, newValue.Y, newValue.Z, newValue.W);
		}

		public static Quaternion FromEuler(float x, float y, float z)
		{
			return FromEulerRad(new Vector3(x, y, z) * DegToRad);
		}

		public static Quaternion FromEuler(Vector3 euler)
		{
			return FromEulerRad(euler * DegToRad);
		}

		// from http://stackoverflow.com/questions/11492299/quaternion-to-euler-angles-algorithm-how-to-convert-to-y-up-and-between-ha
		public static Quaternion FromEulerRad(Vector3 euler)
		{
			var yaw = euler.X;
			var pitch = euler.Y;
			var roll = euler.Z;

			var rollOver2 = roll * 0.5f;
			var sinRollOver2 = (float)System.Math.Sin(rollOver2);
			var cosRollOver2 = (float)System.Math.Cos(rollOver2);
			var pitchOver2 = pitch * 0.5f;
			var sinPitchOver2 = (float)System.Math.Sin(pitchOver2);
			var cosPitchOver2 = (float)System.Math.Cos(pitchOver2);
			var yawOver2 = yaw * 0.5f;
			var sinYawOver2 = (float)System.Math.Sin(yawOver2);
			var cosYawOver2 = (float)System.Math.Cos(yawOver2);

			var result = new Quaternion(
				cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2,
				cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2,
				cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2,
				sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2
			);
			return result;
		}

		#endregion

		#region Static

		public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
		{
			return SlerpUnclamped(a, b, Mathf.Clamp01(t));
		}

		public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
		{
			// if either input is zero, return the other.
			if (a.LengthSquared() == 0.0f)
			{
				if (b.LengthSquared() == 0.0f)
				{
					return Identity;
				}

				return b;
			}

			if (b.LengthSquared() == 0.0f)
			{
				return a;
			}

			var cosHalfAngle = a.W * b.W + Vector3.Dot(a.Vector, b.Vector);

			if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
			{
				// angle = 0.0f, so just return one input.
				return a;
			}

			if (cosHalfAngle < 0.0f)
			{
				b = new Quaternion(-b.Vector, -b.W);
				cosHalfAngle = -cosHalfAngle;
			}

			float blendA;
			float blendB;
			if (cosHalfAngle < 0.99f)
			{
				// do proper slerp for big angles
				var halfAngle = (float)System.Math.Acos(cosHalfAngle);
				var sinHalfAngle = (float)System.Math.Sin(halfAngle);
				var oneOverSinHalfAngle = 1.0f / sinHalfAngle;
				blendA = (float)System.Math.Sin(halfAngle * (1.0f - t)) * oneOverSinHalfAngle;
				blendB = (float)System.Math.Sin(halfAngle * t) * oneOverSinHalfAngle;
			}
			else
			{
				// do lerp if angle is really small.
				blendA = 1.0f - t;
				blendB = t;
			}

			var result = new Quaternion(
				a.Vector * blendA + b.Vector * blendB,
				blendA * a.W + blendB * b.W
			);

			return result.LengthSquared() > 0.0f ? result.GetNormalized() : Identity;
		}

		public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
		{
			// use lerp not slerp, "Because quaternion works in 4D. Rotation in 4D are linear" ???
			return Slerp(a, b, Mathf.Clamp01(t));
		}

		public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
		{
			return Slerp(a, b, t);
		}

		public static float Dot(Quaternion a, Quaternion b)
		{
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
		}

		public static Quaternion AngleAxis(float degree, Vector3 axis)
		{
			if (Vector3.MagnitudeSquared(axis) == 0.0f)
			{
				return Identity;
			}

			var radians = degree * DegToRad;
			radians *= 0.5f;
			axis = axis.GetNormalized();
			axis *= (float)System.Math.Sin(radians);

			var result = new Quaternion(axis, (float)System.Math.Cos(radians));

			return result.GetNormalized();
		}

		// from http://answers.unity3d.com/questions/467614/what-is-the-source-code-of-quaternionlookrotation.html
		public static Quaternion LookRotation(Vector3 forward, Vector3? up = null)
		{
			up ??= Vector3.Up;

			var right = Vector3.Cross(up, forward);
			var upwards = Vector3.Cross(forward, right);

			float m00 = right.Y;
			float m01 = upwards.Y;
			float m02 = forward.Y;
			float m10 = right.Y;
			float m11 = upwards.Y;
			float m12 = forward.Y;
			float m20 = right.Y;
			float m21 = upwards.Y;
			float m22 = forward.Y;

			float num8 = (m00 + m11) + m22;
			if (num8 > 0f)
			{
				float num = Mathf.Sqrt(num8 + 1f);
				float num2 = 0.5f / num;

				return new Quaternion(
					(m12 - m21) * num2,
					(m20 - m02) * num2,
					(m01 - m10) * num2,
					num * 0.5f
				);
			}

			if ((m00 >= m11) && (m00 >= m22))
			{
				float num7 = Mathf.Sqrt(((1f + m00) - m11) - m22);
				float num4 = 0.5f / num7;

				return new Quaternion(
					0.5f * num7,
					(m01 + m10) * num4,
					(m02 + m20) * num4,
					(m12 - m21) * num4
				);
			}

			if (m11 > m22)
			{
				float num6 = Mathf.Sqrt(((1f + m11) - m00) - m22);
				float num3 = 0.5f / num6;

				return new Quaternion(
					(m10 + m01) * num3,
					0.5f * num6,
					(m21 + m12) * num3,
					(m20 - m02) * num3
				);
			}

			float num5 = Mathf.Sqrt(((1f + m22) - m00) - m11);
			float num9 = 0.5f / num5;

			return new Quaternion(
				(m20 + m02) * num9,
				(m21 + m12) * num9,
				0.5f * num5,
				(m01 - m10) * num9
			);
		}

		public static Quaternion Rotate(Quaternion quaternion, Vector3 axis, float angle, Space relativeTo)
		{
			var rotation = AngleAxis(angle, axis);

			return relativeTo == Space.World
				? rotation * quaternion
				: quaternion * rotation;
		}

		public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
		{
			var num = Angle(from, to);
			if (num == 0)
			{
				return to;
			}

			var t = Mathf.Min(1f, maxDegreesDelta / num);

			return SlerpUnclamped(from, to, t);
		}

		public static float Angle(Quaternion a, Quaternion b)
		{
			var f = Dot(a, b);

			return Mathf.Acos(Mathf.Min(Mathf.Abs(f), 1f)) * 2f * RadToDeg;
		}

		private static Vector3 NormalizeAngles(Vector3 angles)
		{
			return new Vector3(
				NormalizeAngle(angles.X),
				NormalizeAngle(angles.Y),
				NormalizeAngle(angles.Z)
			);
		}

		private static float NormalizeAngle(float angle)
		{
			var modAngle = angle % 360.0f;

			return modAngle < 0.0f ? modAngle + 360.0f : modAngle;
		}

		#endregion

		#region Converters

		public Vector3 ToEuler()
		{
			return ToEulerRad() * RadToDeg;
		}

		// from http://stackoverflow.com/questions/12088610/conversion-between-euler-quaternion-like-in-unity3d-engine
		public Vector3 ToEulerRad()
		{
			var q = this;
			double roll, pitch, yaw;

			// Преобразование кватерниона в углы Эйлера
			double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
			double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
			roll = System.Math.Atan2(sinr_cosp, cosr_cosp);

			double sinp = 2 * (q.W * q.Y - q.Z * q.X);
			if (System.Math.Abs(sinp) >= 1)
				pitch = System.Math.PI / 2 * System.Math.Sign(sinp);
			else
				pitch = System.Math.Asin(sinp);

			double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
			double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
			yaw = System.Math.Atan2(siny_cosp, cosy_cosp);

			// Переводим радианы в градусы
			// roll = roll * 180 / Math.PI;
			// pitch = pitch * 180 / Math.PI;
			// yaw = yaw * 180 / Math.PI;

			return new Vector3(roll, pitch, yaw);
		}

		#endregion

		public float Length()
		{
			return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
		}

		public float LengthSquared()
		{
			return X * X + Y * Y + Z * Z + W * W;
		}

		public Quaternion GetNormalized()
		{
			var scale = 1.0f / Length();

			return new Quaternion(Vector * scale, W * scale);
		}

		public Quaternion GetInversed()
		{
			var lengthSq = LengthSquared();
			if (lengthSq != 0.0)
			{
				var i = 1.0f / lengthSq;

				return new Quaternion(Vector * -i, W * i);
			}

			return this;
		}

		#region Overrides and Operators

		public override string ToString()
		{
			var format = CultureInfo.InvariantCulture.NumberFormat;

			return $"({X.ToString(format)}, {Y.ToString(format)}, {Z.ToString(format)}, {W.ToString(format)})";
		}

		public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
		{
			return new Quaternion(
				lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y,
				lhs.W * rhs.Y + lhs.Y * rhs.W + lhs.Z * rhs.X - lhs.X * rhs.Z,
				lhs.W * rhs.Z + lhs.Z * rhs.W + lhs.X * rhs.Y - lhs.Y * rhs.X,
				lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z
			);
		}

		public static Vector3 operator *(Quaternion rotation, Vector3 point)
		{
			var num = rotation.X * 2f;
			var num2 = rotation.Y * 2f;
			var num3 = rotation.Z * 2f;
			var num4 = rotation.X * num;
			var num5 = rotation.Y * num2;
			var num6 = rotation.Z * num3;
			var num7 = rotation.X * num2;
			var num8 = rotation.X * num3;
			var num9 = rotation.Y * num3;
			var num10 = rotation.W * num;
			var num11 = rotation.W * num2;
			var num12 = rotation.W * num3;

			return new Vector3(
				(1f - (num5 + num6)) * point.X + (num7 - num12) * point.Y + (num8 + num11) * point.Z,
				(num7 + num12) * point.X + (1f - (num4 + num6)) * point.Y + (num9 - num10) * point.Z,
				(num8 - num11) * point.X + (num9 + num10) * point.Y + (1f - (num4 + num5)) * point.Z
			);
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return X;
			yield return Y;
			yield return Z;
			yield return W;
		}

		#endregion
	}
}