﻿using System;
using System.Runtime.InteropServices;

namespace Shintio.Essentials.Extensions
{
	public static class ObjectHandleExtensions
	{
		public static IntPtr ToIntPtr(this object target)
		{
			return GCHandle.Alloc(target).ToIntPtr();
		}

		public static GCHandle ToGcHandle(this object target)
		{
			return GCHandle.Alloc(target);
		}

		public static IntPtr ToIntPtr(this GCHandle target)
		{
			return GCHandle.ToIntPtr(target);
		}
		
		public static object? ToObject(this IntPtr intPtr)
		{
			return GCHandle.FromIntPtr(intPtr).Target;
		}
	}
}