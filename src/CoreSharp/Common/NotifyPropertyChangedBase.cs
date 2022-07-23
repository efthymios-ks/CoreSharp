using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoreSharp.Common;

/// <inheritdoc cref="INotifyPropertyChanged" />
public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
{
    //Events  
    public event PropertyChangedEventHandler PropertyChanged;

    //Methods 
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new(propertyName));

    protected virtual bool SetValue<TValue>(ref TValue field, TValue value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
