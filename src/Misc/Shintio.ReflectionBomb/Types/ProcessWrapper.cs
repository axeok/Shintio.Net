using System;
using System.Diagnostics;
using System.Reflection;
using Shintio.ReflectionBomb.Utils;

namespace Shintio.ReflectionBomb.Types
{
	public class ProcessWrapper
	{
		public static readonly Type ProcessType =
			AppDomainWrapper.GetAssembly("System." + "Diagnostics" + ".Process")!
				.GetNativeType("System." + "Diagnostics" + ".Process")!;

		private static readonly PropertyInfo StartInfoProperty = ProcessType.GetProperty("StartInfo")!;
		private static readonly PropertyInfo StandardInputProperty = ProcessType.GetProperty("StandardInput")!;

		private static readonly MethodInfo StartMethod = ProcessType.GetMethod("Start", new Type[] { })!;

		private static readonly MethodInfo StaticStartMethod =
			ProcessType.GetMethod("Start", new Type[] { typeof(ProcessStartInfo) })!;

		private static readonly MethodInfo WaitForExitMethod = ProcessType.GetMethod("WaitForExit", new Type[] { })!;

		private readonly object _process;

		public ProcessWrapper(object process)
		{
			_process = process;
		}

		public object Process => _process;

		public StreamWriterWrapper StandardInput =>
			new StreamWriterWrapper(StandardInputProperty.GetValue(_process, null));

		public void SetStartInfo(ProcessStartInfo startInfo)
		{
			StartInfoProperty.SetValue(this, startInfo);
		}

		public void Start()
		{
			StartMethod.Invoke(_process, new object[] { });
		}

		public void WaitForExit()
		{
			WaitForExitMethod.Invoke(_process, new object[] { });
		}

		public static ProcessWrapper Start(ProcessStartInfo startInfo)
		{
			return new ProcessWrapper(StaticStartMethod.Invoke(null, new object[] { startInfo }));
		}
	}
}