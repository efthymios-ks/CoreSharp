using System;

namespace CoreSharp.Observers.Contracts
{
    public interface IObserver<TValue>
    {
        //Properties
        TValue Value { get; set; }

        //Events 
        event Action<TValue> ValueChanged;
    }
}
