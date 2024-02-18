// #if UNITY_EDITOR
//
// using System.Collections.Generic;
// using System.Linq;
// using Sirenix.OdinInspector;
// using Sirenix.Serialization;
// using UnityEngine;
// using YIUIFramework;
//
// namespace YIUIBind
// {
//     public sealed partial class UIBindEventTable
//     {
//         [FoldoutGroup("Event", 3)]
//         [HorizontalGroup("Event/Horizontal", 0.4F)]
//         [ShowInInspector]
//         [HideReferenceObjectPicker]
//         [HideLabel]
//         [PropertyOrder(-99)]
//         [Delayed]
//         [ShowIf("@UIOperationHelper.CommonShowIf()")]
//         private string m_AddUIEventName = "";
//
//         [FoldoutGroup("Event", 3)]
//         [HorizontalGroup("Event/Horizontal", 0.4F)]
//         [PropertyOrder(-98)]
//         [OdinSerialize]
//         [ShowInInspector]
//         [LabelText("Param Type")]
//         [ShowIf("@UIOperationHelper.CommonShowIf()")]
//
//         //[ListDrawerSettings(DraggableItems = false,lsyExpanded = false,ShowIndexLabels = true,ShowPaging = false,ShowItemCount = false,HideRemoveButton = true)]
//         public List<EUIEventParamType> AllEventParamType = new List<EUIEventParamType>();
//
//         [FoldoutGroup("Event", 3)]
//         [HorizontalGroup("Event/Horizontal", 0.2F)]
//         [GUIColor(0, 1, 0)]
//         [Button("Add")]
//         [PropertyOrder(-98)]
//         [ShowIf("@UIOperationHelper.CommonShowIf()")]
//         private void AddNewUIEvent()
//         {
//             if (m_AddUIEventName.IsEmpty())
//             {
//                 UnityTipsHelper.ShowError($"必须填写名称才可以添加");
//                 return;
//             }
//
//             if (m_EventDic.ContainsKey(m_AddUIEventName))
//             {
//                 UnityTipsHelper.ShowError($"已存在同名数据  请修改 {m_AddUIEventName}");
//                 return;
//             }
//
//             var uiEventBase = UIEventBaseHelper.CreatorUIEventBase(m_AddUIEventName, AllEventParamType);
//             if (uiEventBase == null)
//             {
//                 UnityTipsHelper.ShowError($"创建失败 {m_AddUIEventName}");
//                 return;
//             }
//
//             m_EventDic.Add(m_AddUIEventName, uiEventBase);
//
//             AllEventParamType.Clear();
//             m_AddUIEventName = "";
//         }
//
//         private void RemoveCallBack(UIEventBase uiEvent)
//         {
//             uiEvent.OnRemoveVariableCallBack();
//             m_EventDic.Remove(uiEvent.EventName);
//         }
//
//         private void OnRemoveUIEvent(UIEventBase uiEvent)
//         {
//             if (!m_EventDic.ContainsValue(uiEvent))
//             {
//                 UnityTipsHelper.ShowError($"不存在无法移除 {uiEvent.EventName}  请检查是否有配置错误");
//                 return;
//             }
//
//             //如果已经有绑定了 需要提醒是否移除
//             if (uiEvent.GetBindCount() >= 1)
//             {
//                 var callBackTips = $"{uiEvent.EventName} 已绑定 {uiEvent.GetBindCount()}个目标\n移除会强制清楚所有绑定 请确认是否需要移除!!!";
//                 UnityTipsHelper.CallBack(callBackTips, () => { RemoveCallBack(uiEvent); });
//                 return;
//             }
//
//             RemoveCallBack(uiEvent);
//         }
//
//         public List<string> GetAllEventName()
//         {
//             var allName = new List<string>();
//             foreach (var name in m_EventDic.Keys)
//             {
//                 allName.Add(name);
//             }
//
//             return allName;
//         }
//
//         public List<string> GetFilterParamTypeEventName(List<EUIEventParamType> list)
//         {
//             var allName = new List<string>();
//             foreach (var eventValue in m_EventDic.Values)
//             {
//                 if (eventValue.AllEventParamType.ParamEquals(list))
//                 {
//                     allName.Add(eventValue.EventName);
//                 }
//             }
//
//             return allName;
//         }
//
//         private void OnValidate()
//         {
//             if (m_EventDic == null)
//             {
//                 return;
//             }
//
//             if (UIOperationHelper.IsPlaying())
//             {
//                 return;
//             }
//
//             foreach (var (eventName, uiEvent) in m_EventDic)
//             {
//                 uiEvent.ClearBinds();
//                 uiEvent.ChangeName(eventName);
//                 uiEvent.OnRemoveAction = OnRemoveUIEvent;
//             }
//
//             InitEventTable();
//         }
//     }
// }
//
// #endif