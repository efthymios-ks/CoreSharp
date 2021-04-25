using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoreSharp.Implementations
{
    /// <summary>
    /// Base class implementing INotifyPropertyChanged. 
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        //Events  
        public event PropertyChangedEventHandler PropertyChanged;

        //Methods 
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, args);
        }

        protected void SetProperty<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue))
                return;

            backingField = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}
