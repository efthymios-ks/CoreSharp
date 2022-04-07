using CoreSharp.Json.JsonNet.ContractResolvers;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CoreSharp.Json.JsonNet
{
    public static class JsonSettings
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static JsonSerializerSettings _default;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static JsonSerializerSettings _displayOnly;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static JsonSerializerSettings _primitiveOnly;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static JsonSerializerSettings _strict;

        //Properties
        public static JsonSerializerSettings Default
            => _default ??= CreateDefault();

        public static JsonSerializerSettings DisplayOnly
            => _displayOnly ??= CreateDisplayOnly();

        public static JsonSerializerSettings PrimitiveOnly
            => _primitiveOnly ??= CreatePrimitiveOnly();

        public static JsonSerializerSettings Strict
            => _strict ??= CreateStrict();

        //Methods
        private static JsonSerializerSettings CreateDefault()
            => new()
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = WritableOnlyPropertiesResolver.Instance
            };

        private static JsonSerializerSettings CreateDisplayOnly()
            => new()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

        private static JsonSerializerSettings CreatePrimitiveOnly()
            => new()
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = PrimitiveOnlyResolver.Instance
            };

        private static JsonSerializerSettings CreateStrict()
            => new()
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Error,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = WritableOnlyPropertiesResolver.Instance
            };
    }
}
