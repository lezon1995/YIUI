using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    public static partial class YIUIFactory
    {
        public static async UniTask<T> InstantiateAsync<T>(Transform parent = null) where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo<T>(out var vo))
            {
                return await InstantiateAsync<T>(vo, parent);
            }

            return null;
        }

        public static async UniTask<UIBase> InstantiateAsync(Type uiType, Transform parent = null)
        {
            if (UIBindHelper.TryGetBindVo(uiType, out var vo))
            {
                return await InstantiateAsync(vo, parent);
            }

            return null;
        }


        public static async UniTask<UIBase> InstantiateAsync(string pkgName, string resName, RectTransform parent = null)
        {
            if (UIBindHelper.TryGetBindVo(pkgName, resName, out var vo))
            {
                return await InstantiateAsync(vo, parent);
            }

            return null;
        }


        static async UniTask<T> InstantiateAsync<T>(UIBindVo vo, Transform parent = null) where T : UIBase
        {
            return await CreateAsync(vo, parent) as T;
        }

        public static async UniTask<UIBase> InstantiateAsync(UIBindVo vo, Transform parent = null)
        {
            return await CreateAsync(vo, parent);
        }

        internal static async UniTask<UIPanel> CreatePanelAsync(PanelInfo panelInfo)
        {
            if (UIBindHelper.TryGetBindVo(panelInfo.PkgName, panelInfo.ResName, out var vo))
            {
                return await CreateAsync(vo) as UIPanel;
            }

            return null;
        }

        static async UniTask<UIBase> CreateAsync(UIBindVo vo, Transform parent = null)
        {
            var gameObject = await YIUILoadHelper.LoadAssetAsyncInstantiate(vo.PkgName, vo.ResName);
            if (gameObject)
            {
                return CreateByObjVo(vo, gameObject, parent);
            }

            Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
            return null;
        }
    }
}