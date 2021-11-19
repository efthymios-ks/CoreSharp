using System;
using System.IO;

namespace CoreSharp.Abstracts
{
    /// <summary>
    /// Automatic and safe disposing.
    /// Just override the two clean-up methods.
    /// </summary>
    public abstract class DisposableBase : IDisposable
    {
        //Fields
        private readonly object _lock = new();
        private bool _disposed;

        //Constructors
        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~DisposableBase()
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
        /// The actually disposal is performed here.
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
                catch
                {
                }
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
