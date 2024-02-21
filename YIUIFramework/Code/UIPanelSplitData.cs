using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 面板拆分数据
    /// 主要做分块加载
    /// </summary>
    [HideReferenceObjectPicker]
    [HideLabel]
    public sealed partial class UIPanelSplitData
    {
        [HideInInspector]
        public GameObject Panel;

        [BoxGroup("TabView")]
        [ReadOnly]
        public RectTransform ViewTabsParent;

        [BoxGroup("TabView")]
        [LabelText("TabView(Static)")]
        public List<RectTransform> ViewTabsStatic = new List<RectTransform>();

        [BoxGroup("TabView")]
        [LabelText("TabView(Dynamic)")]
        public List<RectTransform> ViewTabs = new List<RectTransform>();

        [BoxGroup("PopupView")]
        [ReadOnly]
        public RectTransform ViewPopupsParent;

        [BoxGroup("PopupView")]
        [LabelText("PopupView(Dynamic)")]
        public List<RectTransform> ViewPopups = new List<RectTransform>();
    }
}