﻿using UnityEngine;

namespace YIUIFramework
{
    public static partial class YIUIFactory
    {
        internal static void Destroy(GameObject obj)
        {
            YIUILoadHelper.ReleaseInstantiate(obj);
        }

        //内部会自动调用
        //一定要使用本类中的创建 否则会有报错提示
        internal static void Destroy(UIBase uiBase)
        {
            if (uiBase.OwnerGameObject)
            {
                Destroy(uiBase.OwnerGameObject);
            }
            else
            {
                Debug.LogError($"此UI 是空对象 请检查{uiBase.UIBindVo.PkgName} {uiBase.UIBindVo.ResName}");
            }
        }
    }
}