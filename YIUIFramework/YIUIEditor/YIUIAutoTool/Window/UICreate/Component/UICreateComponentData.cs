﻿#if UNITY_EDITOR

namespace YIUIFramework.Editor
{
    public class UICreateComponentData
    {
        public bool AutoRefresh;
        public bool ShowTips;
        public string Namespace; //命名空间
        public string PkgName; //包名/模块名
        public string ResName; //资源名 类名+Base
    }
}
#endif