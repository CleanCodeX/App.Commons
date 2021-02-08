using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Common.Shared.Min.Extensions
{
	public static class ArrayExtensions
	{
		public static byte[] Resize([NotNull] this byte[] source, int newSize)
		{
			Array.Resize(ref source, newSize);

			return source;
		}

		public static MemoryStream? ToStreamIfNotNull(this byte[]? source)
		{
			if (source is null) return null;

			return new(source);
		}

		/// <summary>Creates a memory stream</summary>
		/// <param name="source"></param>
		/// <returns>New memory stream</returns>
		public static MemoryStream ToStream([NotNull] this byte[] source) => new(source);
	}
}
