using System.Collections.Generic;
using Shintio.Essentials.Common;
using Shintio.Essentials.ValueObjects;

namespace Shintio.Essentials.Validation
{
	public class ValidationResult : ValueObject
	{
		public static readonly ValidationResult Success = new ValidationResult(true, string.Empty);

		public ValidationResult(bool isSuccess, LazyString message)
		{
			IsSuccess = isSuccess;
			Message = message;
		}

		public bool IsSuccess { get; }
		public LazyString Message { get; }

		public static implicit operator bool(ValidationResult result) => result.IsSuccess;

		public static ValidationResult Fail(LazyString message)
		{
			return new ValidationResult(false, message);
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return IsSuccess;
			yield return Message;
		}
	}

	public class ValidationResult<T> : ValueObject where T : class
	{
#if DEBUG
		public ValidationResult(bool isSuccess, LazyString message, T? data)
#else
		public ValidationResult(bool isSuccess, LazyString message, T data)
#endif
		{
			IsSuccess = isSuccess;
			Message = message;
			Data = data;
		}

		public bool IsSuccess { get; }
		public LazyString Message { get; }

#if DEBUG
		public T? Data { get; }
#else
		public T Data { get; }
#endif

		public static implicit operator bool(ValidationResult<T> result) => result.IsSuccess;
		public static implicit operator ValidationResult(ValidationResult<T> result) => new ValidationResult(result.IsSuccess, result.Message);

#if DEBUG
		public static ValidationResult<T> NotNull(T? data, LazyString message)
#else
		public static ValidationResult<T> NotNull(T data, LazyString message)
#endif
		{
			return new ValidationResult<T>(data != null, message, data);
		}

#if DEBUG
		public static ValidationResult<T> Null(T? data, LazyString message)
#else
		public static ValidationResult<T> Null(T data, LazyString message)
#endif
		{
			return new ValidationResult<T>(data == null, message, data);
		}

		public static ValidationResult<T> Fail(LazyString message)
		{
			return new ValidationResult<T>(false, message, null);
		}

		public static ValidationResult<T> Success(T data)
		{
			return new ValidationResult<T>(true, "", data);
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return IsSuccess;
			yield return Message;
			yield return Data;
		}
	}

	public class ValidationResult<T1, T2> : ValueObject where T1 : class where T2 : class
	{
#if DEBUG
		public ValidationResult(bool isSuccess, LazyString message, T1? data1, T2? data2)
#else
		public ValidationResult(bool isSuccess, LazyString message, T1 data1, T2 data2)
#endif
		{
			IsSuccess = isSuccess;
			Message = message;
			Data1 = data1;
			Data2 = data2;
		}

		public bool IsSuccess { get; }
		public LazyString Message { get; }

#if DEBUG
		public T1? Data1 { get; }
		public T2? Data2 { get; }
#else
		public T1 Data1 { get; }
		public T2 Data2 { get; }
#endif

		public static implicit operator bool(ValidationResult<T1, T2> result) => result.IsSuccess;

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return IsSuccess;
			yield return Message;
			yield return Data1;
			yield return Data2;
		}
	}

	public class ValidationResult<T1, T2, T3> : ValueObject where T1 : class where T2 : class where T3 : class
	{
#if DEBUG
		public ValidationResult(bool isSuccess, LazyString message, T1? data1, T2? data2, T3? data3)
#else
		public ValidationResult(bool isSuccess, LazyString message, T1 data1, T2 data2, T3 data3)
#endif
		{
			IsSuccess = isSuccess;
			Message = message;
			Data1 = data1;
			Data2 = data2;
			Data3 = data3;
		}

		public bool IsSuccess { get; }
		public LazyString Message { get; }

#if DEBUG
		public T1? Data1 { get; }
		public T2? Data2 { get; }
		public T3? Data3 { get; }
#else
		public T1 Data1 { get; }
		public T2 Data2 { get; }
		public T3 Data3 { get; }
#endif

		public static implicit operator bool(ValidationResult<T1, T2, T3> result) => result.IsSuccess;

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return IsSuccess;
			yield return Message;
			yield return Data1;
			yield return Data2;
			yield return Data3;
		}
	}
}