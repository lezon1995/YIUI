﻿//#define YIUIMACRO_SIMULATE_NONEEDITOR //模拟非编辑器状态  在编辑器使用 非编辑器加载模式 用于在编辑器下测试  

using System;
using System.Collections.Generic;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// UI关联帮助类
    /// </summary>
    public static class UIBindHelper
    {
        /// <summary>
        /// 根据创建时的类获取
        /// </summary>
        static Dictionary<Type, UIBindVo> typeVoDict = new Dictionary<Type, UIBindVo>();

        /// <summary>
        /// 根据 pkg + res 双字典获取
        /// </summary>
        static Dictionary<Key, UIBindVo> pathVoDict = new Dictionary<Key, UIBindVo>();

        /// <summary>
        /// 只有panel 的信息
        /// </summary>
        static Dictionary<string, UIBindVo> panelVoDict = new Dictionary<string, UIBindVo>();

        //改为dll过后 提供给外部的方法
        //1 从UI工具中自动生成绑定代码
        //2 外部请直接调用此方法 UIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.UIBindProvider.Get;
        public static Func<UIBindVo[]> InternalGameGetUIBindVoFunc { internal get; set; }

        //初始化记录
        public static bool IsInit { get; private set; }

        public static Type TypePanel = typeof(UIPanel);

        /// <summary>
        /// 初始化获取到所有UI相关的绑定关系
        /// Editor下是反射
        /// 其他 是序列化的文件 打包的时候一定要生成一次文件
        /// </summary>
        internal static bool InitAllBind()
        {
            if (IsInit)
            {
                Debug.LogError($"已经初始化过了 请检查");
                return false;
            }

#if !UNITY_EDITOR || YIUIMACRO_SIMULATE_NONEEDITOR
            if (InternalGameGetUIBindVoFunc == null)
            {
                Debug.LogError($"使用非反射注册绑定 但是方法未实现 请检查");
                return false;
            }
            var binds = InternalGameGetUIBindVoFunc?.Invoke();
#else
            var binds = new UIBindProvider().Get();
#endif

            if (binds == null || binds.Length == 0)
            {
                Debug.LogError("没有找到绑定信息 或者 没有绑定信息 请检查");
                return false;
            }

            typeVoDict = new Dictionary<Type, UIBindVo>(binds.Length);
            pathVoDict = new Dictionary<Key, UIBindVo>();
            panelVoDict = new Dictionary<string, UIBindVo>(binds.Length);

            foreach (var vo in binds)
            {
                typeVoDict[vo.CreatorType] = vo;
                AddToPathDic(vo);
                if (vo.CodeType == TypePanel)
                {
                    panelVoDict[vo.ResName] = vo;
                }
            }

            IsInit = true;
            return true;
        }

        static void AddToPathDic(UIBindVo vo)
        {
            var pkgName = vo.PkgName;
            var resName = vo.ResName;
            var key = new Key(pkgName, resName);

            if (pathVoDict.TryAdd(key, vo) == false)
            {
                Debug.LogError($"重复资源 请检查 {pkgName} {resName}");
            }
        }

        /// <summary>
        /// 得到UI包信息
        /// </summary>
        public static bool TryGetBindVo(Type uiType, out UIBindVo uiBindVo)
        {
            uiBindVo = default;
            if (uiType == null)
            {
                Debug.LogError($"空 无法取到这个包信息 请检查");
                return false;
            }

            if (typeVoDict.TryGetValue(uiType, out var vo))
            {
                uiBindVo = vo;
                return true;
            }

            Debug.LogError($"未获取到这个UI包信息 请检查  {uiType.Name}");
            return false;
        }

        /// <summary>
        /// 得到UI包信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetBindVo<T>(out UIBindVo uiBindVo)
        {
            return TryGetBindVo(typeof(T), out uiBindVo);
        }

        /// <summary>
        /// 根据唯一ID获取
        /// 由pkg+res 拼接的唯一ID
        /// </summary>
        public static bool TryGetBindVo(string pkgName, string resName, out UIBindVo uiBindVo)
        {
            uiBindVo = default;

            if (pkgName.IsEmpty() || resName.IsEmpty())
            {
                Debug.LogError($"空名称 无法取到这个包信息 请检查");
                return false;
            }

            var key = new Key(pkgName, resName);
            if (pathVoDict.TryGetValue(key, out var vo))
            {
                uiBindVo = vo;
                return true;
            }

            Debug.LogError($"未获取到这个包信息 请检查  {pkgName} {resName}");
            return false;
        }

        /// <summary>
        /// 根据panelName获取
        /// 只有是panel才会存在的信息
        /// 非Panel请使用其他
        /// </summary>
        internal static bool TryGetBindVo(string panelName, out UIBindVo uiBindVo)
        {
            uiBindVo = default;

            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogError($"空名称 无法取到这个包信息 请检查");
                return false;
            }

            if (panelVoDict.TryGetValue(panelName, out var vo))
            {
                uiBindVo = vo;
                return true;
            }

            Debug.LogError($"未获取到这个包信息 请检查  {panelName}");
            return false;
        }

        /// <summary>
        /// 重置 慎用
        /// </summary>
        internal static void Reset()
        {
            if (typeVoDict != null)
            {
                typeVoDict.Clear();
                typeVoDict = null;
            }

            if (pathVoDict != null)
            {
                pathVoDict.Clear();
                pathVoDict = null;
            }

            if (panelVoDict != null)
            {
                panelVoDict.Clear();
                panelVoDict = null;
            }

            IsInit = false;
        }
    }

    public struct Key : IEquatable<Key>
    {
        //包名/模块名
        string Pkg;

        //资源名
        string Res;

        public Key(string pkg, string res)
        {
            Pkg = pkg;
            Res = res;
        }

        public bool Equals(Key other)
        {
            return Pkg == other.Pkg && Res == other.Res;
        }

        public override bool Equals(object obj)
        {
            return obj is Key other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Pkg, Res);
        }
    }
}