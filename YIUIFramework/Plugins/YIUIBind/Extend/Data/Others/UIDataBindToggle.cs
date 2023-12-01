using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIBind
{
    [DetailedInfoBox("可选择的Toggle/Button ...类的",
        @"改变是否可触摸 Toggle / inputField / button / dropdown 这些都可以用 
在某些情况下不允许点击时  灰色点击那种 就可以用这个")]
    [RequireComponent(typeof(Toggle))]
    [LabelText("Toggle")]
    [AddComponentMenu("YIUIBind/Data/开关 【Toggle】 UIDataBindSelectable")]
    public sealed class UIDataBindToggle : UIDataBindBool
    {
        [SerializeField]
        [ReadOnly]
        [Required("必须有此组件")]
        [LabelText("Toggle")]
        private Toggle m_Toggle;

        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            if (m_Toggle == null)
            {
                m_Toggle = GetComponent<Toggle>();
            }
        }

        protected override void OnValueChanged()
        {
            if (m_Toggle)
            {
                m_Toggle.isOn = GetResult();
            }
        }
    }
}