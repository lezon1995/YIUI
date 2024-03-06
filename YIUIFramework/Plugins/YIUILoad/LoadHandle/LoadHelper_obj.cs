using System.Collections.Generic;
using UnityEngine;

namespace YIUIFramework
{
    internal static partial class LoadHelper
    {
        static Dictionary<Object, LoadHandle> objHandles = new Dictionary<Object, LoadHandle>();

        internal static bool AddLoadHandle(Object obj, LoadHandle handle)
        {
            if (!objHandles.TryAdd(obj, handle))
            {
                Debug.LogError($"此obj {obj.name} Handle 已存在 请检查 请勿创建多个");
                return false;
            }

            return true;
        }

        private static bool RemoveLoadHandle(LoadHandle handle)
        {
            var obj = handle.Object;
            if (obj)
            {
                return RemoveLoadHandle(obj);
            }

            return false;
        }

        private static bool RemoveLoadHandle(Object obj)
        {
            if (objHandles.ContainsKey(obj))
            {
                return objHandles.Remove(obj);
            }

            Debug.LogError($"此obj {obj.name} Handle 不存在 请检查 请先创建设置");
            return false;
        }

        internal static LoadHandle GetLoadHandle(Object obj)
        {
            if (objHandles.TryGetValue(obj, out var handle))
            {
                return handle;
            }

            Debug.LogError($"此obj {obj.name} Handle 不存在 请检查 请先创建设置");
            return null;
        }
    }
}