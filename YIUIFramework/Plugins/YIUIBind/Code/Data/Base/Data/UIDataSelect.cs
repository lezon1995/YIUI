﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIBind
{
    /// <summary>
    /// 数据选择数据展示
    /// </summary>
    [Serializable]
    [HideLabel]
    [HideReferenceObjectPicker]
    public class UIDataSelect
    {
        [LabelText("Name")]
        [ReadOnly]
        string m_DataName;

        [ShowInInspector]
        [LabelText("Type")]
        [ReadOnly]
        EUIBindDataType m_DataType;

#if UNITY_EDITOR
        [ShowInInspector]
        [LabelText("Value")]
        [ReadOnly]
        string m_DataValue;
#endif

        [SerializeField]
        [HideInInspector]
        UIData m_Data;

        public UIData Data => m_Data;

        UIDataSelect()
        {
        }

        public UIDataSelect(UIData uiData)
        {
            RefreshData(uiData);
        }

        public void RefreshData(UIData uiData)
        {
            m_Data = uiData;
            m_DataName = m_Data.Name;
            m_DataType = m_Data.DataValue.UIBindDataType;
#if UNITY_EDITOR
            m_DataValue = m_Data.GetValueToString();
#endif
        }

        public void ChangeName(string name)
        {
            m_DataName = name;
        }
    }
}