using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.Math.Common;

namespace Shintio.Math.Utils
{
	public static class Mathf
	{
		// The infamous ''3.14159265358979...'' value (RO).
		public const float PI = MathF.PI;

		// A representation of positive infinity (RO).
		public const float Infinity = float.PositiveInfinity;

		// A representation of negative infinity (RO).
		public const float NegativeInfinity = float.NegativeInfinity;

		// Degrees-to-radians conversion constant (RO).
		public const float Deg2Rad = PI * 2F / 360F;

		// Radians-to-degrees conversion constant (RO).
		public const float Rad2Deg = 1F / Deg2Rad;

		// We cannot round to more decimals than 15 according to docs for System.MathF.Round.
		private const int kMaxDecimals = 15;

		// A tiny floating point value (RO).
		public static readonly float Epsilon = float.Epsilon;
		
		public static float Sin(float f) => MathF.Sin(f);

		public static float Cos(float f) => MathF.Cos(f);

		public static float Tan(float f) => MathF.Tan(f);

		public static float Asin(float f) => MathF.Asin(f);

		public static float Acos(float f) => MathF.Acos(f);

		public static float Atan(float f) => MathF.Atan(f);

		public static float Atan2(float y, float x) => MathF.Atan2(y, x);

		public static float Sqrt(float f) => MathF.Sqrt(f);

		public static float HypotSquared(params float[] values) => values.Sum(v => v * v);
		public static float Hypot(params float[] values) => MathF.Sqrt(values.Sum(v => v * v));

		public static float Abs(float f) => MathF.Abs(f);
		public static Vector2 Abs(Vector2 f) => new Vector2(Abs(f.X), Abs(f.Y));

		public static float Min(float a, float b) => MathF.Min(a, b);

		public static float Min(params float[] values)
		{
			var count = values.Length;
			if (count == 0)
			{
				return 0;
			}

			var result = values[0];
			for (var i = 1; i < count; i++)
			{
				if (values[i] < result)
					result = values[i];
			}

			return result;
		}

		public static float Min(int a, int b) => MathF.Min(a, b);

		public static float Min(params int[] values)
		{
			var count = values.Length;
			if (count == 0)
			{
				return 0;
			}

			var result = values[0];
			for (var i = 1; i < count; i++)
			{
				if (values[i] < result)
					result = values[i];
			}

			return result;
		}
		
		public static DateTime Min(DateTime first, DateTime second)
		{
			return new DateTime(System.Math.Min(first.Ticks, second.Ticks));
		}

		public static float Max(float a, float b) => MathF.Max(a, b);

		public static float Max(params float[] values)
		{
			var count = values.Length;
			if (count == 0)
			{
				return 0;
			}

			var result = values[0];
			for (var i = 1; i < count; i++)
			{
				if (values[i] > result)
					result = values[i];
			}

			return result;
		}

		public static float Max(int a, int b) => MathF.Max(a, b);

		public static float Max(params int[] values)
		{
			var count = values.Length;
			if (count == 0)
			{
				return 0;
			}

			var result = values[0];
			for (var i = 1; i < count; i++)
			{
				if (values[i] > result)
					result = values[i];
			}

			return result;
		}
		
		public static DateTime Max(DateTime first, DateTime second)
		{
			return new DateTime(System.Math.Max(first.Ticks, second.Ticks));
		}

		// Returns /f/ raised to power /p/.
		public static float Pow(float f, float p)
		{
			return MathF.Pow(f, p);
		}

		// Returns e raised to the specified power.
		public static float Exp(float power)
		{
			return MathF.Exp(power);
		}

		// Returns the logarithm of a specified number in a specified base.
		public static float Log(float f, float p)
		{
			return MathF.Log(f, p);
		}

		// Returns the natural (base e) logarithm of a specified number.
		public static float Log(float f)
		{
			return MathF.Log(f);
		}

		// Returns the base 10 logarithm of a specified number.
		public static float Log10(float f)
		{
			return MathF.Log10(f);
		}

		// Returns the smallest integer greater to or equal to /f/.
		public static float Ceil(float f)
		{
			return MathF.Ceiling(f);
		}

		// Returns the largest integer smaller to or equal to /f/.
		public static float Floor(float f)
		{
			return MathF.Floor(f);
		}

		// Returns /f/ rounded to the nearest integer.
		public static float Round(float f)
		{
			return MathF.Round(f);
		}

