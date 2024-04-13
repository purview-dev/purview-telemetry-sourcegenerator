using System.Runtime.CompilerServices;

namespace Purview.Telemetry.SourceGenerator;

#if NET7_0_OR_GREATER

public static class ModuleInitialiser
{
	[ModuleInitializer]
	public static void Init()
	{
		VerifySourceGenerators.Initialize();
	}
}

#endif
