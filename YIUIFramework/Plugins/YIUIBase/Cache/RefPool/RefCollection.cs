using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public static partial class RefPool
    {
        private sealed class RefCollection
        {
            private readonly Queue<IRefPool> m_Refs;
            private readonly Type m_RefType;

            internal RefCollection(Type refType)
            {
                m_Refs = new Queue<IRefPool>();
                m_RefType = refType;
            }

            internal Type RefType => m_RefType;

            internal T _Get<T>() where T : class, IRefPool, new()
            {
                if (typeof(T) != m_RefType)
                {
                    throw new Exception("类型无效");
                }

                lock (m_Refs)
                {
                    if (m_Refs.Count > 0)
                    {
                        return (T)m_Refs.Dequeue();
                    }
                }

                return new T();
            }

            internal IRefPool Get()
            {
                lock (m_Refs)
                {
                    if (m_Refs.Count > 0)
                    {
                        return m_Refs.Dequeue();
                    }
                }

                return (IRefPool)Activator.CreateInstance(m_RefType);
            }

            internal bool _Put(IRefPool iRef)
            {
                iRef.Recycle();
                lock (m_Refs)
                {
                    if (m_Refs.Contains(iRef))
                    {
                        return false;
                    }

                    m_Refs.Enqueue(iRef);
                    return true;
                }
            }

            internal void _Add<T>(int count) where T : class, IRefPool, new()
            {
                if (typeof(T) != m_RefType)
                {
                    throw new Exception("类型无效。");
                }

                lock (m_Refs)
                {
                    while (count-- > 0)
                    {
                        m_Refs.Enqueue(new T());
                    }
                }
            }

            internal void Add(int count)
            {
                lock (m_Refs)
                {
                    while (count-- > 0)
                    {
                        m_Refs.Enqueue((IRefPool)Activator.CreateInstance(m_RefType));
                    }
                }
            }

            internal void Remove(int count)
            {
                lock (m_Refs)
                {
                    if (count > m_Refs.Count)
                    {
                        count = m_Refs.Count;
                    }

                    while (count-- > 0)
                    {
                        m_Refs.Dequeue();
                    }
                }
            }

            internal void RemoveAll()
            {
                lock (m_Refs)
                {
                    m_Refs.Clear();
                }
            }
        }
    }
}