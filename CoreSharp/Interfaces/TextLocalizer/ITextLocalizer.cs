using System;

namespace CoreSharp.Interfaces.Localize
{
    public interface ITextLocalizer
    {
        //Indexers
        string this[string key] { get; }
        string this[string key, params object[] arguments]
        {
            get
            {
                arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));

                return string.Format(this[key], arguments);
            }
        }
    }
}
