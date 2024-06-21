using System;
using UnityEngine;

namespace YIUIFramework
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>, new()
    {
        private static T g_Inst;
        public static bool Exist => g_Inst != null;
        public bool Disposed { get; private set; }

        protected Singleton()
        {
            if (Exist)
            {
#if UNITY_EDITOR
                throw new Exception(this + "是单例类，不能实例化两次");
#endif
            }
        }

        public static T Inst
        {
            get
            {
                if (!Exist)
                {
                    if (SingletonMgr.Disposing)
                    {
                        Debug.LogError($" {typeof(T).Name} 单例管理器已释放或未初始化 禁止使用");
                        return null;
                    }

                    g_Inst = new T();
                    g_Inst.OnInitSingleton();
                    SingletonMgr.Add(g_Inst);
                }

                g_Inst.OnUseSingleton();
                return g_Inst;
            }
        }

        //释放方法2: 静态释放
        public static bool DisposeInst()
        {
            if (g_Inst == null)
            {
                return true;
            }

            return g_Inst.Dispose();
        }

        //释放方法1: 对象释放
        public bool Dispose()
        {
            if (Disposed)
            {
                return false;
            }

            SingletonMgr.Remove(g_Inst);
            g_Inst = null;
            Disposed = true;
            OnDispose();
            return true;
        }

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnInitSingleton()
        {
        }

        protected virtual void OnUseSingleton()
        {
        }
    }
}