using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 同步加载
    /// </summary>
    internal static partial class YIUILoadHelper
    {
        /*
         * 这些方法都是给框架内部使用的 内部自行管理
         * 禁止  internal 改 public
         * 外部有什么加载 应该走自己框架中的加载方式 自行管理
         * 比如你想自己new一个obj 既然不是用UI框架内部提供的方法 那就应该你自行实现不要调用这个类
         */
        static T LoadAsset<T>(string pkgName, string resName) where T : Object
        {
            var handle = LoadHelper.GetLoad(pkgName, resName);
            handle.AddRefCount();

            var loadObj = handle.Object;
            if (loadObj)
            {
                return (T)loadObj;
            }

            (Object obj, int hash) = YIUILoadDI.LoadAsset(pkgName, resName, typeof(T));
            if (obj == null)
            {
                handle.RemoveRefCount();
                return null;
            }

            if (!LoadHelper.AddLoadHandle(obj, handle))
            {
                handle.RemoveRefCount();
                return null;
            }

            handle.ResetHandle(obj, hash);
            return (T)obj;
        }
    }
}