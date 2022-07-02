using CoreSharp.Observers.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreSharp.Observers;

/// <summary>
/// Observe a value for changes and notify
/// using provided <see cref="IEqualityComparer{T}"/>.
/// </summary>
public class StateObserver<TEntity> : IStateObserver<TEntity>
    where TEntity : class
{
    //Fields 
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IEqualityComparer<TEntity> _equalityComparer;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private TEntity _value;

    //Constructors
    public StateObserver()
    : this(EqualityComparer<TEntity>.Default)
    {
    }

    public StateObserver(TEntity initialValue)
    : this(initialValue, EqualityComparer<TEntity>.Default)
    {
    }

    public StateObserver(IEqualityComparer<TEntity> equalityComparer)
        : this(null, equalityComparer)
    {
    }

    public StateObserver(TEntity initialValue, IEqualityComparer<TEntity> equalityComparer)
    {
        _value = initialValue;
        _equalityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));
    }

    //Properties
    public TEntity State
    {
        get => _value;
        set
        {
            if (_equalityComparer.Equals(value, _value))
                return;

            _value = value;
            OnValueChanged(_value);
        }
    }

    //Events 
    public event Action<TEntity> StateChanged;

    private void OnValueChanged(TEntity value)
        => StateChanged?.Invoke(value);
}
