using System;
using System.Diagnostics;
using System.IO;

namespace CoreSharp.Abstracts
{
    /// <inheritdoc cref="IDisposable"/>
    public abstract class DisposableBase : IDisposable
    {
        //Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly object _lock = new();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _disposed;

        //Constructors
        ~DisposableBase()
            => DisposeNativeResources(false);

        //Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            DisposeNativeResources(true);
        }

        /// <summary>
        /// Helper method to call from two locations.
        /// The actually disposal is performed here.
        /// The actual disposal is performed here.
        /// </summary>
        private void DisposeNativeResources(bool disposeManagedResources)
        {
            lock (_lock)
            {
                if (_disposed)
                    return;

                _disposed = true;

                if (disposeManagedResources)
                    CleanUpManagedResources();

                CleanUpNativeResources();
            }
        }

        /// <summary>
        /// Clean up managed resources.
        /// Handles, <see cref="Stream"/> and other <see cref="IDisposable"/>.
        /// </summary>
        protected abstract void CleanUpManagedResources();

        /// <summary>
        /// Clean up native resources, lists and set large fields to null.
        /// Usually fields and properties set in constructor.
        /// </summary>
        protected abstract void CleanUpNativeResources();
    }
}