		// Returns the smallest integer greater to or equal to /f/.
		public static int CeilToInt(float f)
		{
			return (int)MathF.Ceiling(f);
		}

		// Returns the largest integer smaller to or equal to /f/.
		public static int FloorToInt(float f)
		{
			return (int)MathF.Floor(f);
		}

		// Returns /f/ rounded to the nearest integer.
		public static int RoundToInt(float f)
		{
			return (int)MathF.Round(f);
		}

		// Returns the sign of /f/.
		public static float Sign(float f)
		{
			return f >= 0F ? 1F : -1F;
		}

		// Clamps value between min and max and returns value.
		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		// Clamps value between min and max and returns value.
		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
				value = min;
			else if (value > max)
				value = max;
			return value;
		}

		// Clamps value between 0 and 1 and returns value.
		public static float Clamp01(float value)
		{
			if (value < 0F)
				return 0F;
			if (value > 1F)
				return 1F;
			return value;
		}
		
		// Clamps enum value between min and max values of this enum and returns value.
		public static TEnum Clamp<TEnum>(TEnum value) where TEnum : struct, Enum
		{
#if NETCOREAPP3_0_OR_GREATER
			var values = Enum.GetValues<TEnum>();
#else
			var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
#endif

			var minValue = values.First();
			var maxValue = values.Last();
			var comparer = Comparer<TEnum>.Default;
            
			if (comparer.Compare(value, minValue) < 0)
			{
				return minValue;
			}
            
			if (comparer.Compare(value, maxValue) > 0)
			{
				return maxValue;
			}

			return value;
		}

		// Interpolates between /a/ and /b/ by /t/. /t/ is clamped between 0 and 1.
		public static float Lerp(float a, float b, float t)
		{
			return a + (b - a) * Clamp01(t);
		}

		// Interpolates between /a/ and /b/ by /t/ without clamping t.
		public static float LerpUnclamped(float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		// Same as ::ref::Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.
		public static float LerpAngle(float a, float b, float t)
		{
			var delta = Repeat((b - a), 360);
			if (delta > 180)
				delta -= 360;
			return a + delta * Clamp01(t);
		}

		// Same as ::ref::Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.
		public static float LerpAngleUnclamped(float a, float b, float t)
		{
			var delta = Repeat((b - a), 360);
			if (delta > 180)
				delta -= 360;
			return a + delta * t;
		}

		// Moves a value /current/ towards /target/.
		public static float MoveTowards(float current, float target, float maxDelta)
		{
			if (Abs(target - current) <= maxDelta)
				return target;
			return current + Sign(target - current) * maxDelta;
		}

		// Same as ::ref::MoveTowards but makes sure the values interpolate correctly when they wrap around 360 degrees.
		public static float MoveTowardsAngle(float current, float target, float maxDelta)
		{
			var deltaAngle = DeltaAngle(current, target);
			if (-maxDelta < deltaAngle && deltaAngle < maxDelta)
				return target;
			target = current + deltaAngle;
			return MoveTowards(current, target, maxDelta);
		}

		// Interpolates between /min/ and /max/ with smoothing at the limits.
		public static float SmoothStep(float from, float to, float t)
		{
			t = Clamp01(t);
			t = -2.0F * t * t * t + 3.0F * t * t;
			return to * t + from * (1F - t);
		}

		//*undocumented
		public static float Gamma(float value, float absMax, float gamma)
		{
			var negative = value < 0F;
			var absVal = Abs(value);
			if (absVal > absMax)
				return negative ? -absVal : absVal;

			var result = Pow(absVal / absMax, gamma) * absMax;
			return negative ? -result : result;
		}

		// Compares two floating point values if they are similar.
		public static bool Approximately(float a, float b)
		{
			// If a or b is zero, compare that the other is less or equal to epsilon.
			// If neither a or b are 0, then find an epsilon that is good for
			// comparing numbers at the maximum magnitude of a and b.
			// Floating points have about 7 significant digits, so
			// 1.000001f can be represented while 1.0000001f is rounded to zero,
			// thus we could use an epsilon of 0.000001f for comparing values close to 1.
			// We multiply this epsilon by the biggest magnitude of a and b.
			return Abs(b - a) < Max(0.000001f * Max(Abs(a), Abs(b)), Epsilon * 8);
		}

		// Gradually changes a value towards a desired goal over time.
		public static float SmoothDamp(
			float current,
			float target,
			ref float currentVelocity,
			float smoothTime,
			float maxSpeed,
			float deltaTime
		)
		{
			// Based on Game Programming Gems 4 Chapter 1.10
			smoothTime = Max(0.0001F, smoothTime);
			var omega = 2F / smoothTime;

			var x = omega * deltaTime;
			var exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);
			var change = current - target;
			var originalTo = target;

			// Clamp maximum speed
			var maxChange = maxSpeed * smoothTime;
			change = Clamp(change, -maxChange, maxChange);
			target = current - change;

			var temp = (currentVelocity + omega * change) * deltaTime;
			currentVelocity = (currentVelocity - omega * temp) * exp;
			var output = target + (change + temp) * exp;

			// Prevent overshooting
			if (originalTo - current > 0.0F == output > originalTo)
			{
				output = originalTo;
				currentVelocity = (output - originalTo) / deltaTime;
			}

			return output;
		}

