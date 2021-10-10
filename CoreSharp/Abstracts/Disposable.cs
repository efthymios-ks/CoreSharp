using System;

namespace CoreSharp.Abstracts
{
    /// <summary>
    /// Automatic and safe Disposing.
    /// Just override the two CleanUp methods.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        //Fields
        private readonly object _lock = new();
        private bool _disposed;

        //Constructors
        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~Disposable()
        {
            DisposeNativeResources(false);
        }

        //Methods
        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            DisposeNativeResources(true);
        }

        /// <summary>
        /// Helper method to call from two locations.
        /// The actually Disposal is performed here.
        /// </summary>
        private void DisposeNativeResources(bool disposeManagedResources)
        {
            lock (_lock)
            {
                if (_disposed)
                    return;

                _disposed = true;

                try
                {
                    if (disposeManagedResources)
                        CleanUpManagedResources();

                    CleanUpNativeResources();
                }
                catch { }
            }
        }

        /// <summary>
        /// Clean up managed resources.
        /// Handles, Streams and other IDisposables.
        /// </summary>
        protected abstract void CleanUpManagedResources();

        /// <summary>
        /// Clean up native resources, lists and set large fields to null.
        /// Usually fields and properties set in Constructor.
        /// </summary>
        protected abstract void CleanUpNativeResources();
    }
}
