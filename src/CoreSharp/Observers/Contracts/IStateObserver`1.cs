using System;

namespace CoreSharp.Observers.Contracts;

public interface IStateObserver<TEntity>
    where TEntity : class
{
    //Properties
    TEntity State { get; set; }

    //Events 
    event Action<TEntity> StateChanged;
}
