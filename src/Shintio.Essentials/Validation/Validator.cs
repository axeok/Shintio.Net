using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.Essentials.Extensions;
using Shintio.Essentials.ValueObjects;

namespace Shintio.Essentials.Validation
{
	public class Validator : IDisposable
	{
		private bool? _isSuccess;
		protected readonly List<Delegate> Validations;

		public Validator()
		{
			Validations = new List<Delegate>();
		}

		public Validator(IEnumerable<Delegate> validations)
		{
			Validations = validations.ToList();
		}

		public void Dispose()
		{
		}

		public bool IsSuccess => _isSuccess ??= Check();
		public int Checks { get; private set; } = 0;
		public LazyString Message { get; protected set; } = "";
		public ValidationResult Result => GetResult();

		public Validator Then(Func<ValidationResult> validation)
		{
			Validations.Add(validation);

			return this;
		}

		public Validator Then(Func<bool> validation, LazyString message)
		{
			Validations.Add(new Func<ValidationResult>(() => new ValidationResult(validation.Invoke(), message)));

			return this;
		}

		#region ToNext

		public Validator<T> Then<T>(Func<ValidationResult<T>> validation) where T : class
		{
			return new Validator<T>(Validations.With(validation));
		}

		public Validator<T1, T2> Then<T1, T2>(Func<ValidationResult<T1, T2>> validation)
			where T1 : class where T2 : class
		{
			return new Validator<T1, T2>(Validations.With(validation));
		}

		public Validator<T1, T2, T3> Then<T1, T2, T3>(Func<ValidationResult<T1, T2, T3>> validation)
			where T1 : class where T2 : class where T3 : class
		{
			return new Validator<T1, T2, T3>(Validations.With(validation));
		}

		#endregion

		public bool Check()
		{
			if (_isSuccess.HasValue)
			{
				return _isSuccess.Value;
			}
			
			foreach (var validation in Validations)
			{
				Checks++;
				if (!RunValidation(validation))
				{
					_isSuccess = false;
					return false;
				}
			}

			_isSuccess = true;
			return true;
		}

		public ValidationResult GetResult()
		{
			return new ValidationResult(IsSuccess, Message);
		}

		protected virtual bool RunValidation(Delegate validation)
		{
			if (validation is Func<ValidationResult> func)
			{
				return SetResult(func.Invoke());
			}

			return false;
		}

		private bool SetResult(ValidationResult result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			return true;
		}
	}

	public class Validator<T> : Validator where T : class
	{
		public Validator(IEnumerable<Delegate> validations) : base(validations)
		{
		}

		public new ValidationResult<T> GetResult()
		{
			return new ValidationResult<T>(IsSuccess, Message, Data);
		}

#if DEBUG
		public T? Data { get; private set; }
#else
		public T Data { get; private set; }
#endif

		#region Self

		public Validator<T> Then(Func<T, ValidationResult> validation)
		{
			Validations.Add(validation);
			return this;
		}
		
		public Validator<T> Then(Func<T, bool> validation, LazyString message)
		{
			Validations.Add(new Func<T, ValidationResult>(data => new ValidationResult(validation.Invoke(data), message)));
			return this;
		}

		#endregion

		#region ToNext

		public new Validator<T, T2> Then<T2>(Func<ValidationResult<T2>> validation)
			where T2 : class
		{
			return new Validator<T, T2>(Validations.With(validation));
		}

		public new Validator<T, T2, T3> Then<T2, T3>(Func<ValidationResult<T2, T3>> validation)
			where T2 : class where T3 : class
		{
			return new Validator<T, T2, T3>(Validations.With(validation));
		}

		#endregion

		protected override bool RunValidation(Delegate validation) => validation switch
		{
			Func<ValidationResult<T>> func => SetResult(func.Invoke()),
			Func<T, ValidationResult<T>> func => SetResult(func.Invoke(Data!)),
			Func<T, ValidationResult> func => SetResult(func.Invoke(Data!)),
			_ => base.RunValidation(validation),
		};

		private bool SetResult(ValidationResult<T> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data = result.Data;

			return true;
		}

		private bool SetResult(ValidationResult result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			return true;
		}
	}

	public class Validator<T1, T2> : Validator where T1 : class where T2 : class
	{
		public Validator(IEnumerable<Delegate> validations) : base(validations)
		{
		}

		public new ValidationResult<T1, T2> GetResult()
		{
			return new ValidationResult<T1, T2>(IsSuccess, Message, Data1, Data2);
		}

#if DEBUG
		public T1? Data1 { get; private set; }
		public T2? Data2 { get; private set; }
#else
		public T1 Data1 { get; private set; }
		public T2 Data2 { get; private set; }
#endif

		#region Self

		#region T1

