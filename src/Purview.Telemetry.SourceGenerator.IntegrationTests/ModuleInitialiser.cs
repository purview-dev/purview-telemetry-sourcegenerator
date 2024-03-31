using System.Runtime.CompilerServices;

namespace Purview.Telemetry.SourceGenerator;

#if NET7_0_OR_GREATER

static public class ModuleInitialiser {
	[ModuleInitializer]
	static public void Init() {
		VerifySourceGenerators.Initialize();
	}
}

#endif
