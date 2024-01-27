using System;

public struct bool_
{
    private event Action<bool, bool> _onChange;
    private bool _value;

    public void Set(bool value)
    {
        if (_value != value)
        {
            _onChange?.Invoke(_value, value);
            _value = value;
        }
    }

    public static implicit operator bool(bool_ self)
    {
        return self._value;
    }

    public void Listen(Action<bool, bool> action)
    {
        _onChange += action;
    }

    public void UnListen(Action<bool, bool> action)
    {
        _onChange -= action;
    }
}

public struct int_
{
    private event Action<int, int> _onChange;
    private int _value;

    public void Set(int value)
    {
        if (_value != value)
        {
            _onChange?.Invoke(_value, value);
            _value = value;
        }
    }

    public static implicit operator int(int_ self)
    {
        return self._value;
    }

    public void Listen(Action<int, int> action)
    {
        _onChange += action;
    }

    public void UnListen(Action<int, int> action)
    {
        _onChange -= action;
    }
}

public struct float_
{
    private const float TOLERANCE = 0.000000001F;
    private event Action<float, float> _onChange;
    private float _value;

    public void Set(float value)
    {
        if (Math.Abs(_value - value) > TOLERANCE)
        {
            _onChange?.Invoke(_value, value);
            _value = value;
        }
    }

    public static implicit operator float(float_ self)
    {
        return self._value;
    }

    public void Listen(Action<float, float> action)
    {
        _onChange += action;
    }

    public void UnListen(Action<float, float> action)
    {
        _onChange -= action;
    }
}

public struct string_
{
    private event Action<string, string> _onChange;
    private string _value;

    public void Set(string value)
    {
        if (_value != value)
        {
            _onChange?.Invoke(_value, value);
            _value = value;
        }
    }

    public static implicit operator string(string_ self)
    {
        return self._value;
    }

    public void Listen(Action<string, string> action)
    {
        _onChange += action;
    }

    public void UnListen(Action<string, string> action)
    {
        _onChange -= action;
    }
}