using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreSharp.Observers
{
    /// <summary>
    /// Observe a value for changes and notify
    /// using provided <see cref="IEqualityComparer{T}"/>.
    /// </summary>
    public class Observer<TValue> : Contracts.IObserver<TValue>
    {
        //Fields 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IEqualityComparer<TValue> _equalityComparer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TValue _value;

        //Constructors
        public Observer()
        : this(EqualityComparer<TValue>.Default)
        {
        }

        public Observer(TValue initialValue)
        : this(initialValue, EqualityComparer<TValue>.Default)
        {
        }

        public Observer(TValue initialValue, IEqualityComparer<TValue> equalityComparer)
        : this(equalityComparer)
            => _value = initialValue;

        public Observer(IEqualityComparer<TValue> equalityComparer)
            => _equalityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));

        //Properties
        public TValue Value
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
        public event Action<TValue> ValueChanged;

        private void OnValueChanged(TValue value)
            => ValueChanged?.Invoke(value);
    }
}
