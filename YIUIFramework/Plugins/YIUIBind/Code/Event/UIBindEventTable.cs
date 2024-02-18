// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using Sirenix.Serialization;
// using UnityEngine;
// using YIUIFramework;
// using Logger = YIUIFramework.Logger;
//
// namespace YIUIBind
// {
//     [LabelText("UI 事件表")]
//     [AddComponentMenu("YIUIBind/Event Table")]
//     public sealed partial class UIBindEventTable : SerializedMonoBehaviour
//     {
//         [FoldoutGroup("Event", 3)]
//         [Delayed]
//         [Searchable]
//         [OdinSerialize]
//         [ShowInInspector]
//         [LabelText("Runtime Events")]
//         [DictionaryDrawerSettings(KeyLabel = "Event Name", ValueLabel = "事件内容", IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
//         private Dictionary<string, UIEventBase> m_EventDic = new Dictionary<string, UIEventBase>();
//
//         public IReadOnlyDictionary<string, UIEventBase> EventDic => m_EventDic;
//
//         public UIEventBase FindEvent(string eventName)
//         {
//             if (eventName.IsEmpty())
//             {
//                 Logger.LogError($"空的事件名称  请检查");
//                 return null;
//             }
//
//             if (m_EventDic.TryGetValue(eventName, out var value))
//             {
//                 return value;
//             }
//
//             return null;
//         }
//
//         public T FindEvent<T>(string eventName) where T : UIEventBase
//         {
//             return FindEvent(eventName) as T;
//         }
//
//         /// <summary>
//         /// 清除事件
//         /// </summary>
//         public bool ClearEvent(string eventName)
//         {
//             if (m_EventDic.TryGetValue(eventName, out var uiEvent))
//             {
//                 return uiEvent.Clear();
//             }
//
//             Logger.LogError($"没有这个事件定义{eventName}");
//             return false;
//         }
//
//         /// <summary>
//         /// 清除所有事件
//         /// 危险运行时没这个需求
//         /// </summary>
//         public void ClearAllEvents()
//         {
//             foreach (var uiEvent in m_EventDic.Values)
//             {
//                 uiEvent.Clear();
//             }
//
//             m_EventDic.Clear();
//         }
//
//         private void OnDestroy()
//         {
//             foreach (var uiEvent in m_EventDic.Values)
//             {
//                 uiEvent.Clear();
//             }
//
//             m_EventDic = null;
//         }
//
//         private void Awake()
//         {
//             InitEventTable();
//         }
//
//         #region 递归初始化所有绑定数据
//
//         private void InitEventTable()
//         {
//             InitializeBinds(transform);
//         }
//
//         private static void InitializeBinds(Transform transform)
//         {
// #if YIUIMACRO_BIND_INITIALIZE
//             Logger.LogErrorContext(transform,$"{transform.name} 初始化调用所有子类 UIEventBind 绑定");
// #endif
//
//             var binds = ListPool<UIEventBind>.Get();
//             transform.GetComponents(binds);
//             foreach (var bind in binds)
//             {
//                 bind.Initialize(true);
//             }
//
//             ListPool<UIEventBind>.Put(binds);
//
//             for (int i = 0; i < transform.childCount; i++)
//             {
//                 var child = transform.GetChild(i);
//                 InitializeBindsDeep(child);
//             }
//         }
//
//         private static void InitializeBindsDeep(Transform transform)
//         {
//             if (transform.HasComponent<UIBindCDETable>())
//             {
//                 return;
//             }
//
//             var binds = ListPool<UIEventBind>.Get();
//             transform.GetComponents(binds);
//             foreach (var bind in binds)
//             {
//                 bind.Initialize(true);
//             }
//
//             ListPool<UIEventBind>.Put(binds);
//
//             for (int i = 0; i < transform.childCount; i++)
//             {
//                 var child = transform.GetChild(i);
//                 InitializeBindsDeep(child);
//             }
//         }
//
//         #endregion
//     }
// }