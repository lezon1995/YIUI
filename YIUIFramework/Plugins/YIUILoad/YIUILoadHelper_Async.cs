﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YIUIFramework
{
    /// <summary>
    /// 异步加载
    /// </summary>
    internal static partial class YIUILoadHelper
    {
        /// <summary>
        /// 异步加载资源对象
        /// 返回类型
        /// </summary>
        static async UniTask<T> LoadAssetAsync<T>(string pkgName, string resName) where T : Object
        {
            var handle = LoadHelper.GetLoad(pkgName, resName);
            handle.AddRefCount();
            var loadObj = handle.Object;
            if (loadObj)
            {
                return (T)loadObj;
            }

            if (handle.WaitAsync)
            {
                await UniTask.WaitUntil(() => !handle.WaitAsync);

                loadObj = handle.Object;
                if (loadObj)
                {
                    handle.AddRefCount();
                    return (T)loadObj;
                }

                handle.RemoveRefCount();
                return null;
            }

            handle.SetWaitAsync(true);

            (Object obj, int hash) = await YIUILoadDI.LoadAssetAsync(pkgName, resName, typeof(T));

            if (obj == null)
            {
                handle.SetWaitAsync(false);
                handle.RemoveRefCount();
                return null;
            }

            if (!LoadHelper.AddLoadHandle(obj, handle))
            {
                handle.SetWaitAsync(false);
                handle.RemoveRefCount();
                return null;
            }

            handle.ResetHandle(obj, hash);
            handle.SetWaitAsync(false);
            return (T)obj;
        }

        /// <summary>
        /// 异步加载资源对象
        /// 回调类型
        /// </summary>
        internal static void LoadAssetAsync<T>(string pkgName, string resName, Action<T> action) where T : Object
        {
            LoadAssetAsyncAction(pkgName, resName, action).Forget();
        }

        private static async UniTaskVoid LoadAssetAsyncAction<T>(string pkgName, string resName, Action<T> action) where T : Object
        {
            var asset = await LoadAssetAsync<T>(pkgName, resName);
            if (asset)
            {
                action?.Invoke(asset);
            }
            else
            {
                Debug.LogError($"异步加载对象失败 {pkgName} {resName}");
            }
        }
    }
}