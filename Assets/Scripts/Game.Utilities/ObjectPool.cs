using System;
using System.Collections.Generic;

namespace Game.Utilities
{
    public abstract class ObjectPool<T> : IDisposable where T : UnityEngine.Component
    {
        private bool _isDisposed;
        private Queue<T> _q;

        protected int MaxPoolCount => int.MaxValue;

        protected abstract T CreateInstance();

        protected virtual void OnBeforeRent(T instance) => instance.gameObject.SetActive(true);

        protected virtual void OnBeforeReturn(T instance) => instance.gameObject.SetActive(false);

        protected virtual void OnClear(T instance)
        {
            if (instance == null)
            {
                return;
            }

            var go = instance.gameObject;
            if (go == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(go);
        }

        public int Count => _q?.Count ?? 0;

        public T Rent()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("ObjectPool was already disposed.");
            }

            _q ??= new Queue<T>();

            var instance = _q.Count > 0
                ? _q.Dequeue()
                : CreateInstance();

            OnBeforeRent(instance);
            return instance;
        }

        public void Return(T instance)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("ObjectPool was already disposed.");
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            _q ??= new Queue<T>();

            if (_q.Count + 1 == MaxPoolCount)
            {
                throw new InvalidOperationException("Reached Max PoolSize");
            }

            OnBeforeReturn(instance);
            _q.Enqueue(instance);
        }

        public void Clear(bool callOnBeforeRent = false)
        {
            if (_q == null)
            {
                return;
            }

            while (_q.Count != 0)
            {
                var instance = _q.Dequeue();
                if (callOnBeforeRent)
                {
                    OnBeforeRent(instance);
                }

                OnClear(instance);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                Clear(false);
            }

            _isDisposed = true;
        }

        public void Dispose() => Dispose(true);
    }
}