﻿using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    [ExecuteInEditMode]
    public abstract partial class UIEventBind : SerializedMonoBehaviour
    {
        [OdinSerialize]
        [ReadOnly]
        [HideReferenceObjectPicker]
        [Required("必须选择")]
        [HideLabel]
        [PropertyOrder(-999)]
        UIBindCDETable m_EventTable;

        public UIBindCDETable EventTable => m_EventTable;

        [OdinSerialize]
        [LabelText("Publish Event")]
#if UNITY_EDITOR
        [ValueDropdown(nameof(GetEventNameKeys))]
        [OnValueChanged(nameof(OnEventNameSelected))]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
#endif
        [PropertyOrder(-99)]
        protected string m_EventName;

        /// <summary>
        /// 当前的UI事件
        /// </summary>
        [OdinSerialize]
        protected UIEventBase m_UIEvent;

        UIEventBase GetEvent(string eventName)
        {
            if (eventName.IsEmpty())
            {
                //Logger.LogErrorContext(this,$"{name} 尝试获取一个空名称的事件 请检查");
                return null;
            }

            if (m_EventTable == null)
            {
                Logger.LogErrorContext(this, $"{name} 事件表==ull 请检查");
                return null;
            }

            var uiEvent = m_EventTable.FindEvent(eventName);
            if (uiEvent == null)
            {
                Logger.LogErrorContext(this, $"{name}没找到这个事件 {eventName} 请检查配置");
            }

            return uiEvent;
        }

        protected abstract List<EUIEventParamType> GetFilterParamType();

        bool m_Binded;

        internal void Initialize(bool refresh = false)
        {
            if (refresh || !m_Binded)
            {
                m_Binded = true;
                OnRefreshEvent();
            }
        }

        void RefreshEventName()
        {
            if (m_UIEvent != null)
            {
                m_EventName = m_UIEvent.EventName;
            }
        }

        protected virtual void RefreshBind()
        {
        }

        void OnRefreshEvent()
        {
            RefreshEventTable();
#if UNITY_EDITOR
            UnbindEvent();
#endif
            RefreshEventName();
            m_UIEvent = GetEvent(m_EventName);
            RefreshEventName();
#if UNITY_EDITOR
            BindEvent();
#endif
            RefreshBind();
        }

        void RefreshEventTable()
        {
            if (m_EventTable == null)
            {
                m_EventTable = this.GetComponentInParentHard<UIBindCDETable>();
            }
        }
    }
}