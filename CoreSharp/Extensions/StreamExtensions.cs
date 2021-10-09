using CoreSharp.Models.Newtonsoft.Settings;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CoreSharp.Extensions
{
    /// <summary>
    /// <see cref="Stream"/> extensions.
    /// </summary>
    public static class StreamExtensions
    {
        /// <inheritdoc cref="ToEntity(Stream, Type, JsonSerializerSettings)"/>
        public static TEntity ToEntity<TEntity>(this Stream stream) where TEntity : class
            => stream.ToEntity(typeof(TEntity)) as TEntity;

        /// <inheritdoc cref="ToEntity(Stream, Type, JsonSerializerSettings)"/>
        public static object ToEntity(this Stream stream, Type entityType)
            => stream.ToEntity(entityType, DefaultJsonSettings.Instance);

        /// <inheritdoc cref="ToEntity(Stream, Type, JsonSerializerSettings)"/>
        public static TEntity ToEntity<TEntity>(this Stream stream, JsonSerializerSettings settings) where TEntity : class
           => stream.ToEntity(typeof(TEntity), settings) as TEntity;

        /// <summary>
        /// Parse <see cref="Stream"/> json to entity.
        /// </summary>
        public static object ToEntity(this Stream stream, Type entityType, JsonSerializerSettings settings)
        {
            _ = stream ?? throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead)
                throw new NotSupportedException($"{nameof(stream)} is not readable.");
            else if (stream.Position > 0 && !stream.CanSeek)
                throw new NotSupportedException($"{nameof(stream)} is not seekable.");

            try
            {
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
        }
    }
}
