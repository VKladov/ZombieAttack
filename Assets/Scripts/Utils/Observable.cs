using System;

namespace Utils
{
    public class Observable<T>
    {
        public event Action<T> Changed;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke(value);
            }
        }

        private T _value;

        public Observable(T value)
        {
            _value = value;
        }
    }
}