using CoreSharp.Json.JsonNet;
using CoreSharp.Json.TextJson;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="Stream"/> extensions.
/// </summary>
public static class StreamExtensions
{
    // Fields
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const int DefaultBufferSize = 10 * 1024;

    /// <inheritdoc cref="FromJson(Stream, Type)"/>
    public static TEntity FromJson<TEntity>(this Stream stream)
        where TEntity : class
        => stream.FromJson(typeof(TEntity)) as TEntity;

    /// <inheritdoc cref="FromJson(Stream, Type, JsonNet.JsonSerializerSettings)"/>
    public static object FromJson(this Stream stream, Type entityType)
        => stream.FromJson(entityType, JsonSettings.Default);

    /// <inheritdoc cref="FromJson(Stream, Type, JsonNet.JsonSerializerSettings)"/>
    public static TEntity FromJson<TEntity>(this Stream stream, JsonNet.JsonSerializerSettings settings)
        where TEntity : class
       => stream.FromJson(typeof(TEntity), settings) as TEntity;

    /// <summary>
    /// Parses the text representing a JSON object
    /// into an instance of the type specified using Json.NET.
    /// </summary>
    public static object FromJson(this Stream stream, Type entityType, JsonNet.JsonSerializerSettings settings)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
        _ = settings ?? throw new ArgumentNullException(nameof(settings));

        if (!stream.CanRead)
        {
            throw new NotSupportedException($"{nameof(stream)} is not readable.");
        }

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        try
        {
            var serializer = JsonNet.JsonSerializer.Create(settings);
            using var streamReader = new StreamReader(stream, leaveOpen: true);
            using var jsonReader = new JsonNet.JsonTextReader(streamReader);
            return serializer.Deserialize(jsonReader, entityType);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
        }
    }

    /// <inheritdoc cref="FromJsonAsync(Stream, Type)"/>
    public static async Task<TEntity> FromJsonAsync<TEntity>(this Stream stream)
        where TEntity : class
        => await stream.FromJsonAsync(typeof(TEntity)) as TEntity;

    /// <inheritdoc cref="FromJsonAsync(Stream, Type, TextJson.JsonSerializerOptions)"/>
    public static async Task<object> FromJsonAsync(this Stream stream, Type entityType)
        => await stream.FromJsonAsync(entityType, JsonOptions.Default);

    /// <inheritdoc cref="FromJsonAsync(Stream, Type, TextJson.JsonSerializerOptions)"/>
    public static async Task<TEntity> FromJsonAsync<TEntity>(this Stream stream, TextJson.JsonSerializerOptions options)
        where TEntity : class
        => await stream.FromJsonAsync(typeof(TEntity), options) as TEntity;

    /// <summary>
    /// Parses the text representing a JSON object
    /// into an instance of the type specified using Text.Json.
    /// </summary>
    public static async Task<object> FromJsonAsync(this Stream stream, Type entityType, TextJson.JsonSerializerOptions options)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));
        _ = entityType ?? throw new ArgumentNullException(nameof(entityType));
        _ = options ?? throw new ArgumentNullException(nameof(options));

        if (!stream.CanRead)
        {
            throw new NotSupportedException($"{nameof(stream)} is not readable.");
        }

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        try
        {
            return await TextJson.JsonSerializer.DeserializeAsync(stream, entityType, options);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
        }
    }

    /// <summary>
    /// Parses the text representing an XML document
    /// into an instance of the type specified.
    /// </summary>
    public static async Task<TEntity> FromXmlAsync<TEntity>(this Stream stream, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        if (!stream.CanRead)
        {
            throw new NotSupportedException($"{nameof(stream)} is not readable.");
        }

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        try
        {
            var document = await XDocument.LoadAsync(stream, LoadOptions.None, cancellationToken);
            return document.ToEntity<TEntity>();
        }
        catch
        {
            return null;
        }
        finally
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
        }
    }

    /// <summary>
    /// Write <see cref="Stream"/> to physical file.
    /// </summary>
    public static Task ToFileAsync(this Stream stream, string filePath, int bufferSize = DefaultBufferSize, CancellationToken cancellationToken = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        return string.IsNullOrWhiteSpace(filePath)
                ? throw new ArgumentNullException(nameof(filePath))
                : stream.ToFileInternalAsync(filePath, bufferSize, cancellationToken);
    }

    private static async Task ToFileInternalAsync(this Stream stream, string filePath, int bufferSize = DefaultBufferSize, CancellationToken cancellationToken = default)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

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
        {
            stream.Position = 0;
        }

        using var reader = new StreamReader(stream, encoding: encoding, leaveOpen: true);
        return await reader.ReadToEndAsync();
    }

    /// <inheritdoc cref="EqualsAsync(Stream, Stream, int, CancellationToken)"/>
    public static async Task<bool> EqualsAsync(this Stream left, Stream right, CancellationToken cancellationToken = default)
        => await left.EqualsAsync(right, DefaultBufferSize, cancellationToken);

    /// <summary>
    /// Compare two <see cref="Stream"/> in chunks.
    /// </summary>
    public static async Task<bool> EqualsAsync(this Stream left, Stream right, int bufferSize, CancellationToken cancellationToken = default)
    {
        _ = left ?? throw new ArgumentNullException(nameof(left));
        _ = right ?? throw new ArgumentNullException(nameof(right));

        try
        {
            // Reference equals 
            if (left == right)
            {
                return true;
            }

            // Different length
            if (left.Length != right.Length)
            {
                return false;
            }

            // Check readability 
            static void ThrowNotReadableException(string paramName)
                => throw new NotSupportedException($"{paramName} is not readable.");
            if (!left.CanRead)
            {
                ThrowNotReadableException(nameof(left));
            }
            else if (!right.CanRead)
            {
                ThrowNotReadableException(nameof(right));
            }

            // Compare chunks
            left.Position = 0;
            right.Position = 0;
            for (var i = 0; i < left.Length; i += bufferSize)
            {
                var leftChunk = new byte[bufferSize];
                var rightChunk = new byte[bufferSize];

                await left.ReadAsync(leftChunk.AsMemory(), cancellationToken);
                await right.ReadAsync(rightChunk.AsMemory(), cancellationToken);

                if (!leftChunk.SequenceEqual(rightChunk))
                {
                    return false;
                }
            }

            return true;
        }
        finally
        {
            left.Position = 0;
            right.Position = 0;
        }
    }

    /// <inheritdoc cref="ToArrayAsync(Stream, int, CancellationToken)"/>
    public static async Task<byte[]> ToArrayAsync(this Stream stream, CancellationToken cancellationToken = default)
        => await stream.ToArrayAsync(DefaultBufferSize, cancellationToken);

    /// <summary>
    /// Converts <see cref="Stream"/>
    /// to <see langword="byte[]"/>.
    /// </summary>
    public static async Task<byte[]> ToArrayAsync(this Stream stream, int bufferSize, CancellationToken cancellationToken = default)
    {
        _ = stream ?? throw new ArgumentNullException(nameof(stream));

        stream.Position = 0;

        if (stream is MemoryStream internalMemoryStream)
        {
            return internalMemoryStream.ToArray();
        }

        await using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, bufferSize, cancellationToken);
        return memoryStream.ToArray();
    }
}
