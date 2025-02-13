using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using Xunit.Sdk;

namespace Purview.Telemetry.SourceGenerator.MessageTemplates;

public partial class RegexValidationTests
{
	[Theory]
	[MemberData(nameof(OrdinalTests))]
	public void Match_GivenOrdinals_MatchesOrdinalCount(string template, [NotNull] TestMessageTemplateHole[] holes)
	{
		// Arrange/ Act
		var matches = Constants.MessageTemplateMatcher.Matches(template);

		// Assert
		matches.Count.ShouldBe(holes.Length);
		for (var i = 0; i < matches.Count; i++)
		{
			Match match = matches[i];
			match.Success.ShouldBeTrue();

			var hole = holes[i];

			var result = TestMessageTemplateHole.FromMatch(match);

			hole.ShouldBe(result);
		}
	}

	[Theory]
	[MemberData(nameof(NamedTests))]
	public void Match_GivenNames_MatchesNameCount(string template, [NotNull] TestMessageTemplateHole[] holes)
	{
		// Arrange/ Act
		var matches = Constants.MessageTemplateMatcher.Matches(template);

		// Assert
		matches.Count.ShouldBe(holes.Length);
		for (var i = 0; i < matches.Count; i++)
		{
			Match match = matches[i];
			match.Success.ShouldBeTrue();

			var hole = holes[i];

			var result = TestMessageTemplateHole.FromMatch(match);

			hole.ShouldBe(result);
		}
	}

	public static TheoryData<string, TestMessageTemplateHole[]> OrdinalTests
	{
		get
		{
			TheoryData<string, TestMessageTemplateHole[]> data = new()
			{
				{ "Customer with ID {0}", [ TestMessageTemplateHole.Create(0)] },
				{
					"Customer with ID {0} is named {1}",
					[
						TestMessageTemplateHole.Create(0),
						TestMessageTemplateHole.Create(1)
					]
				}
			};

			return data;
		}
	}

	public static TheoryData<string, TestMessageTemplateHole[]> NamedTests
	{
		get
		{
			TheoryData<string, TestMessageTemplateHole[]> data = new()
			{
				{ "Customer with ID {CustomerId}", [ TestMessageTemplateHole.Create("CustomerId")] },
				{
					"Customer with ID {CustomerId} is named {CustomerName}",
					[
						TestMessageTemplateHole.Create("CustomerId"),
						TestMessageTemplateHole.Create("CustomerName")
					]
				}
			};

			return data;
		}
	}

	public sealed record TestMessageTemplateHole : IXunitSerializable
	{
		public TestMessageTemplateHole(
			string? name,
			int? ordinal,
			string? alignment,
			string? format,
			bool destructure,
			bool stringify)
		{
			Name = name;
			Ordinal = ordinal;
			Alignment = alignment;
			Format = format;
			Destructure = destructure;
			Stringify = stringify;

			if (Name == null && Ordinal == null)
				throw new Exception("Name and Ordinal cannot both be null.");
			if (Name != null && Ordinal != null)
				throw new Exception("Name and Ordinal cannot both be set.");
		}

		public TestMessageTemplateHole()
		{
		}

		public bool IsPositional => Name is null;

		public string? Name { get; private set; }

		public int? Ordinal { get; private set; }

		public string? Alignment { get; private set; }

		public string? Format { get; private set; }

		public bool Destructure { get; private set; }

		public bool Stringify { get; private set; }

		public static TestMessageTemplateHole Create(int ordinal, string? alignment = null, string? format = null, bool destructure = false, bool stringify = false)
			=> new(null, ordinal, alignment, format, destructure, stringify);

		public static TestMessageTemplateHole Create(string name, string? alignment = null, string? format = null, bool destructure = false, bool stringify = false)
			=> new(name, null, alignment, format, destructure, destructure);

		public static TestMessageTemplateHole FromMatch([NotNull] Match match)
		{
			if (!match.Success)
				throw new ArgumentException("Match must be successful.", nameof(match));

			string? name = null;
			string? ordinal = null;
			string? alignment = null;
			string? format = null;
			var destructure = match.Groups["destructure"].Success;
			var stringify = match.Groups["stringify"].Success;

			if (match.Groups["named"].Success)
				name = match.Groups["named"].Value;
			if (match.Groups["ordinal"].Success)
				ordinal = match.Groups["ordinal"].Value;
			if (match.Groups["alignment"].Success)
				alignment = match.Groups["alignment"].Value;

			return new(name,
				ordinal == null
					? null
					: int.Parse(ordinal, CultureInfo.InvariantCulture),
				alignment,
				format,
				destructure,
				stringify);
		}

		public void Deserialize(IXunitSerializationInfo info)
		{
			Name = info.GetValue<string?>("Name");
			Ordinal = info.GetValue<int?>("Ordinal");
			Alignment = info.GetValue<string?>("Alignment");
			Format = info.GetValue<string?>("Format");
			Destructure = info.GetValue<bool>("Destructure");
			Stringify = info.GetValue<bool>("Stringify");
		}

		public void Serialize(IXunitSerializationInfo info)
		{
			info.AddValue("Name", Name);
			info.AddValue("Ordinal", Ordinal);
			info.AddValue("Alignment", Alignment);
			info.AddValue("Format", Format);
			info.AddValue("Destructure", Destructure);
			info.AddValue("Stringify", Stringify);
		}
	}
}
