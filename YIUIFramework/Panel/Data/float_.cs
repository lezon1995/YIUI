using System;

namespace YIUIFramework
{
    public struct float_
    {
        private const float TOLERANCE = 0.000000001F;
        private event Action<float, float> onChanged;
        private float _value;

        public void Set(float value)
        {
            if (Math.Abs(_value - value) > TOLERANCE)
            {
                onChanged?.Invoke(_value, value);
                _value = value;
            }
        }

        public void Bind(Action<float, float> action)
        {
            onChanged += action;
        }

        public void Unbind(Action<float, float> action)
        {
            onChanged -= action;
        }

        public static implicit operator float(float_ self)
        {
            return self._value;
        }
    }
}