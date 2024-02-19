﻿using System;
using UnityEngine;

namespace YIUIFramework
{
    public static partial class YIUIFactory
    {
        static void SetParent(Transform self, Transform parent)
        {
            self.SetParent(parent, false);
            if (self is RectTransform rectTransform)
            {
                rectTransform.AutoReset();
            }
        }

        internal static UIBase CreateCommon(string pkgName, string resName, GameObject obj)
        {
            if (UIBindHelper.TryGetBindVoByPath(pkgName, resName, out var vo))
            {
                return CreateByObjVo(vo, obj);
            }

            return null;
        }

        internal static UIBase CreatePanel(PanelInfo panelInfo)
        {
            return Create(panelInfo.PkgName, panelInfo.ResName);
        }

        static T Create<T>() where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo<T>(out var vo))
            {
                return Create(vo) as T;
            }

            return null;
        }

        static UIBase Create(string pkgName, string resName)
        {
            if (UIBindHelper.TryGetBindVoByPath(pkgName, resName, out var vo))
            {
                return Create(vo);
            }

            return null;
        }

        static UIBase Create(UIBindVo vo, Transform parent = null)
        {
            var obj = YIUILoadHelper.LoadAssetInstantiate(vo.PkgName, vo.ResName);
            if (obj)
            {
                return CreateByObjVo(vo, obj, parent);
            }

            Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
            return null;
        }

        static UIBase CreateByObjVo(UIBindVo vo, GameObject obj, Transform parent = null)
        {
            var table = obj.GetComponent<UIBindCDETable>();
            if (table)
            {
                table.CreateComponent();
                var uiBase = (UIBase)Activator.CreateInstance(vo.CreatorType);
                SetParent(obj.transform, parent ? parent : PanelMgr.Inst.UICache);
                uiBase.InitUIBase(vo, obj);
                return uiBase;
            }

            Debug.LogError($"{obj.name} 没有 UIBindCDETable 组件 无法创建 请检查");
            return null;
        }

        static void CreateComponent(this UIBindCDETable cdeTable)
        {
            foreach (var childTable in cdeTable.ChildTables)
            {
                if (childTable == null)
                {
                    Debug.LogError($"{cdeTable.name} 存在null对象的childCde 检查是否因为删除或丢失或未重新生成");
                    continue;
                }

                if (UIBindHelper.TryGetBindVoByPath(childTable.PkgName, childTable.ResName, out var vo))
                {
                    var childBase = (UIBase)Activator.CreateInstance(vo.CreatorType);
                    childTable.CreateComponent();
                    childBase.InitUIBase(vo, childTable.gameObject);
                    cdeTable.AddUIBase(childTable.gameObject.name, childBase);
                }
            }
        }
    }
}