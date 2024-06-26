// Kyoo - A portable and vast media library solution.
// Copyright (c) Kyoo.
//
// See AUTHORS.md and LICENSE file in the project root for full license information.
//
// Kyoo is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// Kyoo is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Kyoo. If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Kyoo.Utils;

/// <summary>
/// A set of utility functions that can be used everywhere.
/// </summary>
public static class Utility
{
	public static readonly JsonSerializerOptions JsonOptions =
		new()
		{
			TypeInfoResolver = new JsonKindResolver(),
			Converters = { new JsonStringEnumConverter() },
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	/// <summary>
	/// Convert a string to snake case. Stollen from
	/// https://github.com/efcore/EFCore.NamingConventions/blob/main/EFCore.NamingConventions/Internal/SnakeCaseNameRewriter.cs
	/// </summary>
	/// <param name="name">The string to convert.</param>
	/// <returns>The string in snake case</returns>
	public static string ToSnakeCase(this string name)
	{
		StringBuilder builder = new(name.Length + Math.Min(2, name.Length / 5));
		UnicodeCategory? previousCategory = default;

		for (int currentIndex = 0; currentIndex < name.Length; currentIndex++)
		{
			char currentChar = name[currentIndex];
			if (currentChar == '_')
			{
				builder.Append('_');
				previousCategory = null;
				continue;
			}

			UnicodeCategory currentCategory = char.GetUnicodeCategory(currentChar);
			switch (currentCategory)
			{
				case UnicodeCategory.UppercaseLetter:
				case UnicodeCategory.TitlecaseLetter:
					if (
						previousCategory == UnicodeCategory.SpaceSeparator
						|| previousCategory == UnicodeCategory.LowercaseLetter
						|| (
							previousCategory != UnicodeCategory.DecimalDigitNumber
							&& previousCategory != null
							&& currentIndex > 0
							&& currentIndex + 1 < name.Length
							&& char.IsLower(name[currentIndex + 1])
						)
					)
					{
						builder.Append('_');
					}

					currentChar = char.ToLowerInvariant(currentChar);
					break;

				case UnicodeCategory.LowercaseLetter:
				case UnicodeCategory.DecimalDigitNumber:
					if (previousCategory == UnicodeCategory.SpaceSeparator)
					{
						builder.Append('_');
					}
					break;

				default:
					if (previousCategory != null)
					{
						previousCategory = UnicodeCategory.SpaceSeparator;
					}
					continue;
			}

			builder.Append(currentChar);
			previousCategory = currentCategory;
		}

		return builder.ToString();
	}

	/// <summary>
	/// Is the lambda expression a member (like x => x.Body).
	/// </summary>
	/// <param name="ex">The expression that should be checked</param>
	/// <returns>True if the expression is a member, false otherwise</returns>
	public static bool IsPropertyExpression(LambdaExpression ex)
	{
		return ex.Body is MemberExpression
			|| (
				ex.Body.NodeType == ExpressionType.Convert
				&& ((UnaryExpression)ex.Body).Operand is MemberExpression
			);
	}

	/// <summary>
	/// Get the name of a property. Useful for selectors as members ex: Load(x => x.Shows)
	/// </summary>
	/// <param name="ex">The expression</param>
	/// <returns>The name of the expression</returns>
	/// <exception cref="ArgumentException">If the expression is not a property, ArgumentException is thrown.</exception>
	public static string GetPropertyName(LambdaExpression ex)
	{
		if (!IsPropertyExpression(ex))
			throw new ArgumentException($"{ex} is not a property expression.");
		MemberExpression? member =
			ex.Body.NodeType == ExpressionType.Convert
				? ((UnaryExpression)ex.Body).Operand as MemberExpression
				: ex.Body as MemberExpression;
		return member!.Member.Name;
	}

	/// <summary>
	/// Slugify a string (Replace spaces by -, Uniformize accents)
	/// </summary>
	/// <param name="str">The string to slugify</param>
	/// <returns>The slug version of the given string</returns>
	public static string ToSlug(string str)
	{
		str = str.ToLowerInvariant();

		string normalizedString = str.Normalize(NormalizationForm.FormD);
		StringBuilder stringBuilder = new();
		foreach (char c in normalizedString)
		{
			UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
			if (unicodeCategory != UnicodeCategory.NonSpacingMark)
				stringBuilder.Append(c);
		}
		str = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

		str = Regex.Replace(str, @"\s", "-", RegexOptions.Compiled);
		str = Regex.Replace(str, @"[^\w\s\p{Pd}]", string.Empty, RegexOptions.Compiled);
		str = str.Trim('-', '_');
		str = Regex.Replace(str, @"([-_]){2,}", "$1", RegexOptions.Compiled);
		return str;
	}

	/// <summary>
	/// Return every <see cref="Type"/> in the inheritance tree of the parameter (interfaces are not returned)
	/// </summary>
	/// <param name="self">The starting type</param>
	/// <returns>A list of types</returns>
	public static IEnumerable<Type> GetInheritanceTree(this Type self)
	{
		for (Type? type = self; type != null; type = type.BaseType)
			yield return type;
	}

	/// <summary>
	/// Get the generic definition of <paramref name="genericType"/>.
	/// For example, calling this function with List&lt;string&gt; and typeof(IEnumerable&lt;&gt;) will return IEnumerable&lt;string&gt;
	/// </summary>
	/// <param name="type">The type to check</param>
	/// <param name="genericType">The generic type to check against (Only generic types are supported like typeof(IEnumerable&lt;&gt;).</param>
	/// <returns>The generic definition of genericType that type inherit or null if type does not implement the generic type.</returns>
	/// <exception cref="ArgumentException"><paramref name="genericType"/> must be a generic type</exception>
	public static Type? GetGenericDefinition(Type type, Type genericType)
	{
		if (!genericType.IsGenericType)
			throw new ArgumentException($"{nameof(genericType)} is not a generic type.");

		IEnumerable<Type> types = genericType.IsInterface
			? type.GetInterfaces()
			: type.GetInheritanceTree();
		return types
			.Prepend(type)
			.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericType);
	}

	/// <summary>
	/// Convert a dictionary to a query string.
	/// </summary>
	/// <param name="query">The list of query parameters.</param>
	/// <returns>A valid query string with all items in the dictionary.</returns>
	public static string ToQueryString(this Dictionary<string, string> query)
	{
		if (!query.Any())
			return string.Empty;
		return "?" + string.Join('&', query.Select(x => $"{x.Key}={x.Value}"));
	}
}
