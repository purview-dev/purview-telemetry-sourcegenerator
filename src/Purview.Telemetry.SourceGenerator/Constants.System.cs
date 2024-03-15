using Purview.Telemetry.SourceGenerator.Templates;

namespace Purview.Telemetry;
partial class Constants {
	static public class System {
		public const string VoidKeyword = "void";
		public const string ObjectKeyword = "object";

		public const string StringKeyword = "string";
		public const string BoolKeyword = "bool";
		public const string ByteKeyword = "byte";
		public const string ShortKeyword = "short";
		public const string IntKeyword = "int";
		public const string LongKeyword = "long";
		public const string FloatKeyword = "float";
		public const string DoubleKeyword = "double";
		public const string DecimalKeyword = "decimal";

		public const string NullKeyword = "null";
		public const string DefaultKeyword = "default";

		readonly static public TypeInfo Func = TypeInfo.Create("System.Func"); // <T>
		readonly static public TypeInfo Action = TypeInfo.Create("System.Action"); // <T>

		readonly static public TypeInfo String = TypeInfo.Create("System.String");
		readonly static public TypeInfo Boolean = TypeInfo.Create("System.Boolean");
		readonly static public TypeInfo Byte = TypeInfo.Create("System.Byte");
		readonly static public TypeInfo Int16 = TypeInfo.Create("System.Int16");
		readonly static public TypeInfo Int32 = TypeInfo.Create("System.Int32");
		readonly static public TypeInfo Int64 = TypeInfo.Create("System.Int64");
		readonly static public TypeInfo Single = TypeInfo.Create("System.Single");
		readonly static public TypeInfo Double = TypeInfo.Create("System.Double");
		readonly static public TypeInfo Decimal = TypeInfo.Create("System.Decimal");
		readonly static public TypeInfo DateTimeOffset = TypeInfo.Create("System.DateTimeOffset");

		readonly static public TypeInfo IEnumerable = TypeInfo.Create("System.Collections.Generic.IEnumerable"); // <>
		readonly static public TypeInfo List = TypeInfo.Create("System.Collections.Generic.List"); // <>
		readonly static public TypeInfo Dictionary = TypeInfo.Create("System.Collections.Generic.Dictionary"); // <>
		readonly static public TypeInfo ConcurrentDictionary = TypeInfo.Create("System.Collections.Concurrent.ConcurrentDictionary"); // <>

		readonly static public TypeInfo IDisposable = TypeInfo.Create("System.IDisposable");
		readonly static public TypeInfo Exception = TypeInfo.Create("System.Exception");

		readonly static public TypeInfo Lazy = TypeInfo.Create("System.Lazy"); // <T>
	}
}
