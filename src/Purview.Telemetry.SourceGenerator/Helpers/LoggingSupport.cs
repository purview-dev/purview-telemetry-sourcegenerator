namespace Purview.Telemetry.SourceGenerator.Helpers;

interface ILogSupport
{
	void SetLogOutput(Action<string, OutputType> action);
}

interface IGenerationLogger
{
	void Debug(string message);

	void Diagnostic(string message);

	void Warning(string message);

	void Error(string message);
}

sealed class Logger(Action<string, OutputType> logger) : IGenerationLogger
{
	public void Debug(string message) => logger(message, OutputType.Debug);

	public void Diagnostic(string message) => logger(message, OutputType.Diagnostic);

	public void Warning(string message) => logger(message, OutputType.Warning);

	public void Error(string message) => logger(message, OutputType.Error);
}

enum OutputType { Debug, Diagnostic, Warning, Error }
