// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using Sirenix.Serialization;
// using UnityEngine;
// using Logger = YIUIFramework.Logger;
//
// namespace YIUIBind
// {
//     [LabelText("UI 组件表")]
//     [AddComponentMenu("YIUIBind/Component Table")]
//     public sealed partial class UIBindComponentTable : SerializedMonoBehaviour
//     {
//         [FoldoutGroup("Component", 1)]
//         [OdinSerialize]
//         [LabelText("Runtime Components")]
//         [ReadOnly]
//         [PropertyOrder(-9)]
//         private Dictionary<string, Component> m_AllBindDic = new Dictionary<string, Component>();
//
//         public IReadOnlyDictionary<string, Component> AllBindDic => m_AllBindDic;
//
//         private Component FindComponent(string comName)
//         {
//             m_AllBindDic.TryGetValue(comName, out var value);
//             if (value == null)
//             {
//                 Logger.LogErrorContext(this, $" {name} 组件表中没有这个组件 {comName}");
//             }
//
//             return value;
//         }
//
//         public T FindComponent<T>(string comName) where T : Component
//         {
//             return FindComponent(comName) as T;
//         }
//     }
// }