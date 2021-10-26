using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoreSharp.Abstracts
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        //Events  
        public event PropertyChangedEventHandler PropertyChanged;

        //Methods 
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, args);
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
