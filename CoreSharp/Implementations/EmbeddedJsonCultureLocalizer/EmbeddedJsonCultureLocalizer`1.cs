using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using CoreSharp.Interfaces.CultureLocalizer;
using Microsoft.Extensions.Localization;

namespace CoreSharp.Implementations.EmbeddedJsonCultureLocalizer
{
    public class EmbeddedJsonCultureLocalizer<TResource> : ICultureLocalizer<TResource>
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ICultureLocalizer localizer;

        //Constructors
        public EmbeddedJsonCultureLocalizer(ICultureLocalizerFactory localizerFactory)
        {
            localizerFactory = localizerFactory ?? throw new ArgumentNullException(nameof(localizerFactory));

            localizer = localizerFactory.Create<TResource>();
        }

        //Indexers
        public string this[string key] => localizer[key];

        public LocalizedString this[string name, params object[] arguments] => localizer[name, arguments];

        LocalizedString IStringLocalizer.this[string name] => localizer[name];

        //Properties
        public Type ResourceType => localizer.ResourceType;

        public CultureInfo Culture => localizer.Culture;

        //Methods 
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => localizer.GetAllStrings(includeParentCultures);
    }
}
