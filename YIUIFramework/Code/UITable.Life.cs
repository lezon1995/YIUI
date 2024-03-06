using System;
using UnityEngine;

namespace YIUIFramework
{
    //生命周期
    public sealed partial class UITable
    {
        internal Action onEnable;
        internal Action onStart;
        internal Action onDisable;
        internal Action onDestroy;

        /// <summary>
        /// 关联UIBase
        /// 由UIBase初始化后调用
        /// 目前还没用 预留这里而已 不知道以后是否会有用
        /// </summary>
        internal void BindUI(UIBase ui)
        {
            if (UI)
            {
                Debug.LogError($"{gameObject.name} m_UIBase 已存在 请检查为何重复调用");
            }

            UI = ui;
        }

        void OnEnable()
        {
            try
            {
                onEnable?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        void Start()
        {
            try
            {
                onStart?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        void OnDisable()
        {
            try
            {
                onDisable?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        void OnDestroy()
        {
            try
            {
                onDestroy?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}