using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YIUIFramework
{
    /// <summary>
    /// 不使用泛型 使用type加载的方式
    /// </summary>
    internal static partial class YIUILoadHelper
    {
        static Object LoadAsset(string pkgName, string resName, Type assetType)
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();

            var loadObj = load.Object;
            if (loadObj)
            {
                return loadObj;
            }

            (Object obj, int hash) = YIUILoadDI.LoadAsset(pkgName, resName, assetType);
            if (obj == null)
            {
                load.RemoveRefCount();
                return null;
            }

            if (!LoadHelper.AddLoadHandle(obj, load))
            {
                load.RemoveRefCount();
                return null;
            }

            load.ResetHandle(obj, hash);
            return obj;
        }

        static async UniTask<Object> LoadAssetAsync(string pkgName, string resName, Type assetType)
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();

            var loadObj = load.Object;
            if (loadObj)
            {
                return loadObj;
            }

            if (load.WaitAsync)
            {
                await UniTask.WaitUntil(() => !load.WaitAsync);

                loadObj = load.Object;
                if (loadObj)
                {
                    return loadObj;
                }

                load.RemoveRefCount();
                return null;
            }

            load.SetWaitAsync(true);

            (Object obj, int hash) = await YIUILoadDI.LoadAssetAsync(pkgName, resName, assetType);
            if (obj == null)
            {
                load.SetWaitAsync(false);
                load.RemoveRefCount();
                return null;
            }

            if (!LoadHelper.AddLoadHandle(obj, load))
            {
                load.SetWaitAsync(false);
                load.RemoveRefCount();
                return null;
            }

            load.ResetHandle(obj, hash);
            load.SetWaitAsync(false);
            return obj;
        }

        static void LoadAssetAsync(string pkgName, string resName, Type assetType, Action<Object> action)
        {
            LoadAssetAsyncAction(pkgName, resName, assetType, action).Forget();
        }

        static async UniTaskVoid LoadAssetAsyncAction(string pkgName, string resName, Type assetType, Action<Object> action)
        {
            var asset = await LoadAssetAsync(pkgName, resName, assetType);
            if (asset == null)
            {
                Debug.LogError($"异步加载对象失败 {pkgName} {resName}");
                return;
            }

            action?.Invoke(asset);
        }
    }
}