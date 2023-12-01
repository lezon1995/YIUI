using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YIUIBind
{
    /// <summary>
    /// 点击事件绑定
    /// 与按钮无关
    /// 只要是任何可以被射线检测的物体都可以响应点击事件
    /// 额外多一个String 参数自定义
    /// </summary>
    [InfoBox("提示: 可用事件参数 1个")]
    [LabelText("Click<string>")]
    [AddComponentMenu("YIUIBind/Event/点击 【Click String】 UIEventBindClickString")]
    public class UIEventBindClickString : UIEventBindClick
    {
        [SerializeField]
        [LabelText("Parameter<string>")]
        private string m_ExtraParam;

        [NonSerialized]
        private List<EUIEventParamType> m_FilterParamType = new List<EUIEventParamType>
        {
            EUIEventParamType.String,
        };

        protected override List<EUIEventParamType> GetFilterParamType()
        {
            return m_FilterParamType;
        }

        protected override void OnUIEvent(PointerEventData eventData)
        {
            m_UIEvent?.Invoke(m_ExtraParam);
        }
    }
}