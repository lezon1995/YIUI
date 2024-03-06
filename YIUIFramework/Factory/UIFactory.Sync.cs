using System;
using UnityEngine;

namespace YIUIFramework
{
    public static partial class UIFactory
    {
        public static T Instantiate<T>(Transform parent = null) where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo<T>(out var vo))
            {
                return Create(vo, parent) as T;
            }

            return null;
        }


        public static UIBase Instantiate(Type type, Transform parent = null)
        {
            if (UIBindHelper.TryGetBindVo(type, out var vo))
            {
                return Create(vo, parent);
            }

            return null;
        }

        public static UIBase Instantiate(string pkgName, string resName, Transform parent = null)
        {
            if (UIBindHelper.TryGetBindVo(pkgName, resName, out var vo))
            {
                return Create(vo, parent);
            }

            return null;
        }

        public static T Instantiate<T>(string pkgName, string resName, Transform parent = null) where T : UIBase
        {
            if (UIBindHelper.TryGetBindVo(pkgName, resName, out var vo))
            {
                return Create(vo, parent) as T;
            }

            return null;
        }

        public static T Instantiate<T>(UIBindVo vo, Transform parent = null) where T : UIBase
        {
            return Create(vo, parent) as T;
        }
    }
}