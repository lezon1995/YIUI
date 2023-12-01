using Sirenix.OdinInspector;
using UnityEngine;
using YIUIFramework;

namespace YIUIBind
{
    [LabelText("置灰")]
    [RequireComponent(typeof(UIGrayscale))]
    [AddComponentMenu("YIUIBind/Data/置灰 【Gray】 UIDataBindGray")]
    public sealed class UIDataBindGray : UIDataBindBool
    {
        [SerializeField]
        [Range(0, 255)]
        private int m_EnabledGray;

        [SerializeField]
        [Range(0, 255)]
        private int m_DisabledGray = 255;

        [SerializeField]
        [ReadOnly]
        [Required("必须有此组件")]
        [LabelText("UI灰度")]
        private UIGrayscale m_Grayscale;

        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            if (m_Grayscale == null)
            {
                m_Grayscale = GetComponent<UIGrayscale>();
            }
        }

        protected override void OnValueChanged()
        {
            if (m_Grayscale)
            {
                m_Grayscale.GrayScale = GetResult() ? m_EnabledGray : m_DisabledGray;
            }
        }
    }
}