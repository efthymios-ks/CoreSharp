using CoreSharp.EqualityComparers;
using JsonNet = Newtonsoft.Json;
using TextJson = System.Text.Json;

namespace CoreSharp.Observers
{
    /// <summary>
    /// Observe a value for changes and notify
    /// using json conversions.
    /// </summary>
    public class JsonObserver<TEntity> : Observer<TEntity>
    {
        //Constructors
        public JsonObserver()
            : base(new JsonEqualityComparer<TEntity>())
        {
        }

        public JsonObserver(JsonNet.JsonSerializerSettings settings)
            : base(new JsonEqualityComparer<TEntity>(settings))
        {
        }

        public JsonObserver(TextJson.JsonSerializerOptions options)
            : base(new JsonEqualityComparer<TEntity>(options))
        {
        }

        public JsonObserver(TEntity initialValue)
            : base(initialValue, new JsonEqualityComparer<TEntity>())
        {
        }

        public JsonObserver(TEntity initialValue, JsonNet.JsonSerializerSettings settings)
            : base(initialValue, new JsonEqualityComparer<TEntity>(settings))
        {
        }

        public JsonObserver(TEntity initialValue, TextJson.JsonSerializerOptions options)
            : base(initialValue, new JsonEqualityComparer<TEntity>(options))
        {
        }
    }
}
