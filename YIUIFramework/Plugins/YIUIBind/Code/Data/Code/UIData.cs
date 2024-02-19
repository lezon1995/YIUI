using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIBind
{
    /// <summary>
    /// 添加UIData前的准备数据
    /// </summary>
    public sealed class UINewData
    {
        [Delayed]
        [HideLabel]
        [HorizontalGroup("UINewData")]
        public string Name;

        [HideLabel]
        [HorizontalGroup("UINewData")]
        [ValueDropdown(nameof(GetFilteredTypeList))]
        public Type Type;

        public IEnumerable<Type> GetFilteredTypeList()
        {
            var q = typeof(UIDataValue).Assembly.GetTypes()
                // Excludes BaseClass
                .Where(x => !x.IsAbstract)
                // Excludes C1<>
                .Where(x => !x.IsGenericTypeDefinition)
                // Excludes classes not inheriting from BaseClass
                .Where(x => typeof(UIDataValue).IsAssignableFrom(x));

            return q;
        }
    }

    [Serializable]
    [HideLabel]
    [HideReferenceObjectPicker]
    public sealed partial class UIData
    {
        /// <summary>
        /// 当前变量名称
        /// </summary>
        public string Name { get; private set; }

#if UNITY_EDITOR
        [InfoBox("The data hasn't binded yet", InfoMessageType.Error, nameof(ShowIfBindsTips))]
#endif
        [SerializeField]
        [ReadOnly]
        [LabelText("Guid")]
        int m_DataGuid;

        public int Guid => m_DataGuid;

        [OdinSerialize]
        UIDataValue m_DataValue;

        public UIDataValue DataValue => m_DataValue;

        UIData()
        {
        }

        public UIData(string name, UIDataValue dataValue)
        {
            Name = name;
            m_DataValue = dataValue;
            m_DataGuid = System.Guid.NewGuid().GetHashCode();
        }
    }
}