#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    public abstract partial class UIEventBase
    {
        bool ShowIfAllEventParamType => AllEventParamType.Count >= 1;

        protected string GetParamTypeString(int index)
        {
            var current = AllEventParamType[index];
            return current.GetParamTypeString();
        }

        public abstract string GetEventType();
        public abstract string GetEventHandleType();

        public void ChangeName(string eventName)
        {
            m_EventName = eventName;
        }

        //移除注册
        [HideInInspector]
        public Action<UIEventBase> OnRemoveAction;

        //确定移除
        public void OnRemoveVariableCallBack()
        {
            if (m_Binds.IsEmpty())
            {
                return;
            }

            for (var i = m_Binds.Count - 1; i >= 0; i--)
            {
                m_Binds[i].RemoveBind(this);
            }
        }

        [GUIColor(1, 0, 0)]
        [Button("Remove")]
        [PropertyOrder(-99)]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        void RemoveEvent()
        {
            try
            {
                OnRemoveAction?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                throw;
            }
        }

        bool ShowIfBinds => m_Binds.Count >= 1;
        bool ShowIfBindsTips => m_Binds.Count <= 0;

        [ShowInInspector]
        [SerializeField]
        [LabelText("Published By")]
        [ReadOnly]
        [ShowIf(nameof(ShowIfBinds))]
        List<UIEventBind> m_Binds = new List<UIEventBind>();

        public int GetBindCount()
        {
            return m_Binds.Count;
        }

        internal void AddBind(UIEventBind bind)
        {
            if (m_Binds.IndexOf(bind) == -1)
            {
                m_Binds.Add(bind);
            }
        }

        internal void RemoveBind(UIEventBind bind)
        {
            m_Binds.Remove(bind);
        }

        internal void ClearBinds()
        {
            m_Binds.Clear();
        }
    }
}
#endif