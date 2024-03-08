using System.Runtime.CompilerServices;

namespace Purview.Telemetry.SourceGenerator;

static public class ModuleInitialiser {
	[ModuleInitializer]
	static public void Init() {
		VerifySourceGenerators.Initialize();
	}
}
