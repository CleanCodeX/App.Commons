using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Common.Shared.Min.Extensions
{
	public static class ObjectExtensions
	{
		[return: NotNull]
		public static Enum ToEnum<TEnum>([NotNull] this object enumeration) 
			where TEnum : struct, Enum => enumeration.ToEnum(typeof(TEnum));

		public static IDictionary<string, Enum> ToDictionary<TEnum>(this TEnum source) where TEnum : struct, Enum => Enum.GetNames<TEnum>().ToDictionary(k => k, v => (Enum)v.ParseEnum<TEnum>());

		public static object? ParseEnum(this object? value, Type enumType,
			bool ignoreCase = true)
		{
			if (!(value is string str)) return null;

			Enum.TryParse(enumType, str, ignoreCase, out var result);

			return result;
		}

		[return: NotNull]
		public static Enum ToEnum([NotNull] this object @enum, Type enumType)
		{
			@enum.ThrowIfNull(nameof(@enum));
			var type = @enum!.GetType();
			type.IsEnum.ThrowIfFalse(nameof(type.IsEnum));

			return (Enum)Enum.Parse(enumType, @enum.ToString()!);
		}
	}
}