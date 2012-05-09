using System;

namespace Samples
{
    public class DisposedMarker
    {
        private readonly IDisposable _disposable;
        private bool _disposed;

        public DisposedMarker(IDisposable disposable)
        {
            _disposable = disposable;
        }

        public bool AlreadyDisposed
        {
            get { return _disposed; }
        }

        public void MarkDisposed()
        {
            _disposed = true;
        }

        public void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(_disposable.GetType().FullName);
        }
    }
}