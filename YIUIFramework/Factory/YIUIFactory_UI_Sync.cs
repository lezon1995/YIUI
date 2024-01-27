using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public static partial class YIUIFactory
    {
        public static T Instantiate<T>(Transform parent = null) where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo<T>(out var vo))
            {
                return Instantiate<T>(vo, parent);
            }

            return null;
        }


        public static UIBase Instantiate(Type uiType, Transform parent = null)
        {
            if (UIBindHelper.TryGetBindVo(uiType, out var vo))
            {
                return Instantiate(vo, parent);
            }

            return null;
        }

        public static UIBase Instantiate(string pkgName, string resName, Transform parent = null)
        {
            if (UIBindHelper.TryGetBindVoByPath(pkgName, resName, out var vo))
            {
                return Instantiate(vo, parent);
            }

            return null;
        }

        public static T Instantiate<T>(UIBindVo vo, Transform parent = null) where T : UIBase
        {
            return Create(vo, parent) as T;
        }

        private static UIBase Instantiate(UIBindVo vo, Transform parent = null)
        {
            return Create(vo, parent);
        }
    }
}