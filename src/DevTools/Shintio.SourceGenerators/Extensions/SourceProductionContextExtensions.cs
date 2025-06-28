using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace Meta.SourceGenerator.Extensions;

public static class SourceProductionContextExtensions
{
	public static void Log(
		this SourceProductionContext context,
		DiagnosticSeverity level,
		string message,
		[CallerFilePath] string callerFilePath = null!
	)
	{
		var generator = Path.GetFileNameWithoutExtension(callerFilePath)!;

		var descriptor = new DiagnosticDescriptor(
			generator,
			message,
			message,
			generator,
			level,
			isEnabledByDefault: true
		);

		context.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None));
	}

	public static void LogInfo(
		this SourceProductionContext context,
		string message,
		[CallerFilePath] string callerFilePath = null!
	) => Log(context, DiagnosticSeverity.Info, message, callerFilePath);

	public static void LogWarning(
		this SourceProductionContext context,
		string message,
		[CallerFilePath] string callerFilePath = null!
	) => Log(context, DiagnosticSeverity.Warning, message, callerFilePath);

	public static void LogError(
		this SourceProductionContext context,
		string message,
		[CallerFilePath] string callerFilePath = null!
	) => Log(context, DiagnosticSeverity.Error, message, callerFilePath);
}