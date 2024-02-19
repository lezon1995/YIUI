using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    public sealed partial class UIBindCDETable
    {
        [FoldoutGroup("Component", 1)]
        [OdinSerialize]
        [LabelText("Runtime Components")]
        [ReadOnly]
        [PropertyOrder(-9)]
        Dictionary<string, Component> m_AllBindDic = new Dictionary<string, Component>();

        public IReadOnlyDictionary<string, Component> AllBindDic => m_AllBindDic;

        Component FindComponent(string comName)
        {
            m_AllBindDic.TryGetValue(comName, out var value);
            if (value == null)
            {
                Logger.LogErrorContext(this, $" {name} 组件表中没有这个组件 {comName}");
            }

            return value;
        }

        public T FindComponent<T>(string comName) where T : Component
        {
            return FindComponent(comName) as T;
        }
    }
}