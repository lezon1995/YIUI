﻿using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    public static partial class YIUIFactory
    {
        public static async UniTask<T> InstantiateAsync<T>(RectTransform parent = null) where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo<T>(out var vo))
            {
                return await InstantiateAsync<T>(vo, parent);
            }

            return null;
        }

        public static async UniTask<T> InstantiateAsync<T>(UIBindVo vo, RectTransform parent = null) where T : UIBase
        {
            var uiBase = await CreateAsync(vo);
            SetParent(uiBase.OwnerRectTransform, parent ? parent : PanelMgr.Inst.UICache);
            return (T)uiBase;
        }

        public static async UniTask<UIBase> InstantiateAsync(UIBindVo vo, RectTransform parent = null)
        {
            var uiBase = await CreateAsync(vo);
            SetParent(uiBase.OwnerRectTransform, parent ? parent : PanelMgr.Inst.UICache);
            return uiBase;
        }

        internal static async UniTask<UIBase> CreatePanelAsync(PanelInfo panelInfo)
        {
            if (UIBindHelper.TryGetBindVoByPath(panelInfo.PkgName, panelInfo.ResName, out var vo))
            {
                return await CreateAsync(vo);
            }

            return null;
        }

        private static async UniTask<UIBase> CreateAsync(UIBindVo vo)
        {
            var obj = await YIUILoadHelper.LoadAssetAsyncInstantiate(vo.PkgName, vo.ResName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
                return null;
            }

            return CreateByObjVo(vo, obj);
        }
    }
}