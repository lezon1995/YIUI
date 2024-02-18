// #if UNITY_EDITOR
//
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Sirenix.OdinInspector;
// using Sirenix.Serialization;
// using UnityEngine;
// using YIUIFramework;
// using Logger = YIUIFramework.Logger;
//
// namespace YIUIBind
// {
//     public sealed partial class UIBindComponentTable
//     {
//         [FoldoutGroup("Component", 1)]
//         [OdinSerialize]
//         [LabelText("Editor Components")]
//         [HideReferenceObjectPicker]
//         [PropertyOrder(-10)]
//         [ShowIf("@UIOperationHelper.CommonShowIf()")]
//         private List<UIBindData> m_ComponentList = new List<UIBindData>();
//
//         [FoldoutGroup("Component", 1)]
//         [Button("Auto Name", 30)]
//         [PropertyOrder(-100)]
//         [ShowIf("@UIOperationHelper.CommonShowIf()")]
//         public void AutoCheck()
//         {
//             if (UIOperationHelper.CheckUIOperation(this))
//             {
//                 CheckComponentsName();
//             }
//         }
//
//         public void AddComponent(Component component)
//         {
//             if (UIOperationHelper.CheckUIOperation(this))
//             {
//                 m_ComponentList.Add(new UIBindData
//                 {
//                     GameObject = component.gameObject,
//                     Name = component.gameObject.name,
//                     Type = component.GetType(),
//                 });
//             }
//         }
//
//         /// <summary>
//         /// 检查所有绑定命名
//         /// 必须m_ 开头
//         /// 如果没用命名则使用对象的名字拼接
//         /// 会尝试强制修改
//         /// 如果还有同名则报错
//         /// </summary>
//         private void CheckComponentsName()
//         {
//             m_AllBindDic.Clear();
//             if (m_ComponentList.IsEmpty())
//             {
//                 return;
//             }
//
//             for (var i = 0; i < m_ComponentList.Count; i++)
//             {
//                 var data = m_ComponentList[i];
//                 var oldName = data.Name;
//                 var type = data.Type;
//                 var component = data.GameObject.GetComponent(type);
//                 if (component == null)
//                 {
//                     Logger.LogErrorContext(this, $"{name} 空对象  所以 {oldName} 已忽略");
//                     continue;
//                 }
//
//                 var newName = oldName;
//
//                 var cName = NameUtility.ComponentName;
//                 if (!oldName.CheckFirstName(cName))
//                 {
//                     if (newName.IsEmpty())
//                     {
//                         if (component)
//                         {
//                             newName = $"{NameUtility.FirstName}{cName}{component.name}";
//                         }
//                         else
//                         {
//                             continue;
//                         }
//                     }
//                     else
//                     {
//                         newName = $"{NameUtility.FirstName}{cName}{oldName}";
//                     }
//                 }
//
//                 newName = newName.ChangeToBigName(cName);
//
//                 if (oldName != newName)
//                 {
//                     data.Name = newName;
//                 }
//
//                 if (data.Name.IsEmpty())
//                 {
//                     Logger.LogErrorContext(this, $"{name} 存在空名称 {component.name} 已忽略");
//                     continue;
//                 }
//
//                 if (component == null)
//                 {
//                     Logger.LogErrorContext(this, $"{name} 空对象  所以 {data.Name} 已忽略");
//                     continue;
//                 }
//
//                 if (m_AllBindDic.ContainsValue(component))
//                 {
//                     Logger.LogErrorContext(component, $"{name} 这个组件已经存在了 重复对象 {component.name} 已忽略");
//                     continue;
//                 }
//
//                 if (m_AllBindDic.ContainsKey(data.Name))
//                 {
//                     Logger.LogErrorContext(component, $"{name} 这个命名已经存在了 重复添加 {data.Name} 已忽略");
//                     continue;
//                 }
//
//                 m_AllBindDic.Add(data.Name, component);
//             }
//         }
//     }
//
//     /// <summary>
//     /// 绑定数据对应关系
//     /// </summary>
//     [Serializable]
//     [HideLabel]
//     [HideReferenceObjectPicker]
//     internal sealed class UIBindData
//     {
//         [HideLabel]
//         [HorizontalGroup("UIBindData")]
//         public string Name;
//
//         [HideLabel]
//         [HorizontalGroup("UIBindData")]
//         [SerializeField]
//         public GameObject GameObject;
//
//         [HideLabel]
//         [HorizontalGroup("UIBindData")]
//         [ValueDropdown(nameof(GetComponentTypes))]
//         public Type Type;
//
//         private IEnumerable<Type> GetComponentTypes()
//         {
//             if (GameObject)
//             {
//                 return GameObject.GetComponents<Component>().Select(t => t.GetType());
//             }
//
//             return null;
//         }
//     }
// }
// #endif