using System;
using UnityEngine;

namespace YIUIFramework
{
    //生命周期
    public sealed partial class UIBindCDETable
    {
        internal Action UIBaseOnEnable;
        internal Action UIBaseStart;
        internal Action UIBaseOnDisable;
        internal Action UIBaseOnDestroy;

        /// <summary>
        /// 关联UIBase
        /// 由UIBase初始化后调用
        /// 目前还没用 预留这里而已 不知道以后是否会有用
        /// </summary>
        internal void BindUIBase(UIBase uiBase)
        {
            if (m_UIBase != null)
            {
                Debug.LogError($"{gameObject.name} m_UIBase 已存在 请检查为何重复调用");
            }

            m_UIBase = uiBase;
        }

        private void Awake()
        {
            AwakeData();
            AwakeEvent();
        }

        private void OnEnable()
        {
            try
            {
                UIBaseOnEnable?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }


        private void Start()
        {
            try
            {
                UIBaseStart?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }


        private void OnDisable()
        {
            try
            {
                UIBaseOnDisable?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private void OnDestroy()
        {
            try
            {
                UIBaseOnDestroy?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }

            OnDestroyEvent();
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            OnValidateData();
            OnValidateEvent();
        }
#endif
    }
}