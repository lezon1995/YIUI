using System;
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
            if (UIBindHelper.TryGetBindVo(pkgName, resName, out var vo))
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
            if (UIBindHelper.TryGetBindVo(pkgName, resName, out var vo))
            {
                return Create(vo);
            }

            return null;
        }

        static UIBase Create(UIBindVo vo, Transform parent = null)
        {
            var gameObject = YIUILoadHelper.LoadAssetInstantiate(vo.PkgName, vo.ResName);
            if (gameObject)
            {
                return CreateByObjVo(vo, gameObject, parent);
            }

            Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
            return null;
        }

        static UIBase CreateByObjVo(UIBindVo vo, GameObject gameObject, Transform parent = null)
        {
            if (gameObject.TryGetComponent<UITable>(out var table))
            {
                table.NewChildUIComponents();
                var uiBase = (UIBase)Activator.CreateInstance(vo.CreatorType);
                SetParent(gameObject.transform, parent ? parent : PanelMgr.Inst.UICache);
                uiBase.InitUIBase(vo, gameObject);
                return uiBase;
            }

            Debug.LogError($"{gameObject.name} 没有 UITable 组件 无法创建 请检查");
            return null;
        }

        static void NewChildUIComponents(this UITable self)
        {
            foreach (var table in self.ChildTables)
            {
                if (table == null)
                {
                    Debug.LogError($"{self.name} 存在null对象的childTable 检查是否因为删除或丢失或未重新生成");
                    continue;
                }

                if (UIBindHelper.TryGetBindVo(table.PkgName, table.ResName, out var vo))
                {
                    var uiBase = (UIBase)Activator.CreateInstance(vo.CreatorType);
                    table.NewChildUIComponents();
                    uiBase.InitUIBase(vo, table.gameObject);
                    self.AddUIBase(table.gameObject.name, uiBase);
                }
            }
        }
    }
}