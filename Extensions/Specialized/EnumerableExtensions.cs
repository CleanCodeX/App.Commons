using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Common.Shared.Min.Extensions.Specialized
{
	public static class EnumerableExtensions
	{
		[Pure]
		public static bool IsEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? source) => source is null || !source.Any();

		[Pure]
		public static bool IsNotEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? source) => source is not null && source.Any();

		[Pure]
		public static string Join<T>(this IEnumerable<T>? source, string separator = ", ") => source is null ? string.Empty : string.Join(separator, source);
	}
}