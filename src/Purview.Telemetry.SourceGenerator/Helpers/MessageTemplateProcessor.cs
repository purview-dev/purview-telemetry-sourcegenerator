// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// This has been lifted and modified, with thanks!, from the https://github.com/dotnet/extensions repo.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace Purview.Telemetry.SourceGenerator.Helpers;

// properties:
// - Name - property name or ordinal.
// - Alignment? - padding alignment.
// - Format? - format string.
// - IsPositional - true if the property is positional, false if it is named.

/// <summary>
/// The Message Template format consists of a string with 'holes' in it.
/// Each hole is a named property, with optional formatting.
/// </summary>
static class MessageTemplateProcessor
{
	const int WrongBraceFound = -2;
	const int NoBracesFound = -1;

	static readonly char[] FormatDelimiters = [',', ':'];

	/// <summary>
	/// Finds the property arguments contained in the message template.
	/// 
	/// Template contain holes, holes contain properties and formatting information.
	/// </summary>
	public static bool ExtractProperties(string? message, out ImmutableArray<string> properties)
	{
		if (string.IsNullOrEmpty(message))
			return true;

		var scanIndex = 0;
		var endIndex = message!.Length;

		List<string>? messageProperties = null;
		var success = true;
		while (scanIndex < endIndex)
		{
			var openBraceIndex = FindBraceIndex(message, '{', scanIndex, endIndex);
			if (openBraceIndex == WrongBraceFound)
			{
				// found '}' instead of '{'
				success = false;
				break;
			}
			else if (openBraceIndex == NoBracesFound)
				// scanned the string and didn't find any remaining '{' or '}'
				break;

			var closeBraceIndex = FindBraceIndex(message, '}', openBraceIndex + 1, endIndex);
			if (closeBraceIndex < 0)
			{
				success = false;
				break;
			}

			// Format hole syntax : { index[,alignment][ :formatString] }.
			var formatDelimiterIndex = FindIndexOfAny(message, FormatDelimiters, openBraceIndex, closeBraceIndex);
			var propertyName = message.Substring(openBraceIndex + 1, formatDelimiterIndex - openBraceIndex - 1).Trim();

			if (string.IsNullOrWhiteSpace(propertyName))
			{
				// braces with no named argument, such as "{}" and "{ }"
				success = false;
				break;
			}

			messageProperties ??= [];
			messageProperties.Add(propertyName);

			scanIndex = closeBraceIndex + 1;
		}

		if (messageProperties != null)
			properties = [.. messageProperties];

		return success;
	}

	/// <summary>
	/// Allows replacing individual properties with different strings.
	/// </summary>
	public static string? MapProperties(string? template, Func<string, string> mapProperty)
	{
		if (string.IsNullOrEmpty(template))
			return template;

		StringBuilder sb = new();
		var scanIndex = 0;
		var endIndex = template!.Length;
		while (scanIndex < endIndex)
		{
			var openBraceIndex = FindBraceIndex(template, '{', scanIndex, endIndex);

			if (openBraceIndex == WrongBraceFound)
				// found '}' instead of '{'
				break;
			else if (openBraceIndex == NoBracesFound)
				// scanned the string and didn't find any remaining '{' or '}'
				break;

			var closeBraceIndex = FindBraceIndex(template, '}', openBraceIndex + 1, endIndex);
			if (closeBraceIndex < 0)
				break;

			// Format hole syntax : { index[,alignment][ :formatString] }.
			var formatDelimiterIndex = FindIndexOfAny(template, FormatDelimiters, openBraceIndex, closeBraceIndex);
			var property = template.Substring(openBraceIndex + 1, formatDelimiterIndex - openBraceIndex - 1).Trim();
			var remappedProperty = mapProperty(property);

			sb
				.Append(template, scanIndex, openBraceIndex - scanIndex + 1)
				.Append(remappedProperty)
				.Append(template, formatDelimiterIndex, closeBraceIndex - formatDelimiterIndex + 1)
			;

			scanIndex = closeBraceIndex + 1;
		}

		sb.Append(template, scanIndex, template.Length - scanIndex);
		return sb.ToString();
	}

	static int FindIndexOfAny(string template, char[] chars, int startIndex, int endIndex)
	{
		var findIndex = template.IndexOfAny(chars, startIndex, endIndex - startIndex);
		return findIndex == -1
			? endIndex
			: findIndex;
	}

	/// <summary>
	/// Searches for the next brace index in the template.
	/// </summary>
	/// <remarks>The search skips any sequences of {{ or }}.</remarks>
	/// <example>{{prefix{{{Argument}}}suffix}}.</example>
	/// <returns>The zero-based index position of the first occurrence of the searched brace; -1 if the searched brace was not found; -2 if the wrong brace was found.</returns>
	static int FindBraceIndex(string template, char searchedBrace, int startIndex, int endIndex)
	{
		Debug.Assert(searchedBrace is '{' or '}', "Searched brace must be { or }");

		var braceIndex = NoBracesFound;
		var scanIndex = startIndex;

		while (scanIndex < endIndex)
		{
			var current = template[scanIndex];
			if (current is '{' or '}')
			{
				var currentBrace = current;
				var scanIndexBeforeSkip = scanIndex;
				while (current == currentBrace && ++scanIndex < endIndex)
					current = template[scanIndex];

				var bracesCount = scanIndex - scanIndexBeforeSkip;

				// if it is an even number of braces, just skip them, otherwise, we found an unescaped brace
				if (bracesCount % 2 != 0)
				{
					if (currentBrace == searchedBrace)
						if (currentBrace == '{')
							// For '{' pick the last occurrence.
							braceIndex = scanIndex - 1;
						else
							// For '}' pick the first occurrence.
							braceIndex = scanIndexBeforeSkip;
					else
						// wrong brace found
						braceIndex = WrongBraceFound;

					break;
				}
			}
			else
				scanIndex++;
		}

		return braceIndex;
	}
}
