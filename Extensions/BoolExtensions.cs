using System.Diagnostics.CodeAnalysis;
using Common.Shared.Min.Helpers;

namespace Common.Shared.Min.Extensions
{
	public static class BoolExtensions
	{
		public static void ThrowIfFalse([DoesNotReturnIf(false)] this bool source, string argName, string? customErrorText = null)
			=> Requires.NotFalse(source, argName, customErrorText);

		public static void ThrowIfTrue([DoesNotReturnIf(true)] this bool source, string argName, string? customErrorText = null)
			=> Requires.NotTrue(source, argName, customErrorText);
	}
}