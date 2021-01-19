using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Common.Shared.Min.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<MemoryStream> CopyAsMemoryStreamAsync([NotNull] this Stream stream)
        {
            var buffer = new byte[stream.Length];
            var ms = new MemoryStream(buffer);

            stream.Position = 0;

            await stream.CopyToAsync(ms).KeepContext();

            ms.Position = 0;

            return ms;
        }
    }
}
