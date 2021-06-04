using System.Globalization;

namespace CoreSharp.Interfaces.Localize
{
    public interface ITextLocalizerFactory
    {
        //Methods
        ITextLocalizer Create<TResource>(CultureInfo culture);
        ITextLocalizer Create<TResource>() => Create<TResource>(CultureInfo.CurrentUICulture);
    }
}
