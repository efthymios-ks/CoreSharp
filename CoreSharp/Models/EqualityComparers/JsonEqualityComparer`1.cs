using CoreSharp.Models.Newtonsoft.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreSharp.Models.EqualityComparers
{
    /// <summary>
    /// Defines methods to support the comparison of objects for equality.
    /// Uses only <see cref="JsonConvert.SerializeObject(object?, JsonSerializerSettings?)"/> for conversions
    /// </summary>
    public class JsonEqualityComparer<TEntity> : IEqualityComparer<TEntity>
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        //Constructors
        public JsonEqualityComparer() : this(DefaultJsonSettings.Instance)
        {
        }

        public JsonEqualityComparer(JsonSerializerSettings jsonSerializerSettings)
            => _jsonSerializerSettings = jsonSerializerSettings ?? throw new ArgumentNullException(nameof(jsonSerializerSettings));

        //Methods
        public bool Equals(TEntity left, TEntity right)
        {
            var leftJson = JsonConvert.SerializeObject(left, _jsonSerializerSettings);
            var rightJson = JsonConvert.SerializeObject(right, _jsonSerializerSettings);
            return leftJson == rightJson;
        }

        public int GetHashCode(TEntity item)
            => JsonConvert.SerializeObject(item, _jsonSerializerSettings).GetHashCode();
    }
}
