using System;
using Sirenix.OdinInspector;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    [Serializable]
    [HideLabel]
    public abstract class UIDataValue
    {
        [ShowInInspector]
        [PropertyOrder(-99)]
        public abstract EUIBindDataType UIBindDataType { get; }

        public abstract Type UIDataValueType { get; }

        /// <summary>
        /// 从另一个Value设置数据
        /// </summary>
        internal abstract bool SetValueFrom(UIDataValue dataValue);

        //值改变消息 无参
        private event Action m_OnValueChangeAction;

        internal void AddValueChangeAction(Action action)
        {
            m_OnValueChangeAction -= action;
            m_OnValueChangeAction += action;
        }

        internal void RemoveValueChangeAction(Action action)
        {
            m_OnValueChangeAction -= action;
        }

        internal void InvokeValueChangeAction()
        {
            try
            {
                m_OnValueChangeAction?.Invoke();
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                throw;
            }
        }
    }
}