		// Gradually changes an angle given in degrees towards a desired goal angle over time.
		public static float SmoothDampAngle(
			float current,
			float target,
			ref float currentVelocity,
			float smoothTime,
			float maxSpeed,
			float deltaTime
		)
		{
			target = current + DeltaAngle(current, target);
			return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
		}

		// Loops the value t, so that it is never larger than length and never smaller than 0.
		public static float Repeat(float t, float length)
		{
			return Clamp(t - Floor(t / length) * length, 0.0f, length);
		}
		
		public static int Repeat(int min, int max, int value)
		{
			if (value < min) {
				value = max;
			}

			if (value > max) {
				value = min;
			}

			return value;
		}

		// PingPongs the value t, so that it is never larger than length and never smaller than 0.
		public static float PingPong(float t, float length)
		{
			t = Repeat(t, length * 2F);
			return length - Abs(t - length);
		}

		// Calculates the ::ref::Lerp parameter between of two values.
		public static float InverseLerp(float a, float b, float value)
		{
			if (a != b)
				return Clamp01((value - a) / (b - a));
			return 0.0f;
		}

		// Calculates the shortest difference between two given angles.
		public static float DeltaAngle(float current, float target)
		{
			var delta = Repeat((target - current), 360.0F);
			if (delta > 180.0F)
				delta -= 360.0F;
			return delta;
		}

		private static float ClampToFloat(double value)
		{
			if (double.IsPositiveInfinity(value))
				return float.PositiveInfinity;

			if (double.IsNegativeInfinity(value))
				return float.NegativeInfinity;

			if (value < float.MinValue)
				return float.MinValue;

			if (value > float.MaxValue)
				return float.MaxValue;

			return (float)value;
		}

		private static int ClampToInt(long value)
		{
			if (value < int.MinValue)
				return int.MinValue;

			if (value > int.MaxValue)
				return int.MaxValue;

			return (int)value;
		}

		private static float RoundToMultipleOf(float value, float roundingValue)
		{
			if (roundingValue == 0)
				return value;
			return Round(value / roundingValue) * roundingValue;
		}

		private static float GetClosestPowerOfTen(float positiveNumber)
		{
			if (positiveNumber <= 0)
				return 1;
			return Pow(10, RoundToInt(Log10(positiveNumber)));
		}

		private static int GetNumberOfDecimalsForMinimumDifference(float minDifference)
		{
			return Clamp(-FloorToInt(Log10(Abs(minDifference))), 0, kMaxDecimals);
		}

		private static int GetNumberOfDecimalsForMinimumDifference(double minDifference)
		{
			return (int)System.Math.Max(0, -System.Math.Floor(System.Math.Log10(System.Math.Abs(minDifference))));
		}

		private static float RoundBasedOnMinimumDifference(float valueToRound, float minDifference)
		{
			if (minDifference == 0)
				return DiscardLeastSignificantDecimal(valueToRound);
			return MathF.Round(valueToRound, GetNumberOfDecimalsForMinimumDifference(minDifference),
				MidpointRounding.AwayFromZero);
		}

		private static double RoundBasedOnMinimumDifference(double valueToRound, double minDifference)
		{
			if (minDifference == 0)
				return DiscardLeastSignificantDecimal(valueToRound);
			return System.Math.Round(valueToRound, GetNumberOfDecimalsForMinimumDifference(minDifference),
				MidpointRounding.AwayFromZero);
		}

		private static float DiscardLeastSignificantDecimal(float v)
		{
			var decimals = Clamp((int)(5 - Log10(Abs(v))), 0, kMaxDecimals);
			return MathF.Round(v, decimals, MidpointRounding.AwayFromZero);
		}

