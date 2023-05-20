using System;
using System.Threading.Tasks;

namespace CoreSharp.Common;

public sealed class LazyTask<TValue>
{
    // Fields
    private readonly Func<Task<TValue>> _valueFactory;
    private TValue _value;

    // Constructors
    public LazyTask(Func<Task<TValue>> valueFactory)
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        _valueFactory = valueFactory;
    }

    // Properties
    public bool IsValueInitialized { get; private set; }

    // Methods 
    /// <summary>
    /// Gets the lazy initialized value of current <see cref="LazyTask{TValue}"/>.
    /// Will only initialize only once. Any subsequence call, will retrieve stored value.
    /// </summary>
    public async ValueTask<TValue> GetValueAsync()
    {
        if (IsValueInitialized)
        {
            return _value;
        }

        _value = await _valueFactory();
        IsValueInitialized = true;
        return _value;
    }
}
