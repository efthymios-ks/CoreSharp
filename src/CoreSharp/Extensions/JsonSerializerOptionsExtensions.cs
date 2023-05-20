using System;
using System.Text.Json;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="JsonSerializerOptions"/> extensions.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    // TODO: Add unit tests.
    /// <summary>
    /// Override settings from one <see cref="JsonSerializerOptions"/> to another.<br/>
    /// Useful when you cannot assign new reference to the options. E.g. ASP.NET Core DI.
    /// <code>
    /// public static void ConfigureServices(IServiceCollection services)
    /// {
    ///     services.AddControllers
    ///             .AddJsonOptions(options => 
    ///             {
    ///                 // Map from custom options. 
    ///                 JsonOptions.Default.MapTo(options);
    ///                 
    ///                 // Override with user preferred. 
    ///                 options.NumberHandling = JsonNumberHandling.Strict;
    ///             });
    /// }
    /// </code>
    /// </summary>
    public static void MapTo(this JsonSerializerOptions from, JsonSerializerOptions to)
    {
        _ = from ?? throw new ArgumentNullException(nameof(from));
        _ = to ?? throw new ArgumentNullException(nameof(to));

        to.AllowTrailingCommas = from.AllowTrailingCommas;
        to.DefaultBufferSize = from.DefaultBufferSize;
        to.DefaultIgnoreCondition = from.DefaultIgnoreCondition;
        to.DictionaryKeyPolicy = from.DictionaryKeyPolicy;
        to.Encoder = from.Encoder;
        to.IgnoreReadOnlyFields = from.IgnoreReadOnlyFields;
        to.IgnoreReadOnlyProperties = from.IgnoreReadOnlyProperties;
        to.IncludeFields = from.IncludeFields;
        to.MaxDepth = from.MaxDepth;
        to.NumberHandling = from.NumberHandling;
        to.PropertyNameCaseInsensitive = from.PropertyNameCaseInsensitive;
        to.PropertyNamingPolicy = from.PropertyNamingPolicy;
        to.ReadCommentHandling = from.ReadCommentHandling;
        to.ReferenceHandler = from.ReferenceHandler;
        to.TypeInfoResolver = from.TypeInfoResolver;
        to.UnknownTypeHandling = from.UnknownTypeHandling;
        to.WriteIndented = from.WriteIndented;

        to.Converters.Clear();
        foreach (var converter in from.Converters)
        {
            to.Converters.Add(converter);
        }
    }
}