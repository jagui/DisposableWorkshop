using System;

namespace Samples
{
    public abstract class DisposableLazy<T> : Lazy<T>, IDisposable where T : class, IDisposable
    {
        private readonly DisposedMarker _disposedMarker;

        protected DisposableLazy(Func<T> factoryMethod)
            : base(factoryMethod)
        {
            _disposedMarker = new DisposedMarker(this);
        }

        protected override void OnCreatingValue()
        {
            _disposedMarker.ThrowIfDisposed();
        }

        public void Dispose()
        {
            lock (Locker)
            {
                if (_disposedMarker.AlreadyDisposed)
                    return;
                if (LazyLoaded != null)
                    LazyLoaded.Dispose();
                _disposedMarker.MarkDisposed();
            }
        }
    }
}