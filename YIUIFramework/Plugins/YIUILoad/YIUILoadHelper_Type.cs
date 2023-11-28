﻿using System;
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
        internal static Object LoadAsset(string pkgName, string resName, Type assetType)
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            var loadObj = load.Object;
            if (loadObj)
            {
                load.AddRefCount();
                return loadObj;
            }

            (Object obj, int hash) = YIUILoadDI.LoadAsset(pkgName, resName, assetType);
            if (obj == null)
            {
                load.RemoveRefCount();
                return null;
            }

            if (LoadHelper.AddLoadHandle(obj, load))
            {
                load.ResetHandle(obj, hash);
                load.AddRefCount();
                return obj;
            }

            return null;
        }

        internal static async UniTask<Object> LoadAssetAsync(string pkgName, string resName, Type assetType)
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            var loadObj = load.Object;
            if (loadObj)
            {
                load.AddRefCount();
                return loadObj;
            }

            if (load.WaitAsync)
            {
                await UniTask.WaitUntil(() => !load.WaitAsync);

                loadObj = load.Object;
                if (loadObj)
                {
                    load.AddRefCount();
                    return loadObj;
                }

                Debug.LogError($"错误这个时候不应该是null");
            }

            load.SetWaitAsync(true);

            (Object obj, int hash) = await YIUILoadDI.LoadAssetAsync(pkgName, resName, assetType);
            if (obj == null)
            {
                load.RemoveRefCount();
                return null;
            }

            if (LoadHelper.AddLoadHandle(obj, load))
            {
                load.ResetHandle(obj, hash);
                load.AddRefCount();
                load.SetWaitAsync(false);
                return obj;
            }

            return null;
        }

        internal static void LoadAssetAsync(string pkgName, string resName, Type assetType, Action<Object> action)
        {
            LoadAssetAsyncAction(pkgName, resName, assetType, action).Forget();
        }

        private static async UniTaskVoid LoadAssetAsyncAction(string pkgName, string resName, Type assetType, Action<Object> action)
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