		public Validator<T1, T2> Then(Func<T1, ValidationResult> validation)
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T1Generic

		public Validator<T1, T2> Then<T>(Func<T1, ValidationResult> validation) where T : T1
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T2Generic

		public Validator<T1, T2> Then<T>(Func<T2, ValidationResult> validation) where T : T2
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T1&T2

		public Validator<T1, T2> Then(Func<T1, T2, ValidationResult> validation)
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region ToNext

		public new Validator<T1, T2, T3> Then<T3>(Func<ValidationResult<T3>> validation)
			where T3 : class
		{
			return new Validator<T1, T2, T3>(Validations.With(validation));
		}

		#endregion

		#endregion

		protected override bool RunValidation(Delegate validation) => validation switch
		{
			Func<ValidationResult<T1>> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult<T1>> func => SetResult(func.Invoke(Data1!)),

			Func<ValidationResult<T1, T2>> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult<T1, T2>> func => SetResult(func.Invoke(Data1!)),
			Func<T2, ValidationResult<T1, T2>> func => SetResult(func.Invoke(Data2!)),
			Func<T1, T2, ValidationResult<T1, T2>> func => SetResult(func.Invoke(Data1!, Data2!)),
			Func<ValidationResult<T2>> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult<T2>> func => SetResult(func.Invoke(Data1!)),
			Func<T2, ValidationResult<T2>> func => SetResult(func.Invoke(Data2!)),
			Func<T1, T2, ValidationResult<T2>> func => SetResult(func.Invoke(Data1!, Data2!)),

			Func<ValidationResult> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult> func => SetResult(func.Invoke(Data1!)),
			Func<T2, ValidationResult> func => SetResult(func.Invoke(Data2!)),
			Func<T1, T2, ValidationResult> func => SetResult(func.Invoke(Data1!, Data2!)),
			_ => base.RunValidation(validation),
		};

		#region SetResult

		private bool SetResult(ValidationResult result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			return true;
		}

		private bool SetResult(ValidationResult<T1> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data1 = result.Data;

			return true;
		}

		private bool SetResult(ValidationResult<T2> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data2 = result.Data;

			return true;
		}

		private bool SetResult(ValidationResult<T1, T2> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data1 = result.Data1;
			Data2 = result.Data2;

			return true;
		}

		#endregion
	}

	public class Validator<T1, T2, T3> : Validator where T1 : class where T2 : class where T3 : class
	{
		public Validator(IEnumerable<Delegate> validations) : base(validations)
		{
		}

		public new ValidationResult<T1, T2, T3> GetResult()
		{
			return new ValidationResult<T1, T2, T3>(IsSuccess, Message, Data1, Data2, Data3);
		}

#if DEBUG
		public T1? Data1 { get; private set; }
		public T2? Data2 { get; private set; }
		public T3? Data3 { get; private set; }
#else
		public T1 Data1 { get; private set; }
		public T2 Data2 { get; private set; }
		public T3 Data3 { get; private set; }
#endif

		#region Self

		#region T1

		public Validator<T1, T2, T3> Then(Func<T1, ValidationResult> validation)
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T1Generic

		public Validator<T1, T2, T3> Then<T>(Func<T1, ValidationResult> validation) where T : T1
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T2Generic

		public Validator<T1, T2, T3> Then<T>(Func<T2, ValidationResult> validation) where T : T2
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T3Generic

		public Validator<T1, T2, T3> Then<T>(Func<T3, ValidationResult> validation) where T : T3
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T1&T2

		public Validator<T1, T2, T3> Then(Func<T1, T2, ValidationResult> validation)
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T1&T3Generic

		public Validator<T1, T2, T3> Then<T>(Func<T1, T3, ValidationResult> validation) where T : T3
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T2&T3Generic

		public Validator<T1, T2, T3> Then<TX2, TX3>(Func<T2, T3, ValidationResult> validation)
			where TX2 : T2 where TX3 : T3
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#region T1&T2&T3

		public Validator<T1, T2, T3> Then(Func<T1, T2, T3, ValidationResult> validation)
		{
			Validations.Add(validation);
			return this;
		}

		#endregion

		#endregion

