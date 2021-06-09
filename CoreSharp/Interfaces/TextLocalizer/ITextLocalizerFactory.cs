using System.Globalization;

namespace CoreSharp.Interfaces.Localize
{
    public interface ITextLocalizerFactory
    {
        //Methods
        ITextLocalizer GetOrCreate<TResource>(CultureInfo culture);
        ITextLocalizer GetOrCreate<TResource>() => GetOrCreate<TResource>(CultureInfo.CurrentUICulture);
    }
}
