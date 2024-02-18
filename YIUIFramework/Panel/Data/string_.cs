using System;

namespace YIUIFramework
{
    public struct string_
    {
        private event Action<string, string> onChanged;
        private string _value;

        public void Set(string value)
        {
            if (_value != value)
            {
                onChanged?.Invoke(_value, value);
                _value = value;
            }
        }

        public void Bind(Action<string, string> action)
        {
            onChanged += action;
        }

        public void Unbind(Action<string, string> action)
        {
            onChanged -= action;
        }

        public static implicit operator string(string_ self)
        {
            return self._value;
        }
    }
}