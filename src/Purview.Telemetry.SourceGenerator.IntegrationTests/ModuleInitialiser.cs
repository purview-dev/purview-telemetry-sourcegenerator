using System.Runtime.CompilerServices;

namespace Purview.Telemetry.SourceGenerator;

public static class ModuleInitialiser
{
	[ModuleInitializer]
	public static void Init()
	{
		DiffEngine.DiffRunner.MaxInstancesToLaunch(20);
		VerifySourceGenerators.Initialize();
	}
}
