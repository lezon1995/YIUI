using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// 对象缓存池
    /// </summary>
    public class ObjCache<T>
    {
        Stack<T> m_pool;
        protected Func<T> m_createCallback;

        public ObjCache(Func<T> createCallback, int capacity = 0)
        {
            m_pool = capacity > 0
                ? new Stack<T>(capacity)
                : new Stack<T>();

            m_createCallback = createCallback;
        }

        public T Get()
        {
            return m_pool.Count > 0 ? m_pool.Pop() : m_createCallback();
        }

        public void Put(T value)
        {
            m_pool.Push(value);
        }

        public void Clear(bool disposeItem = false)
        {
            if (disposeItem)
            {
                foreach (var item in m_pool)
                {
                    switch (item)
                    {
                        case IDisposer disposer:
                            disposer.Dispose();
                            break;
                        case IDisposable disposer2:
                            disposer2.Dispose();
                            break;
                    }
                }
            }

            m_pool.Clear();
        }

        public void Clear(Action<T> disposeAction)
        {
            while (m_pool.Count >= 1)
            {
                disposeAction?.Invoke(m_pool.Pop());
            }

            m_pool.Clear();
        }
    }
}