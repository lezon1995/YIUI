using System;

namespace YIUIFramework
{
    public struct int_
    {
        private event Action<int, int> onChanged;
        private int _value;

        public void Set(int value)
        {
            if (_value != value)
            {
                onChanged?.Invoke(_value, value);
                _value = value;
            }
        }

        public void Bind(Action<int, int> action)
        {
            onChanged += action;
        }

        public void Unbind(Action<int, int> action)
        {
            onChanged -= action;
        }

        public static implicit operator int(int_ self)
        {
            return self._value;
        }
    }
}