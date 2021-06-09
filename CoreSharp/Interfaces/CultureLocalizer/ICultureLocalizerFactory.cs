using System;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace CoreSharp.Interfaces.CultureLocalizer
{
    public interface ICultureLocalizerFactory : IStringLocalizerFactory
    {
        //Methods
        ICultureLocalizer Create(Type resourceType, CultureInfo culture);
        ICultureLocalizer Create<TResource>() => Create<TResource>(CultureInfo.CurrentUICulture);
        ICultureLocalizer Create<TResource>(CultureInfo culture) => Create(typeof(TResource), culture);
    }
}
