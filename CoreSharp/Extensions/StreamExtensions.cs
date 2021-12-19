using CoreSharp.Models.Newtonsoft.Settings;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Stream"/> extensions.
    /// </summary>
    public static class StreamExtensions
    {
        /// <inheritdoc cref="FromJson(Stream, Type, JsonSerializerSettings)"/>
        public static TEntity FromJson<TEntity>(this Stream stream) where TEntity : class
            => stream.FromJson(typeof(TEntity)) as TEntity;

        /// <inheritdoc cref="FromJson(Stream, Type, JsonSerializerSettings)"/>
        public static object FromJson(this Stream stream, Type entityType)
            => stream.FromJson(entityType, DefaultJsonSettings.Instance);

        /// <inheritdoc cref="FromJson(Stream, Type, JsonSerializerSettings)"/>
        public static TEntity FromJson<TEntity>(this Stream stream, JsonSerializerSettings settings) where TEntity : class
           => stream.FromJson(typeof(TEntity), settings) as TEntity;

        /// <inheritdoc cref="JsonSerializer.Deserialize(JsonReader, Type?)" />
        public static object FromJson(this Stream stream, Type entityType, JsonSerializerSettings settings)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead)
                throw new NotSupportedException($"{nameof(stream)} is not readable.");

            try
            {
                if (stream.CanSeek)
                    stream.Position = 0;

                var serializer = JsonSerializer.Create(settings);
                using var streamReader = new StreamReader(stream);
                using var jsonReader = new JsonTextReader(streamReader);
                return serializer.Deserialize(jsonReader, entityType);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (stream.CanSeek)
                    stream.Position = 0;
            }
        }

        /// <summary>
        /// Write <see cref="Stream"/> to physical file.
        /// </summary>
        public static async Task ToFileAsync(this Stream stream, string filePath, int bufferSize = 81920, CancellationToken cancellationToken = default)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (stream.CanSeek)
                stream.Position = 0;
            await using var fileStream = File.OpenWrite(filePath);
            await stream.CopyToAsync(fileStream, bufferSize, cancellationToken);
        }

        /// <summary>
        /// Read <see cref="Stream"/> to <see cref="string"/>.
        /// </summary>
        public static async Task<string> ToStringAsync(this Stream stream, Encoding encoding = null)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));
            encoding ??= Encoding.UTF8;

            if (stream.CanSeek)
                stream.Position = 0;
            var reader = new StreamReader(stream, encoding);
            return await reader.ReadToEndAsync();
        }
    }
}
