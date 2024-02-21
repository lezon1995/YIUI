using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    //Panel的分块数据
    public sealed partial class UITable
    {
        [OdinSerialize]
        [LabelText("源数据")]
        [ReadOnly]
#if !YIUIMACRO_BIND_RUNTIME_EDITOR
        [HideInInspector]
#endif
        internal bool IsSplitData;

        //源数据 拆分前的源数据
        [ShowInInspector]
        [HideLabel]
        [BoxGroup("面板拆分数据", centerLabel: true)]
        [OdinSerialize]
#if UNITY_EDITOR
        [ShowIf(nameof(ShowPanelSplitData))]
#endif
        internal UIPanelSplitData PanelSplitData = new UIPanelSplitData();

#if UNITY_EDITOR
        bool ShowPanelSplitData
        {
            get { return IsSplitData && UICodeType == UIType.Panel; }
        }

        //拆分后的引用数据 
        [ShowInInspector]
        [HideLabel]
        [BoxGroup("面板拆分数据", centerLabel: true)]
        [OdinSerialize]
        [HideIf(nameof(HidePanelSplitData))]
        [ReadOnly] //就是一个只读的 展示用数据 请不要使用此数据 或修改数据
        internal UIPanelSplitData PanelSplitEditorShowData;

        bool HidePanelSplitData
        {
            get { return IsSplitData || UICodeType != UIType.Panel; }
        }
#endif
    }
}