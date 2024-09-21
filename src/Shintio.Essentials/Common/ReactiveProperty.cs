namespace Shintio.Essentials.Common
{
    public class ReactiveProperty<T>
    {
        public delegate void ChangedDelegate(T newValue);

        public event ChangedDelegate? Changed;

        private T _value;

        public ReactiveProperty(T initialValue)
        {
            _value = initialValue;
        }

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke(_value);
            }
        }

        public void SetValueSilently(T value)
        {
            _value = value;
        }
        }
    }
}