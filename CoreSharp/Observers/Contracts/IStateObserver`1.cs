using System;

namespace CoreSharp.Observers.Contracts
{
    public interface IStateObserver<TValue>
    {
        //Properties
        TValue Value { get; set; }

        //Events 
        event Action<TValue> ValueChanged;
    }
}
