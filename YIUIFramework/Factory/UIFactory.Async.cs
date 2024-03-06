using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    public static partial class UIFactory
    {
        public static async UniTask<T> InstantiateAsync<T>(Transform parent = null) where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo<T>(out var vo))
            {
                return await CreateAsync(vo, parent) as T;
            }

            return null;
        }

        public static async UniTask<UIBase> InstantiateAsync(Type type, Transform parent = null)
        {
            if (UIBindHelper.TryGetBindVo(type, out var vo))
            {
                return await CreateAsync(vo, parent);
            }

            return null;
        }

        public static async UniTask<T> InstantiateAsync<T>(string pkgName, string resName, RectTransform parent = null) where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo(pkgName, resName, out var vo))
            {
                return await CreateAsync(vo, parent) as T;
            }

            return null;
        }

        public static async UniTask<UIBase> InstantiateAsync(string pkgName, string resName, RectTransform parent = null)
        {
            if (UIBindHelper.TryGetBindVo(pkgName, resName, out var vo))
            {
                return await CreateAsync(vo, parent);
            }

            return null;
        }

        public static async UniTask<T> InstantiateAsync<T>(UIBindVo vo, Transform parent = null) where T : UIBase
        {
            return await CreateAsync(vo, parent) as T;
        }

        static async UniTask<UIBase> CreateAsync(UIBindVo vo, Transform parent = null)
        {
            var gameObject = await YIUILoadHelper.LoadAssetAsyncInstantiate(vo.PkgName, vo.ResName);
            if (gameObject)
            {
                return InitializeUI(vo, gameObject, parent);
            }

            Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
            return null;
        }
    }
}