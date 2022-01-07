// -----------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.Utils
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="StreamExtensions" />.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// The GetAllBytes.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] GetAllBytes(this Stream stream)
        {
            using var memoryStream = new MemoryStream();
            stream.Position = 0;
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// The GetAllBytesAsync.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{byte[]}"/>.</returns>
        public static async Task<byte[]> GetAllBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
        {
            await using var memoryStream = new MemoryStream();
            stream.Position = 0;
            await stream.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();

        }

        /// <summary>
        /// The CopyToAsync.
        /// </summary>
        /// <param name="stream">The stream<see cref="Stream"/>.</param>
        /// <param name="destination">The destination<see cref="Stream"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public static Task CopyToAsync(this Stream stream, Stream destination, CancellationToken cancellationToken)
        {
            stream.Position = 0;
            return stream.CopyToAsync(
                destination,
                81920, //this is already the default value, but needed to set to be able to pass the cancellationToken
                cancellationToken
            );
        }
    }
}