		protected override bool RunValidation(Delegate validation) => validation switch
		{
			Func<ValidationResult<T1>> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult<T1>> func => SetResult(func.Invoke(Data1!)),

			Func<ValidationResult<T1, T2>> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult<T1, T2>> func => SetResult(func.Invoke(Data1!)),
			Func<T2, ValidationResult<T1, T2>> func => SetResult(func.Invoke(Data2!)),
			Func<T1, T2, ValidationResult<T1, T2>> func => SetResult(func.Invoke(Data1!, Data2!)),
			Func<ValidationResult<T2>> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult<T2>> func => SetResult(func.Invoke(Data1!)),
			Func<T2, ValidationResult<T2>> func => SetResult(func.Invoke(Data2!)),
			Func<T1, T2, ValidationResult<T2>> func => SetResult(func.Invoke(Data1!, Data2!)),

			Func<ValidationResult<T1, T2, T3>> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult<T1, T2, T3>> func => SetResult(func.Invoke(Data1!)),
			Func<T2, ValidationResult<T1, T2, T3>> func => SetResult(func.Invoke(Data2!)),
			Func<T3, ValidationResult<T1, T2, T3>> func => SetResult(func.Invoke(Data3!)),
			Func<T1, T2, ValidationResult<T1, T2, T3>> func => SetResult(func.Invoke(Data1!, Data2!)),
			Func<T1, T3, ValidationResult<T1, T2, T3>> func => SetResult(func.Invoke(Data1!, Data3!)),
			Func<T2, T3, ValidationResult<T1, T2, T3>> func => SetResult(func.Invoke(Data2!, Data3!)),
			Func<T1, T2, T3, ValidationResult<T1, T2, T3>> func => SetResult(func.Invoke(Data1!, Data2!, Data3!)),
			Func<ValidationResult<T2, T3>> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult<T2, T3>> func => SetResult(func.Invoke(Data1!)),
			Func<T2, ValidationResult<T2, T3>> func => SetResult(func.Invoke(Data2!)),
			Func<T3, ValidationResult<T2, T3>> func => SetResult(func.Invoke(Data3!)),
			Func<T1, T2, ValidationResult<T2, T3>> func => SetResult(func.Invoke(Data1!, Data2!)),
			Func<T1, T3, ValidationResult<T2, T3>> func => SetResult(func.Invoke(Data1!, Data3!)),
			Func<T2, T3, ValidationResult<T2, T3>> func => SetResult(func.Invoke(Data2!, Data3!)),
			Func<T1, T2, T3, ValidationResult<T2, T3>> func => SetResult(func.Invoke(Data1!, Data2!, Data3!)),
			Func<ValidationResult<T3>> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult<T3>> func => SetResult(func.Invoke(Data1!)),
			Func<T2, ValidationResult<T3>> func => SetResult(func.Invoke(Data2!)),
			Func<T3, ValidationResult<T3>> func => SetResult(func.Invoke(Data3!)),
			Func<T1, T2, ValidationResult<T3>> func => SetResult(func.Invoke(Data1!, Data2!)),
			Func<T1, T3, ValidationResult<T3>> func => SetResult(func.Invoke(Data1!, Data3!)),
			Func<T2, T3, ValidationResult<T3>> func => SetResult(func.Invoke(Data2!, Data3!)),
			Func<T1, T2, T3, ValidationResult<T3>> func => SetResult(func.Invoke(Data1!, Data2!, Data3!)),

			Func<ValidationResult> func => SetResult(func.Invoke()),
			Func<T1, ValidationResult> func => SetResult(func.Invoke(Data1!)),
			Func<T2, ValidationResult> func => SetResult(func.Invoke(Data2!)),
			Func<T3, ValidationResult> func => SetResult(func.Invoke(Data3!)),
			Func<T1, T2, ValidationResult> func => SetResult(func.Invoke(Data1!, Data2!)),
			Func<T1, T3, ValidationResult> func => SetResult(func.Invoke(Data1!, Data3!)),
			Func<T2, T3, ValidationResult> func => SetResult(func.Invoke(Data2!, Data3!)),
			Func<T1, T2, T3, ValidationResult> func => SetResult(func.Invoke(Data1!, Data2!, Data3!)),

			_ => base.RunValidation(validation),
		};

		#region SetResult

		private bool SetResult(ValidationResult result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			return true;
		}

		private bool SetResult(ValidationResult<T1> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data1 = result.Data;

			return true;
		}

		private bool SetResult(ValidationResult<T2> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data2 = result.Data;

			return true;
		}

		private bool SetResult(ValidationResult<T3> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data3 = result.Data;

			return true;
		}

		private bool SetResult(ValidationResult<T1, T2> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data1 = result.Data1;
			Data2 = result.Data2;

			return true;
		}

		private bool SetResult(ValidationResult<T1, T3> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data1 = result.Data1;
			Data3 = result.Data2;

			return true;
		}

		private bool SetResult(ValidationResult<T2, T3> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data2 = result.Data1;
			Data3 = result.Data2;

			return true;
		}

		private bool SetResult(ValidationResult<T1, T2, T3> result)
		{
			if (!result.IsSuccess)
			{
				Message = result.Message;
				return false;
			}

			Data1 = result.Data1;
			Data2 = result.Data2;
			Data3 = result.Data3;

			return true;
		}

		#endregion
	}
}