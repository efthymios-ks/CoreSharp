using CoreSharp.EqualityComparers;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Observers
{
    /// <summary>
    /// Observe a value for changes and notify
    /// using json conversions.
    /// </summary>
    public class JsonStateObserver<TEntity> : StateObserver<TEntity>
        where TEntity : class
    {
        //Constructors
        public JsonStateObserver()
            : base(new JsonEqualityComparer<TEntity>())
        {
        }

        public JsonStateObserver(JsonNet.JsonSerializerSettings settings)
            : base(new JsonEqualityComparer<TEntity>(settings))
        {
        }

        public JsonStateObserver(TextJson.JsonSerializerOptions options)
            : base(new JsonEqualityComparer<TEntity>(options))
        {
        }

        public JsonStateObserver(TEntity initialValue)
            : base(initialValue, new JsonEqualityComparer<TEntity>())
        {
        }

        public JsonStateObserver(TEntity initialValue, JsonNet.JsonSerializerSettings settings)
            : base(initialValue, new JsonEqualityComparer<TEntity>(settings))
        {
        }

        public JsonStateObserver(TEntity initialValue, TextJson.JsonSerializerOptions options)
            : base(initialValue, new JsonEqualityComparer<TEntity>(options))
        {
        }
    }
}
