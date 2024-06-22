using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    //Panel的分块数据
    public sealed partial class UITable
    {
        [LabelText("源数据"), OdinSerialize, ReadOnly]
#if !YIUIMACRO_BIND_RUNTIME_EDITOR
        [HideInInspector]
#endif
        internal bool IsSplitData;

        //源数据 拆分前的源数据
        [ShowInInspector, HideLabel, OdinSerialize, BoxGroup("面板拆分数据", centerLabel: true)]
#if UNITY_EDITOR
        [ShowIf(nameof(ShowPanelSplitData))]
#endif
        internal UIPanelSplitData PanelSplitData = new UIPanelSplitData();

#if UNITY_EDITOR
        bool ShowPanelSplitData => IsSplitData && UICodeType == UIType.Panel;

        [ShowInInspector, HideLabel, OdinSerialize, BoxGroup("面板拆分数据", centerLabel: true)]
        //拆分后的引用数据 就是一个只读的 展示用数据 请不要使用此数据 或修改数据
        [ReadOnly, HideIf(nameof(HidePanelSplitData))]
        internal UIPanelSplitData PanelSplitEditorShowData;

        bool HidePanelSplitData => IsSplitData || UICodeType != UIType.Panel;
#endif
    }
}