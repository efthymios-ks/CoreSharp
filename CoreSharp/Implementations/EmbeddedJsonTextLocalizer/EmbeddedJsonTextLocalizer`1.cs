using System;
using CoreSharp.Interfaces.Localize;

namespace CoreSharp.Implementations.TextLocalizer
{
    public class EmbeddedJsonTextLocalizer<TResource> : ITextLocalizer<TResource>
    {
        //Fields
        private readonly ITextLocalizer localizer;

        //Constructors
        public EmbeddedJsonTextLocalizer(ITextLocalizerFactory localizerFactory)
        {
            localizerFactory = localizerFactory ?? throw new ArgumentNullException(nameof(localizerFactory));

            localizer = localizerFactory.Create<TResource>();
        }

        //Indexers
        public string this[string key] => localizer[key];
    }
}
