using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Common.Shared.Min.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<MemoryStream> CopyAsMemoryStreamAsync([NotNull] this Stream source, bool resetPosition = false)
        {
            var buffer = new byte[source.Length];
            var ms = new MemoryStream(buffer);

            if(resetPosition && source.CanSeek)
                source.Position = 0;

            await source.CopyToAsync(ms).KeepContext();

            ms.Position = 0;

            return ms;
        }


        /// <summary>
        /// Saves the whole content of the stream to a target stream by setting the source position top 0
        /// </summary>
        public static void CopyAllTo([NotNull] this Stream source, [NotNull] Stream target)
        {
            source.Position = 0;
            source.CopyTo(target);
        }
    }
}