		private static double DiscardLeastSignificantDecimal(double v)
		{
			var decimals = System.Math.Max(0, (int)(5 - System.Math.Log10(System.Math.Abs(v))));
			try
			{
				return System.Math.Round(v, decimals);
			}
			catch (ArgumentOutOfRangeException)
			{
				// This can happen for very small numbers.
				return 0;
			}
		}

		public static Vector3 RotateAroundZ(Vector3 vector, float zAngle)
		{
			zAngle *= Deg2Rad;

			var x = vector.X;
			var y = vector.Y;
			var z = vector.Z;

			var newX = x * Cos(zAngle) - y * Sin(zAngle);
			var newY = x * Sin(zAngle) + y * Cos(zAngle);

			return new Vector3(newX, newY, z);
		}
		
		public static Vector3 RotateAroundZ(Vector3 vector, Angle angle)
		{
			var x = vector.X;
			var y = vector.Y;
			var z = vector.Z;

			var newX = x * Cos(angle.Radians) - y * Sin(angle.Radians);
			var newY = x * Sin(angle.Radians) + y * Cos(angle.Radians);

			return new Vector3(newX, newY, z);
		}
		
		public static Vector3 RotateAroundY(Vector3 vector, Angle angle)
		{
			var x = vector.X;
			var y = vector.Y;
			var z = vector.Z;

			var newX = x * MathF.Cos(angle.Radians) + z * MathF.Sin(angle.Radians);
			var newZ = -x * MathF.Sin(angle.Radians) + z * MathF.Cos(angle.Radians);

			return new Vector3(newX, y, newZ);
		}

		public static Vector3 RotateAroundX(Vector3 vector, Angle angle)
		{
			var x = vector.X;
			var y = vector.Y;
			var z = vector.Z;

			var newY = y * MathF.Cos(angle.Radians) - z * MathF.Sin(angle.Radians);
			var newZ = y * MathF.Sin(angle.Radians) + z * MathF.Cos(angle.Radians);

			return new Vector3(x, newY, newZ);
		}
		
		public static Vector3 GetDirection(Vector3 rotation)
		{
			const float oneDeg = MathF.PI / 180;

			var num = rotation.Z * oneDeg;
			var num2 = rotation.X * oneDeg;
			var num3 = MathF.Abs(MathF.Cos(num2));

			return new Vector3(-MathF.Sin(num) * num3, MathF.Cos(num) * num3, MathF.Sin(num2));
		}

		public static Vector3 GetRotation(Vector3 direction)
		{
			const float oneRad = 180 / MathF.PI;

			var num = MathF.Atan2(direction.Y, direction.X) * oneRad;
			var num2 = MathF.Atan2(direction.Z, MathF.Sqrt(direction.X * direction.X + direction.Y * direction.Y)) *
			           oneRad;

			return new Vector3(-num2, 0, num);
		}

		public static Vector3 GetForwardVector(float heading)
		{
			var headingRadians = heading * Deg2Rad + PI / 2;

			return new Vector3(
				Cos(headingRadians),
				Sin(headingRadians),
				0
			);
		}

		public static Vector3 GetPositionFrontOfPos(Transform transform, float dist)
		{
			return GetPositionFrontOfPos(transform.Position, transform.Heading, dist);
		}
		
		public static Vector3 GetPositionFrontOfPos(Vector3 pos, float heading, float dist)
		{
			heading *= MathF.PI / 180;

			return new Vector3(
				pos.X + (dist * MathF.Sin(-heading)),
				pos.Y + (dist * MathF.Cos(-heading)),
				pos.Z
			);
		}

		public static bool IsPointWithinCircle(Vector2 point, Vector2 center, float radius = 1)
		{
			return MathF.Pow(point.X - center.X, 2) + MathF.Pow(point.Y - center.Y, 2) <= MathF.Pow(radius, 2);
		}
		
		public static bool IsPointWithinVerticalCylinder(Vector3 point, Vector3 bottomCenter, float radius, float height)
		{
			return point.Z >= bottomCenter.Z && point.Z <= bottomCenter.Z + height &&
			       IsPointWithinCircle(point, bottomCenter, radius);
		}

		public static float GetHeadingToPoint(Vector2 from, Vector2 to)
		{
			return GetHeadingToPoint(from.X, from.Y, to.X, to.Y);
		}
		
