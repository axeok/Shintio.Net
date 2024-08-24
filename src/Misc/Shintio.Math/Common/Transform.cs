using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Shintio.Essentials.Common;
using Shintio.Json.Attributes;
using Shintio.Math.Common.Enums;
using Shintio.Math.Utils;

namespace Shintio.Math.Common
{
	public class Transform : ValueObject
	{
		public static readonly Transform Zero = new Transform(Vector3.Zero, Vector3.Zero);

		[JsonConstructor]
		public Transform(Vector3 position, Vector3 rotation) => (Position, Rotation) = (position, rotation);

		public Transform(Vector3 position, Quaternion quaternion) =>
			(Position, Rotation) = (position, quaternion.ToEuler());

		public Transform(Vector3 position) => (Position, Rotation) = (position, Vector3.Zero);
		public Transform(float x, float y, float z) => (Position, Rotation) = (new Vector3(x, y, z), Vector3.Zero);
		public Transform(double x, double y, double z) => (Position, Rotation) = (new Vector3(x, y, z), Vector3.Zero);

		public Transform(Vector3 position, float heading) =>
			(Position, Rotation) = (position, new Vector3(0, 0, heading));
		
		public Transform(Vector3 position, double heading) =>
			(Position, Rotation) = (position, new Vector3(0, 0, heading));

		public Transform(float x, float y, float z, float heading) => (Position, Rotation) =
			(new Vector3(x, y, z), new Vector3(0, 0, heading));
		
		public Transform(double x, double y, double z, double heading) => (Position, Rotation) =
			(new Vector3(x, y, z), new Vector3(0, 0, heading));

		public Transform(float x, float y, float z, float rX, float rY, float rZ) => (Position, Rotation) =
			(new Vector3(x, y, z), new Vector3(rX, rY, rZ));
		
		public Transform(double x, double y, double z, double rX, double rY, double rZ) => (Position, Rotation) =
			(new Vector3(x, y, z), new Vector3(rX, rY, rZ));
		
		public static Transform operator +(Transform a, Transform b) => 
			new Transform(a.Position + b.Position, a.Rotation + b.Rotation);
		
		public static Transform operator -(Transform a, Transform b) => 
			new Transform(a.Position - b.Position, a.Rotation - b.Rotation);
		

		// For EF
		protected Transform()
		{
		}

		public Vector3 Position { get; private set; } = Vector3.Zero;
		public Vector3 Rotation { get; private set; } = Vector3.Zero;

		[JsonIgnore] [NotMapped] public Quaternion Quaternion => Quaternion.FromEuler(Rotation);

		[JsonIgnore] [NotMapped] public Vector3 Forward => Quaternion * Vector3.Forward;
		[JsonIgnore] [NotMapped] public Vector3 Up => Quaternion * Vector3.Up;
		[JsonIgnore] [NotMapped] public Vector3 Right => Quaternion * Vector3.Right;

		[JsonIgnore] [NotMapped] public float Heading => Rotation.Z;

		public override string ToString()
		{
			return $"({Position}, {Rotation})";
		}

		// public void Deconstruct(out Vector3 position, out Vector3 rotation)
		// {
		// 	position = Position;
		// 	rotation = Rotation;
		// }

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Position;
			yield return Rotation;
		}

		public static void Initialize()
		{
			// Inits static properties.
		}

		public static Transform Deserialize(string str)
		{
			var data = str.Split(";")
				.Select(str => string.IsNullOrEmpty(str) ? default(float) : Convert.ToSingle(str))
				.ToArray();

			return new Transform(
				new Vector3(data[0], data[1], data[2]),
				new Vector3(data[3], data[4], data[5])
			);
		}

		public string Serialize()
		{
			return $"{IgnoreDefault(Position.X)};" +
			       $"{IgnoreDefault(Position.Y)};" +
			       $"{IgnoreDefault(Position.Z)};" +
			       $"{IgnoreDefault(Rotation.X)};" +
			       $"{IgnoreDefault(Rotation.Y)};" +
			       $"{IgnoreDefault(Rotation.Z)}";
		}

		private string IgnoreDefault(float value)
		{
			return value != 0 ? $"{Convert.ToSingle(value)}" : "";
		}

		public static Transform Rotate(Transform transform, Vector3 axis, float angle, Space relativeTo)
		{
			var rotation = Quaternion.AngleAxis(angle, axis);

			var quaternion = relativeTo == Space.World
				? rotation * transform.Quaternion
				: transform.Quaternion * rotation;

			return new Transform(transform.Position, quaternion);
		}

		public static Transform Lerp(Transform a, Transform b, float t)
		{
			return LerpUnclamped(a, b, Mathf.Clamp01(t));
		}

		public static Transform LerpUnclamped(Transform a, Transform b, float t)
		{
			return new Transform(
				Vector3.LerpUnclamped(a.Position, b.Position, t),
				Vector3.LerpAngleUnclamped(a.Rotation, b.Rotation, t)
			);
		}
	}
}