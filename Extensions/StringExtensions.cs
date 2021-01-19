using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace Common.Shared.Min.Extensions
{
	public static class StringExtensions
	{
		public static string Repeat(this string input, int count)
		{
			if (string.IsNullOrEmpty(input)) return string.Empty;

			var builder = new StringBuilder(input.Length * count);

			for (var i = 0; i < count; i++)
				builder.Append(input);

			return builder.ToString();
		}

		public static bool TryParseEnum<TEnum>(this string value,
			out TEnum enumValue, bool ignoreCase = true)
			where TEnum : struct, Enum
		{
			enumValue = default;

			return !value.IsNullOrEmpty() && Enum.TryParse(value, ignoreCase, out enumValue);
		}

		public static Enum? ParseEnum(this string? value, Type enumType,
			bool ignoreCase = true)
		{
			if (string.IsNullOrEmpty(value)) return null;
			if (Enum.TryParse(enumType, value, ignoreCase, out var lResult)) return (Enum)lResult!;

			return default;
		}

		public static TEnum ParseEnum<TEnum>(this string? value,
			bool ignoreCase = true,
			TEnum defaultValue = default)
			where TEnum : struct, Enum
		{
			if (string.IsNullOrEmpty(value)) return defaultValue;
			return Enum.TryParse(value, ignoreCase, out TEnum lResult)
				? lResult
				: defaultValue;
		}

		[Pure]
		public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str) => string.IsNullOrEmpty(str);

		[Pure]
		public static bool IsNotNullOrEmpty([NotNullWhen(false)] this string? str) => !str.IsNullOrEmpty();

		[Pure]
		public static bool IsNotNullOrWhiteSpace([NotNullWhen(true)] this string? str) => !string.IsNullOrWhiteSpace(str);
		[Pure]
		public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? str) => string.IsNullOrWhiteSpace(str);

		public static bool EqualsInsensitive(this string text, string compareValue) => string.Equals(text, compareValue, StringComparison.OrdinalIgnoreCase);
		public static bool ContainsInsensitive(this string text, string containValue) => text.Contains(containValue, StringComparison.OrdinalIgnoreCase);

		public static bool Contains(this string? source, string toCheck, StringComparison comp) => source?.IndexOf(toCheck, comp) >= 0;

		public static string InsertArgs(this string str, CultureInfo culture, params object[] args) => string.Format(culture, str, args);

		/// <param name="source">The string to truncate</param>
		/// <param name="maxLength">If greater than zero, the string will be truncated at specified length -1 and elipsis appended if longer than maxLengh</param>
		/// <param name="addElipsis">Whether the last allowed character will be replaced by an eplipsis, if text is longer than maxlength.</param>
		/// <returns></returns>
		public static string? TruncateAt(this string? source, int maxLength, bool addElipsis = true)
		{
			if (maxLength <= 0 || source.IsNullOrEmpty() || source.Length <= maxLength) return source;

			if (addElipsis)
				return source.Substring(0, maxLength - 1) + "…";

			return source.Substring(0, maxLength);
		}

		public static string InsertArgs(this string str, params object?[] args)
		{
			if (str.IsNullOrEmpty()) return str;
			if (args.Length == 0) return str;
			if (!str.Contains("}") || !str.Contains("{")) return str;

			return string.Format(str, args);
		}

        public static string ReplaceUntil(this string source, string separator, string find, string replace)
        {
            var strBefore = source.SubstringBefore(separator, true);
            var strAfter = source.SubstringAfter(separator);
            var newString = strBefore.Replace(find, replace, StringComparison.Ordinal) + strAfter;

            return newString;
        }

        public static string? Remove(this string? source, string? textToRemove)
        {
            if (source!.IsNullOrEmpty()) return source;

            return textToRemove!.IsNullOrEmpty()
                ? source
                : source!.Replace(textToRemove, string.Empty);
        }

        /// <summary>
        /// Returns the substring until the given <paramref name="occurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="occurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringBefore(this string? source, char occurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = false)
        {
            if (source.IsNullOrEmpty()) return source;

            var index = source.IndexOf(occurrence, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            var length = includeOccurrence ? index + 1 : index;
            return source.Substring(0, length);
        }

        /// <summary>
        /// Returns the substring until the given <paramref name="occurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="occurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringBefore(this string? source, string occurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = false)
        {
            if (source.IsNullOrEmpty()) return source;

            var index = source.IndexOf(occurrence, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            var length = includeOccurrence ? index + occurrence.Length : index;
            return source.Substring(0, length);
        }

        /// <summary>
        /// Returns the substring between the given <paramref name="startOccurrence" /> and <paramref name="endOccurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startOccurrence">The occurrence to search.</param>
        /// <param name="endOccurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringBetween(this string? source, string startOccurrence, string endOccurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = false)
        {
            if (source.IsNullOrEmpty()) return source;

            var startIndex = source.IndexOf(startOccurrence, StringComparison.InvariantCultureIgnoreCase);
            if (startIndex == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            startIndex = includeOccurrence ? startIndex : startIndex + startOccurrence.Length;

            var endIndex = source.IndexOf(endOccurrence, StringComparison.InvariantCultureIgnoreCase);
            if (endIndex == -1)
            {
                if (returnEmptyIfNotFound)
                    return string.Empty;

                endIndex = source.Length - 1;
            }
            else
                endIndex += includeOccurrence ? endOccurrence.Length : 0;

            return source[startIndex..endIndex];
        }

        /// <summary>
        /// Returns the substring between the given <paramref name="startOccurrence" /> and <paramref name="endOccurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startOccurrence">The occurrence to search.</param>
        /// <param name="endOccurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringBetween(this string? source, char startOccurrence, char endOccurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = false)
        {
            if (source.IsNullOrEmpty()) return source;

            var startIndex = source.IndexOf(startOccurrence, StringComparison.InvariantCultureIgnoreCase);
            if (startIndex == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            startIndex = includeOccurrence ? startIndex : startIndex + 1;

            var endIndex = source.IndexOf(endOccurrence, StringComparison.InvariantCultureIgnoreCase);
            if (endIndex == -1)
            {
                if (returnEmptyIfNotFound)
                    return string.Empty;

                endIndex = source.Length - 1;
            }
            else
                endIndex += includeOccurrence ? 1 : 0;

            return source[startIndex..endIndex];
        }

        /// <summary>
        /// Returns the substring until the given <paramref name="occurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="occurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringAfter(this string? source, char occurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = false)
        {
            if (source.IsNullOrEmpty()) return source;

            var index = source.IndexOf(occurrence, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            return source[(includeOccurrence ? index : index + 1)..];
        }

        /// <summary>
        /// Returns the substring until the given <paramref name="occurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="occurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringAfter(this string? source, string occurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = false)
        {
            if (source.IsNullOrEmpty()) return source;

            var index = source.IndexOf(occurrence, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            return source[(includeOccurrence ? index : index + occurrence.Length)..];
        }

        /// <summary>
        /// Returns the substring until the given <paramref name="occurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="occurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringBeforeLast(this string? source, string occurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = true)
        {
            if (source.IsNullOrEmpty()) return source;

            var index = source.LastIndexOf(occurrence, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            var length = includeOccurrence ? index + occurrence.Length : index;
            return source.Substring(0, length);
        }

        /// <summary>
        /// Returns the substring until the given <paramref name="occurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="occurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringBeforeLast(this string? source, char occurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = false)
        {
            if (source.IsNullOrEmpty()) return source;

            var index = source.LastIndexOf(occurrence);
            if (index == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            var length = includeOccurrence ? index + 1 : index;
            return source.Substring(0, length);
        }

        /// <summary>
        /// Returns the substring after the given <paramref name="occurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="occurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringAfterLast(this string? source, string occurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = true)
        {
            if (source.IsNullOrEmpty()) return source;

            var index = source.LastIndexOf(occurrence, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            return source[(includeOccurrence ? index : index + occurrence.Length)..];
        }

        /// <summary>
        /// Returns the substring after the given <paramref name="occurrence"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="occurrence">The occurrence to search.</param>
        /// <param name="includeOccurrence">Whether include the occurrence or not.</param>
        /// <param name="returnEmptyIfNotFound">Whether to return an empty string if the occurrence could not be found.</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("source")]
        public static string? SubstringAfterLast(this string? source, char occurrence, bool includeOccurrence = false, bool returnEmptyIfNotFound = false)
        {
            if (source.IsNullOrEmpty()) return source;

            var index = source.LastIndexOf(occurrence);
            if (index == -1)
                return returnEmptyIfNotFound ? string.Empty : source;

            return source[(includeOccurrence ? index : index + 1)..];
        }
    }
}
