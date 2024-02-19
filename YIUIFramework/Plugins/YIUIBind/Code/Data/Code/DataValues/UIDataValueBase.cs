using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    /// <summary>
    /// 数据基类
    /// 泛型 扩展请参考其他数据 注意满足需求
    /// 具体请查看文档
    /// @李胜扬
    /// </summary>
    [Serializable]
    public abstract class UIDataValueBase<T> : UIDataValue, IUIDataValue<T>
    {
        [OdinSerialize]
        [LabelText("Value")]
        [HideReferenceObjectPicker]
        [Delayed]
#if UNITY_EDITOR
        [OnValueChanged(nameof(OnValueChanged))]
#endif
        T m_Value;

#if UNITY_EDITOR
        void OnValueChanged()
        {
            InvokeValueChangeAction();
        }
#endif

        //基类中的事件 双参数 1 新值 2 老值
        event Action<T, T> m_OnValueChangeAction;

        public void AddValueChangeAction(Action<T, T> action)
        {
            m_OnValueChangeAction -= action;
            m_OnValueChangeAction += action;
        }

        public void RemoveValueChangeAction(Action<T, T> action)
        {
            m_OnValueChangeAction -= action;
        }

        public void InvokeValueChangeAction(T newValue, T oldValue)
        {
            try
            {
                m_OnValueChangeAction?.Invoke(newValue, oldValue);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                throw;
            }
        }

        public T GetValue()
        {
            return m_Value;
        }

        internal sealed override bool SetValueFrom(UIDataValue dataValue)
        {
            return SetValueFrom(dataValue, true); //必须使用强制刷新
        }

        public bool SetValueFrom(UIDataValue dataValue, bool force, bool notify = true)
        {
            if (dataValue == null)
            {
                Logger.LogError($"{typeof(T)} 失败，Value为空");
                return false;
            }

            if (dataValue is not UIDataValueBase<T>)
            {
                Logger.LogError($"失败，类型不一致 当前类型 {typeof(T)} 传入类型 {dataValue.UIDataValueType}");
                return false;
            }

            return SetValue(dataValue.GetValue<T>(), force, notify);
        }

        public bool SetValue(T value, bool force = false, bool notify = true)
        {
            if (!force && EqualsValue(value))
            {
                return false;
            }

            var oldValue = m_Value;
            SetValueFrom(value);
            InvokeValueChangeAction();
            if (notify)
            {
                InvokeValueChangeAction(value, oldValue);
            }

            return true;
        }

        protected virtual void SetValueFrom(T value)
        {
            m_Value = value;
        }

        protected abstract bool EqualsValue(T value);
    }
}