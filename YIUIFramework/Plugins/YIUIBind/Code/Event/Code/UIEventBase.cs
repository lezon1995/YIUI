using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIBind
{
    [Serializable]
    [HideReferenceObjectPicker]
    public abstract partial class UIEventBase
    {
        [LabelText("Event Name")]
        [SerializeField]
        [ReadOnly]
#if UNITY_EDITOR
        [InfoBox("The Event hasn't binded yet", InfoMessageType.Error, nameof(ShowIfBindsTips))]
        [ShowIf(nameof(ShowIfBindsTips))]
#endif
        private string m_EventName;

        public string EventName => m_EventName;

        [SerializeField]
        [ReadOnly]
        [LabelText("Param Types")]
#if UNITY_EDITOR
        [ShowIf(nameof(ShowIfAllEventParamType))]
#endif
        public List<EUIEventParamType> AllEventParamType = new List<EUIEventParamType>();

        public void RefreshAllEventParamType(List<EUIEventParamType> targetType)
        {
            AllEventParamType.Clear();
            AllEventParamType.AddRange(targetType);
        }

        public void SetName(string name)
        {
            m_EventName = name;
        }

        protected UIEventBase()
        {
        }

        protected UIEventBase(string name)
        {
            m_EventName = name;
        }

        public abstract bool Clear();
    }
}