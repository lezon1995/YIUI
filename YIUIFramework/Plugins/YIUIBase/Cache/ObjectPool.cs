using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YIUIFramework
{
    public class ObjectPool<T> where T : new()
    {
#if UNITY_EDITOR
        public static bool isRunning = false;
        readonly HashSet<T> Trace = new HashSet<T>();
#endif

        readonly Stack<T> m_Stack = new Stack<T>();
        readonly UnityAction<T> m_ActionOnGet;
        readonly UnityAction<T> m_ActionOnRelease;

        public int countAll { get; private set; }

        public int countActive
        {
            get { return countAll - countInactive; }
        }

        public int countInactive
        {
            get { return m_Stack.Count; }
        }

        public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
        {
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
        }

        public T Get()
        {
            T element;
            if (m_Stack.Count == 0)
            {
                element = new T();
                countAll++;
#if UNITY_EDITOR
                Trace.Add(element);
#endif
            }
            else
            {
                element = m_Stack.Pop();
            }

            m_ActionOnGet?.Invoke(element);
            return element;
        }

        public void Release(T element)
        {
#if UNITY_EDITOR
            if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
            {
                Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            }

            if (element == null)
            {
                Debug.LogError("Internal error. Release element is null.");
                return;
            }

            if (ObjectPool<bool>.isRunning == false)
            {
                m_ActionOnRelease?.Invoke(element);
                return;
            }

            if (Trace.Contains(element) == false)
            {
                Debug.LogError("Internal error. Release element is not from pool ");
            }
#endif

            m_ActionOnRelease?.Invoke(element);
            m_Stack.Push(element);
        }
    }
}