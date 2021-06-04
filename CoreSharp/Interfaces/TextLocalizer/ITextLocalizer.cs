namespace CoreSharp.Interfaces.Localize
{
    public interface ITextLocalizer
    {
        //Indexers
        string this[string key] { get; }
        string this[string key, params object[] arguments] => string.Format(this[key], arguments);
    }
}
