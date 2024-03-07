namespace Purview.Telemetry.SourceGenerator.Samples;

[LogTarget]
public interface ILogSample {
	void Basic();

	IDisposable? BasicWithScopeNullable();

	IDisposable BasicWithScope();

	void WithParameters(string message, int value);

	void WithException(Exception ex);

	[Entry(LogGeneratedLevel.Info)]
	void ExplicitBasic();

	[Entry(LogGeneratedLevel.Info)]
	IDisposable? ExplicitBasicWithScopeNullable();

	[Entry(LogGeneratedLevel.Info)]
	IDisposable ExplicitBasicWithScope();

	[Entry(LogGeneratedLevel.Debug)]
	void ExplicitWithParameters(string message, int value);

	[Entry(LogGeneratedLevel.Info)]
	void ExplicitWithException(Exception ex);

	// Generates a diagnostic error > 6 parameters.
	void MoreThan6Parameters(int one, int two, int three, int four, int five, int six, int seven);

	// This does not... 6 + 7th is an Exception
	void MoreThan6Parameters(int one, int two, int three, int four, int five, int six, Exception seven);
}
