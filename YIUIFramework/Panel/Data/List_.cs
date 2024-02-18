using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class List_<T>
    {
        private event Action<T> onAdd;
        private event Action<T> onRemove;
        private event Action<T, T> onChange;

        private readonly List<T> _list = new List<T>();

        public T this[int index]
        {
            get { return _list[index]; }
            set
            {
                T pre = _list[index];
                if (!Equals(pre, value))
                {
                    _list[index] = value;
                    onChange?.Invoke(pre, value);
                }
            }
        }

        public void Add(T item)
        {
            _list.Add(item);
            onAdd?.Invoke(item);
        }

        public void Remove(T item)
        {
            if (_list.Remove(item))
            {
                onRemove?.Invoke(item);
            }
        }

        public void BindAdd(Action<T> action)
        {
            onAdd += action;
        }

        public void UnbindAdd(Action<T> action)
        {
            onAdd -= action;
        }

        public void BindRemove(Action<T> action)
        {
            onRemove += action;
        }

        public void UnbindRemove(Action<T> action)
        {
            onRemove -= action;
        }

        public void Bind(Action<T, T> action)
        {
            onChange += action;
        }

        public void Unbind(Action<T, T> action)
        {
            onChange -= action;
        }

        public static implicit operator List<T>(List_<T> self)
        {
            return self._list;
        }
    }
}