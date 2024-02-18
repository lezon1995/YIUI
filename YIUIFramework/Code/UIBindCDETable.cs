﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    [Serializable]
    [LabelText("UI CDE总表")]
    [AddComponentMenu("YIUIBind/CDE Table")]
    public sealed partial class UIBindCDETable : SerializedMonoBehaviour
    {
        [ReadOnly]
        [HorizontalGroup("UIInfo", 0.2F)]
        [HideLabel]
        public string PkgName;

        [ReadOnly]
        [HorizontalGroup("UIInfo", 0.6F)]
        [HideLabel]
        public string ResName;

        #region 关联

        //关联的UI
        private UIBase m_UIBase;

        [OdinSerialize]
        [LabelText("编辑时所有公共组件")]
        [ReadOnly]
        [PropertyOrder(1000)] //生成UI类时使用
#if UNITY_EDITOR
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
#endif
        internal List<UIBindCDETable> ChildTables = new List<UIBindCDETable>();

        [OdinSerialize]
        [NonSerialized]
        [ShowInInspector]
        [ReadOnly]
        [PropertyOrder(1000)]
        [LabelText("运行时所有公共组件")] //动态生成后的子类(公共组件) 运行时使用
#if UNITY_EDITOR
        [HideIf("@UIOperationHelper.CommonShowIf()")]
#endif
        private Dictionary<string, UIBase> m_ChildUIBases = new Dictionary<string, UIBase>();

        internal void AddUIBase(string uiName, UIBase uiBase)
        {
            bool success = m_ChildUIBases.TryAdd(uiName, uiBase);

            if (success == false)
            {
                Debug.LogError($"{name} 已存在 {uiName} 请检查为何重复添加 是否存在同名组件");
            }
        }

        internal UIBase FindUIBase(string uiName)
        {
            if (m_ChildUIBases.TryGetValue(uiName, out var uiBase))
            {
                return uiBase;
            }

            Debug.LogError($"{name} 不存在 {uiName} 请检查");
            return null;
        }

        public T FindUIBase<T>(string uiName) where T : UIBase
        {
            return FindUIBase(uiName) as T;
        }

        #endregion
    }
}