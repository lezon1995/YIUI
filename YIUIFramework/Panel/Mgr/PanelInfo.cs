using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 面板信息
    /// </summary>
    public class PanelInfo
    {
        public BasePanel Panel { get; private set; }

        public bool ActiveSelf => Panel is { ActiveSelf: true };

        /// <summary>
        /// 包名
        /// </summary>
        public string PkgName { get; }

        /// <summary>
        /// 资源名称 因为每个包分开 这个资源名称是有可能重复的 虽然设计上不允许  
        /// </summary>
        public string ResName { get; }

        /// <summary>
        /// C#文件名 因为有可能存在Res名称与文件名不一致的问题
        /// </summary>
        public string Name { get; }

        public PanelInfo(string panelName, string pkgName, string resName)
        {
            Name = panelName;
            PkgName = pkgName;
            ResName = resName;
        }

        internal void Reset(UIBase uiBase)
        {
            switch (uiBase)
            {
                case null:
                    Panel = null;
                    break;
                case BasePanel panel:
                    Panel = panel;
                    break;
                default:
                    Debug.LogError($"当前UI 不是Panel 请检查 {PkgName} {ResName}");
                    break;
            }
        }
    }
}