		public static float GetHeadingToPoint(float fromX, float fromY, float toX, float toY)
		{
			var x = toX - fromX;
			var y = toY - fromY;

			if (x == 0)
			{
				return y > 0 ? 180 : 0;
			}

			var heading = MathF.Atan(y / x) * Rad2Deg + (x > 0 ? 90 : 270);

			return heading > 180 ? heading - 180 : heading + 180;
		}

		public static Vector3 GetMidPoint(Vector3 pos1, Vector3 pos2)
		{
			return (pos1 + pos2) / 2;
		}
		
		public static Vector3 GetAverage(params Vector3[] positions)
		{
			var x = positions.Average(p => p.X);
			var y = positions.Average(p => p.Y);
			var z = positions.Average(p => p.Z);

			return new Vector3(x, y, z);
		}
		
		public static Vector2 GetAverage(params Vector2[] positions)
		{
			var x = positions.Average(p => p.X);
			var y = positions.Average(p => p.Y);

			return new Vector2(x, y);
		}

		public static uint GetPercentOrMax(uint value, uint percent, uint max)
		{
			return System.Math.Min((uint)(value / 100f * percent), max);
		}

		public static int GetValueFromPercents(int percents, int min, int max)
		{
			percents = Clamp(percents, 0, 100);

			return RoundToInt(Map(percents, 0, 100, min, max));
		}

		public static float Map(float value, float inMin, float inMax, float outMin, float outMax, bool clamped = true)
		{
			var res = (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
			return clamped ? System.Math.Clamp(res, outMin, outMax) : res;
		}
		
		public static float GetHeadingError(float initial, float final)
		{
			initial = (initial + 360) % 360;
			final = (final + 360) % 360;
    
			var diff = final - initial;
    
			if (MathF.Abs(diff) <= 180)
			{
				return diff;
			}
    
			return diff > 0 ? diff - 360 : diff + 360;
		}

		public static float GetAbsHeadingError(float first, float second)
		{
			return Abs(GetHeadingError(first, second));
		}

		public static float GetHeadingDifferenceTo(float from, float to)
		{
			var diff = (to - from + 180) % 360 - 180;

			return Mathf.Abs(diff < -180 ? diff + 360 : diff);
		}

		public static Vector2 GetPointOnCircle(float radius, float angleRad)
		{
			return new Vector2(
				radius * MathF.Sin(angleRad),
				radius * MathF.Cos(angleRad)
			);
		}
		
		public static Vector3 GetPointOnSphere(float radius, Vector2 anglesRad)
		{
			return new Vector3(
				radius * MathF.Sin(anglesRad.X),
				radius * MathF.Cos(anglesRad.X),
				radius * MathF.Cos(anglesRad.Y)
			);
		}

		public static float CalculateAngle(Vector2 point1, Vector2 point2)
		{
			return CalculateAngle(Vector2.Zero, point1, point2);
		}
		
		public static float CalculateAngle(Vector2 center, Vector2 point1, Vector2 point2)
		{
			var deltaX1 = point1.X - center.X;
			var deltaY1 = point1.Y - center.Y;
			var deltaX2 = point2.X - center.X;
			var deltaY2 = point2.Y - center.Y;
			
			var sin = deltaX1 * deltaY2 - deltaX2 * deltaY1;  
			var cos = deltaX1 * deltaX2 + deltaY1 * deltaY2;

			return Atan2(sin, cos) * (180f / PI);
		}
		
		public static int MapToClosestMultiple(int number, int multiple)
		{
			number += multiple / 2;
			number -= number % multiple;
			
			return number;
		}
		
		// Including 0.
		public static bool IsPowerOfTwo(int x)
		{
			return (x & (x - 1)) == 0;
		}
		
		public static void Spiral(int gridSizeX, int gridSizeY, Action<Vector2> callback)
		{
			int x = 0, y = 0, dx = 0, dy = -1;
			
			var t = System.Math.Max(gridSizeX, gridSizeY);
			var maxI = t * t;
			for (var i = 0; i < maxI; i++)
			{
				if ((-gridSizeX / 2 <= x) && (x <= gridSizeX / 2) && (-gridSizeY / 2 <= y) && (y <= gridSizeY / 2))
				{
					callback(new Vector2(x, y));
				}
				
				if ((x == y) || ((x < 0) && (x == -y)) || ((x > 0) && (x == 1 - y)))
				{
					t = dx;
					dx = -dy;
					dy = t;
				}
				
				x += dx;
				y += dy;
			}
		}
	}
}