using System;

namespace Samples
{
    public abstract class Lazy<T> where T : class
    {
        private readonly Func<T> _factoryMethod;
        protected readonly object Locker;
        protected T LazyLoaded;

        protected Lazy(Func<T> factoryMethod)
        {
            if (factoryMethod == null)
                throw new ArgumentNullException("factoryMethod");
            _factoryMethod = factoryMethod;
            Locker = new object();
        }


        protected abstract void OnCreatingValue();
        public T Value
        {
            get
            {
                lock (Locker)
                {
                    OnCreatingValue();
                    if (LazyLoaded == null)
                        LazyLoaded = _factoryMethod();
                    return LazyLoaded;
                }
            }
        }
    }
}