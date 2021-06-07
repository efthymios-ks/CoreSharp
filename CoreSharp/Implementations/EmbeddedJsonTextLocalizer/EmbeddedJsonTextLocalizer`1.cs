using System;
using System.Diagnostics;
using System.Globalization;
using CoreSharp.Interfaces.Localize;

namespace CoreSharp.Implementations.TextLocalizer
{
    public class EmbeddedJsonTextLocalizer<TResource> : ITextLocalizer<TResource>
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ITextLocalizer localizer;

        //Constructors
        public EmbeddedJsonTextLocalizer(ITextLocalizerFactory localizerFactory)
        {
            localizerFactory = localizerFactory ?? throw new ArgumentNullException(nameof(localizerFactory));

            localizer = localizerFactory.Create<TResource>();
        }

        //Indexers
        public string this[string key] => localizer[key];

        //Properties
        public CultureInfo Culture => localizer.Culture;
    }
}
