using System;
using UnityEngine;

namespace YIUIFramework
{
    public static partial class YIUIFactory
    {
        public static T Instantiate<T>(RectTransform parent = null) where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo<T>(out var vo))
            {
                return Instantiate<T>(vo, parent);
            }

            return null;
        }

        public static T Instantiate<T>(UIBindVo vo, RectTransform parent = null) where T : UIBase
        {
            var instance = (T)Create(vo);
            if (instance == null)
            {
                return null;
            }

            SetParent(instance.OwnerRectTransform, parent ? parent : PanelMgr.Inst.UICache);
            return instance;
        }

        private static void SetParent(RectTransform self, RectTransform parent)
        {
            self.SetParent(parent, false);
            self.AutoReset();
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

        private static T Create<T>() where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo<T>(out var vo))
            {
                return (T)Create(vo);
            }

            return null;
        }

        private static UIBase Create(string pkgName, string resName)
        {
            if (UIBindHelper.TryGetBindVoByPath(pkgName, resName, out var vo))
            {
                return Create(vo);
            }

            return null;
        }

        private static UIBase Create(UIBindVo vo)
        {
            var obj = YIUILoadHelper.LoadAssetInstantiate(vo.PkgName, vo.ResName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 {vo.PkgName}/{vo.ResName}");
                return null;
            }

            return CreateByObjVo(vo, obj);
        }

        private static UIBase CreateByObjVo(UIBindVo vo, GameObject obj)
        {
            var cdeTable = obj.GetComponent<UIBindCDETable>();
            if (cdeTable == null)
            {
                Debug.LogError($"{obj.name} 没有 UIBindCDETable 组件 无法创建 请检查");
                return null;
            }

            cdeTable.CreateComponent();
            var uiBase = (UIBase)Activator.CreateInstance(vo.CreatorType);
            uiBase.InitUIBase(vo, obj);
            return uiBase;
        }

        private static void CreateComponent(this UIBindCDETable cdeTable)
        {
            foreach (var childCde in cdeTable.AllChildCdeTable)
            {
                if (childCde == null)
                {
                    Debug.LogError($"{cdeTable.name} 存在null对象的childCde 检查是否因为删除或丢失或未重新生成");
                    continue;
                }

                if (UIBindHelper.TryGetBindVoByPath(childCde.PkgName, childCde.ResName, out var vo))
                {
                    var childBase = (UIBase)Activator.CreateInstance(vo.CreatorType);
                    childCde.CreateComponent();
                    childBase.InitUIBase(vo, childCde.gameObject);
                    cdeTable.AddUIBase(childCde.gameObject.name, childBase);
                }
            }
        }
    }
}