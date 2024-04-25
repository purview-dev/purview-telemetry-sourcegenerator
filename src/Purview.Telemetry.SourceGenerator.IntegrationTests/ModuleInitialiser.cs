using System.Runtime.CompilerServices;

namespace Purview.Telemetry.SourceGenerator;

#if NET7_0_OR_GREATER

public static class ModuleInitialiser
{
	[ModuleInitializer]
	public static void Init()
	{
		DiffEngine.DiffRunner.MaxInstancesToLaunch(20);
		VerifySourceGenerators.Initialize();
	}
}

#endif
